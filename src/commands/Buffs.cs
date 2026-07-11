using System;
using System.Linq;
using MoreCommands.Common;

namespace MoreCommands.Commands;

public sealed class BuffsCommand : CommandBase
{
    private static readonly string[] EffectColors =
    [
        "#7DD3FC", // blue
        "#86EFAC", // green
        "#FDE047", // yellow
        "#D8B4FE", // purple
        "#FDA4AF", // red
        "#67E8F9", // cyan
        "#FDBA74", // orange
        "#C4B5FD", // violet
    ];

    public override string[] Aliases => ["buffs"];
    public override CommandTag Tag => CommandTag.Player;
    public override string Description => "List active player buffs, colored effects, and unambiguous possible item sources";
    public override bool EnablesCheatsOnUse => false;

    protected override bool TryValidateArguments(string[] args, out string error)
    {
        bool valid = args.Length <= 1;
        error = valid ? null : "Usage: buffs [container or effect filter]";
        return valid;
    }

    public override void ConfigureBuilder(CommandConsole.CommandBuilder builder)
    {
        builder
            .AutocompleteCustom(AutocompleteBuff)
            .AutocompleteValidator(AutocompleteHelpers.ValidateOptionalSingleFilter);
    }

    public override void Execute(string[] args)
    {
        string filter = args.FirstOrDefault() ?? "";
        BuffContainer[] buffs = [.. ENT_Player.playerObject.curBuffs.currentBuffs
            .Where(buff => Matches(buff, filter))];

        if (buffs.Length == 0)
        {
            Accessors.CommandConsoleAccessor.EchoToConsole(
                string.IsNullOrWhiteSpace(filter) ? "No active buffs" : "No matching buffs");
            return;
        }

        string heading = buffs.Length == 1 ? "Active buff:" : $"Active buffs ({buffs.Length}):";
        Accessors.CommandConsoleAccessor.EchoToConsole(
            $"{heading}\n{string.Join("\n\n", buffs.Select(FormatBuff))}");
    }

    private static bool Matches(BuffContainer container, string filter)
    {
        return string.IsNullOrWhiteSpace(filter)
            || Helpers.Substr(container.id, filter)
            || container.buffs.Any(buff => Helpers.Substr(buff.id, filter));
    }

    private static string FormatBuff(BuffContainer container)
    {
        string source = FindPossibleSource(container);
        string id = string.IsNullOrWhiteSpace(container.id) ? "(unnamed)" : container.id;
        string sourceLine = string.IsNullOrWhiteSpace(source)
            ? ""
            : $"\n  Possible source: {Colors.Tagged(source, Colors.C_WORLD)}";
        string duration = container.loseOverTime
            ? $"{Math.Max(0f, container.buffTime) * 100f:0.#}% remaining"
            : "permanent";
        string effects = container.buffs.Count == 0
            ? "    (none)"
            : string.Join("\n", container.buffs.Select(FormatEffect));

        return $"- {Colors.Tagged(id, Colors.C_PLAYER)}{sourceLine}\n"
            + $"  Duration: {duration} | Multiplier: {container.multiplier:0.##}x\n"
            + $"  Effects:\n{effects}";
    }

    private static string FormatEffect(BuffContainer.Buff effect, int index)
    {
        string id = string.IsNullOrWhiteSpace(effect.id) ? "(unnamed)" : effect.id;
        string coloredId = $"<color={EffectColors[index % EffectColors.Length]}>{id}</color>";
        return $"    - {coloredId}: {effect.amount:0.##} / {effect.maxAmount:0.##}";
    }

    private static string FindPossibleSource(BuffContainer container)
    {
        string source = null;
        foreach (UnityEngine.GameObject prefab in CL_AssetManager.GetFullCombinedAssetDatabase().itemPrefabs)
        {
            Item item = prefab?.GetComponent<Item_Object>()?.itemData;
            if (item?.handItemAsset is not HandItem_Buff buffItem) continue;

            bool matches = HasSameDefinition(buffItem.buff, container)
                || (buffItem.useSecondaryBuffs
                    && (buffItem.secondaryBuffs?.Any(buff => HasSameDefinition(buff, container)) ?? false));
            if (!matches) continue;

            string candidate = item.prefabName ?? prefab.name;
            if (string.IsNullOrWhiteSpace(candidate)) continue;
            if (source != null && !string.Equals(source, candidate, StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }

            source = candidate;
        }

        return source;
    }

    private static bool HasSameDefinition(BuffContainer template, BuffContainer active)
    {
        if (template == null
            || active == null
            || !string.Equals(template.id, active.id, StringComparison.Ordinal)
            || !string.Equals(template.desc, active.desc, StringComparison.Ordinal)
            || template.loseOverTime != active.loseOverTime
            || template.loseRate != active.loseRate
            || template.loseRateEffectedByPerks != active.loseRateEffectedByPerks
            || template.buffs == null
            || active.buffs == null
            || template.buffs.Count != active.buffs.Count)
        {
            return false;
        }

        for (int index = 0; index < template.buffs.Count; index++)
        {
            BuffContainer.Buff expected = template.buffs[index];
            BuffContainer.Buff actual = active.buffs[index];
            if (expected == null
                || actual == null
                || !string.Equals(expected.id, actual.id, StringComparison.Ordinal)
                || expected.maxAmount != actual.maxAmount)
            {
                return false;
            }
        }

        return true;
    }

    private static void AutocompleteBuff(CommandConsole.CommandAutocomplete autocomplete)
    {
        if (autocomplete.activeArg == 0)
        {
            autocomplete.FromArray(ActiveBuffTerms());
            return;
        }

        autocomplete.Reject();
    }

    private static string[] ActiveBuffTerms()
    {
        var currentBuffs = ENT_Player.playerObject?.curBuffs?.currentBuffs;
        if (currentBuffs == null) return [];

        return [.. currentBuffs
            .SelectMany(buff => buff.buffs.Select(effect => effect.id).Prepend(buff.id))
            .Where(value => !string.IsNullOrWhiteSpace(value))
            .Distinct(StringComparer.OrdinalIgnoreCase)];
    }
}

public sealed class ClearBuffsCommand : CommandBase
{
    public override string[] Aliases => ["clearbuffs"];
    public override CommandTag Tag => CommandTag.Player;
    public override string Description => "Clear MoreCommands buffs by default, all buffs, or one exact container";
    public override bool EnablesCheatsOnUse => true;

    protected override bool TryValidateArguments(string[] args, out string error)
    {
        bool valid = args.Length <= 1
            && (args.Length == 0 || IsValidMode(args[0]));
        error = valid ? null : "Usage: clearbuffs [mc|all|exact container ID]";
        return valid;
    }

    public override void ConfigureBuilder(CommandConsole.CommandBuilder builder)
    {
        builder
            .AutocompleteCustom(AutocompleteMode)
            .AutocompleteValidator(ValidateMode);
    }

    public override void Execute(string[] args)
    {
        string mode = args.FirstOrDefault() ?? "mc";
        var playerBuffs = ENT_Player.playerObject.curBuffs;
        BuffContainer[] toRemove = [.. playerBuffs.currentBuffs
            .Where(buff => ShouldRemove(buff, mode))];

        foreach (BuffContainer buff in toRemove)
        {
            playerBuffs.RemoveBuffContainer(buff);
        }

        if (toRemove.Any(IsMoreCommandsBuff)
            && CommandRegistry.GetCommand<BuffCommand>() is ITogglableCommand buffCommand)
        {
            buffCommand.Enabled = false;
        }

        playerBuffs.UpdateBuffs();
        string containerLabel = toRemove.Length == 1 ? "container" : "containers";
        Accessors.CommandConsoleAccessor.EchoToConsole($"Cleared {toRemove.Length} buff {containerLabel}");
    }

    private static bool ShouldRemove(BuffContainer buff, string mode)
    {
        if (string.Equals(mode, "all", StringComparison.OrdinalIgnoreCase)) return true;
        if (string.Equals(mode, "mc", StringComparison.OrdinalIgnoreCase)) return IsMoreCommandsBuff(buff);
        return string.Equals(buff.id, mode, StringComparison.OrdinalIgnoreCase);
    }

    private static bool IsMoreCommandsBuff(BuffContainer buff)
    {
        return buff.id?.StartsWith(MyPluginInfo.PLUGIN_GUID, StringComparison.OrdinalIgnoreCase) ?? false;
    }

    private static void AutocompleteMode(CommandConsole.CommandAutocomplete autocomplete)
    {
        if (autocomplete.activeArg == 0)
        {
            var currentBuffs = ENT_Player.playerObject?.curBuffs?.currentBuffs;
            string[] suggestions = ["mc", "all", .. (currentBuffs ?? [])
                .Select(buff => buff.id)
                .Where(id => !string.IsNullOrWhiteSpace(id))
                .Distinct(StringComparer.OrdinalIgnoreCase)];
            autocomplete.FromArray(suggestions);
            return;
        }

        autocomplete.Reject();
    }

    private static void ValidateMode(CommandConsole.CommandValidator validator)
    {
        string value = validator.ArgumentAt(validator.activeArg);
        if (validator.activeArg != 0 || !IsValidMode(value))
        {
            validator.Reject();
        }
    }

    private static bool IsValidMode(string mode)
    {
        return string.Equals(mode, "mc", StringComparison.OrdinalIgnoreCase)
            || string.Equals(mode, "all", StringComparison.OrdinalIgnoreCase)
            || HasExactContainer(mode);
    }

    private static bool HasExactContainer(string id)
    {
        return ENT_Player.playerObject?.curBuffs?.currentBuffs?.Any(buff =>
            string.Equals(buff.id, id, StringComparison.OrdinalIgnoreCase)) ?? false;
    }
}
