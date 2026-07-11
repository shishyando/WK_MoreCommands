
namespace MoreCommands.Common;

public static class PerkChanger
{
    public static void MaxOutPerk(string perkId)
    {
        ENT_Player player = ENT_Player.playerObject;
        if (player == null) return;
        Perk old = player.GetPerk(perkId);
        if (old != null)
        {
            int toAdd = old.stackMax - old.stackAmount;
            if (toAdd > 0) old.AddStack(toAdd);
            return;
        }

        Perk template = CL_AssetManager.GetPerkAsset(perkId, "");
        if (template != null)
        {
            if (template.stackMax > 0) player.AddPerk(template, template.stackMax);
            return;
        }

        Plugin.Beep.LogWarning($"Perk {perkId} not found!");
    }
}
