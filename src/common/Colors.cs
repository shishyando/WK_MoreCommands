using HarmonyLib;
using MoreCommands.Common;
using UnityEngine;

public static class Colors
{
    public static Color HIGHLIGHT = new(0.25f, 0.45f, 0.5f);

    public static Color C_PLAYER = new(0.68f, 0.95f, 0.96f);
    public static Color C_WORLD = new(0.73f, 0.51f, 0.21f);
    public static Color C_CONSOLE = new(0.59f, 0.23f, 0.82f);

    public static Color WHITE = Color.white;
    public static Color MAGENTA = Color.magenta;
    public static Color CYAN = Color.cyan;
    public static Color LIGHT_GREEN =  new(0.3f, 0.9f, 0.3f);
    public static Color BRIGHT_YELLOW =  new(1f, 1f, 0f, 0.8f);
    public static Color BROWN = new(0.6f, 0.3f, 0);
    public static Color REDDISH = new(0.78f, 0.13f, 0.25f);

    private const string CHEAT_SIGN = "<color=orange>+</color>";
    public const string COMMAND_SEP = "<color=grey>---------------------</color>";
    
    public static string Str(Color c)
    {
        return $"#{ColorUtility.ToHtmlStringRGB(c)}";
    }

    public static string Tagged(string s, Color c)
    {
        return $"<color={Str(c)}>{s}</color>";
    }

    public static string Highlighted(string s)
    {
        return Tagged(s, HIGHLIGHT);
    }

    public static string FormatCommand(ICommand c)
    {
        Color color = c.Tag switch
        {
            CommandTag.Player => C_PLAYER,
            CommandTag.World => C_WORLD,
            CommandTag.Console => C_CONSOLE,
            _ => WHITE,
        };
        string prefix = c.CheatsOnly ? CHEAT_SIGN : "-";

        return Tagged($"{prefix} {c.Aliases.Join()}:\n{c.Description}", color);
    }

}
