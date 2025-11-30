using HarmonyLib;
using MoreCommands.Common;
using UnityEngine;

public static class Colors
{
    public static Color HIGHLIGHT = new(0.25f, 0.45f, 0.5f); // dark cyan

    public static Color C_PLAYER = new(0.68f, 0.95f, 0.96f); // cyanish
    public static Color C_WORLD = new(0.929f,0.855f,0.714f); // light brownish
    public static Color C_CONSOLE = new(0.898f,0.796f,0.949f); // Light grayish violet
    public static Color C_DESCRIPTION = new(0.792f,0.792f,0.792f); // light grey

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

        return $"{prefix} {Tagged(c.Aliases.Join(), color)}:\n{Tagged(c.Description, C_DESCRIPTION)}";
        // return Tagged($"{prefix} {c.Aliases.Join()}:\n{c.Description}", color);
    }

}
