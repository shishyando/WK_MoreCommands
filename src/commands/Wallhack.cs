using System;
using MoreCommands.Common;
using MoreCommands.Outlines;
using UnityEngine;

namespace MoreCommands.Commands;


public sealed class WallhackCommand : CommandBase
{
    public override string[] Aliases => ["wallhack", "wh"];
    public override CommandTag Tag => CommandTag.World;
    public override string Description => "Add outlines to any game entity\n wh [entity] (optional color like 'red', 'green' or '#RRGGBB')\nentity is searched by substring\nwithout params will toggle default behaviour";
    public override bool CheatsOnly => true;

    public override Action<string[]> GetLogicCallback()
    {
        return args =>
        {
            if (args.Length == 0)
            {
                bool enabled = OutlinesController.ToggleDefault();
                if (enabled) Accessors.CommandConsoleAccessor.EchoToConsole($"Switching wallhacks to default mode");
                else Accessors.CommandConsoleAccessor.EchoToConsole($"Switching wallhacks off");
                return;
            }
            string entityIdLower = Prefabs.Entities().Filter(args[0]).AnyName();
            if (entityIdLower == null)
            {
                Accessors.CommandConsoleAccessor.EchoToConsole($"No such entity found: {args[0]}");
                return;
            }

            if (OutlinesController.IsEnabled(entityIdLower))
            {
                OutlinesController.DisableOutlines(entityIdLower);
                Accessors.CommandConsoleAccessor.EchoToConsole($"Disabled outlines for: {entityIdLower}");
                return;
            }

            Color c = UnityEngine.Random.ColorHSV();
            if (args.Length == 2 && !ColorUtility.TryParseHtmlString(args[1], out c))
            {
                Accessors.CommandConsoleAccessor.EchoToConsole($"Failed to parse color {args[1]}");
                return;
            }

            c = OutlinesController.EnableOutlines(entityIdLower, c);
            Accessors.CommandConsoleAccessor.EchoToConsole($"Enabled outlines for {Colors.Tagged(entityIdLower, c)}, color: {Colors.Tagged(Colors.Str(c), c)}");
        };
    }

    public override void OnExit()
    {
        OutlinesController.ClearAll();
    }
}
