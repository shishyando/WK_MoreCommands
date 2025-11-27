using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using HarmonyLib;
using MoreCommands.Common;

namespace MoreCommands.Commands;


public sealed class ManCommand : CommandBase
{
    public override string[] Aliases => ["man", "mhelp", "morecommandshelp"];
    public override CommandTag Tag => CommandTag.Console;
    public override string Description => "prints MoreCommands with their descriptions ('+' = enables cheats)";
    public override bool CheatsOnly => false;

    protected override Action<string[]> GetLogicCallback()
    {
        return args =>
        {
            foreach (ICommand c in CommandRegistry.GetAllCommands())
            {
                if (args.Length == 0 || c.Aliases.Contains(args[0]))
                {
                    Accessors.CommandConsoleAccessor.EchoToConsole(Colored($"{(c.CheatsOnly ? CHEAT_SIGN : "-")} {c.Aliases.Join()}:\n{c.Description}", ColorByTag(c.Tag)));
                }
            }
        };
    }

    private const string CHEAT_SIGN = "<color=orange>+</color>";

    private static string Colored(string m, string color)
    {
        return $"<color={color}>{m}</color>";
    }


    private static string ColorByTag(CommandTag tag)
    {
        return tag switch
        {
            CommandTag.Player => "#adf2f5ff",
            CommandTag.World => "#bb8135ff",
            CommandTag.Console => "#973bd1ff",
            _ => "white",
        };
    }
}
