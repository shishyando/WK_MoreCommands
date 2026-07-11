using System;
using System.Collections.Generic;
using BepInEx.Configuration;
using TMPro;
using UnityEngine;

namespace MoreCommands.Common;

public static class TimerStyleController
{
    public sealed class TimerStyle
    {
        public Color FaceColor;
        public Color OutlineColor;
        public float OutlineWidth;
        public float FontSize;
        public float CharacterSpacing;
        public float Alpha;
        public FontStyles FontStyle;

        public TimerStyle Copy()
        {
            return (TimerStyle)MemberwiseClone();
        }
    }

    private const string Section = "TimerStyle";
    private const float UnsetFloat = -9999f;
    private static ConfigFile Config;
    private static ConfigEntry<string> Preset;
    private static ConfigEntry<string> FaceColor;
    private static ConfigEntry<string> OutlineColor;
    private static ConfigEntry<float> OutlineWidth;
    private static ConfigEntry<float> FontSize;
    private static ConfigEntry<float> CharacterSpacing;
    private static ConfigEntry<float> Alpha;
    private static ConfigEntry<string> FontStyle;
    private static TimerStyle VanillaStyle;
    private static TMP_Text VanillaTimer;

    public static readonly string[] StyleNames = ["normal", "bold", "italic", "bolditalic", "underline", "strikethrough"];

    private static readonly Dictionary<string, Func<TimerStyle, TimerStyle>> Presets = new(StringComparer.OrdinalIgnoreCase)
    {
        ["default"] = original => original.Copy(),
        ["minimal"] = original => From("#F4F0E8", "#18212B", 0.28f, original.FontSize, 1f, 0.96f, FontStyles.Normal),
        ["ice"] = original => From("#D9F7FF", "#174A73", 0.38f, original.FontSize + 4f, 3f, 1f, FontStyles.Bold),
        ["warning"] = original => From("#FFE66D", "#7A1628", 0.42f, original.FontSize + 5f, 2f, 1f, FontStyles.Bold),
        ["terminal"] = original => From("#70FF9D", "#062713", 0.36f, original.FontSize + 2f, 5f, 1f, FontStyles.Bold),
        ["neon"] = original => From("#FF71D9", "#311060", 0.4f, original.FontSize + 5f, 4f, 1f, FontStyles.Bold),
        ["gold"] = original => From("#FFE29A", "#633B0C", 0.38f, original.FontSize + 3f, 2f, 1f, FontStyles.Bold),
        ["contrast-white"] = original => From("#FFFFFF", "#080A0D", 0.42f, original.FontSize + 5f, 2f, 1f, FontStyles.Bold),
        ["contrast-black"] = original => From("#080A0D", "#FFFFFF", 0.42f, original.FontSize + 5f, 2f, 1f, FontStyles.Bold),
        ["contrast-red"] = original => From("#FF4B4B", "#3B0008", 0.42f, original.FontSize + 5f, 2f, 1f, FontStyles.Bold),
        ["contrast-cyan"] = original => From("#35F2FF", "#002A3A", 0.42f, original.FontSize + 5f, 2f, 1f, FontStyles.Bold),
        ["contrast-lime"] = original => From("#B7FF3C", "#163300", 0.42f, original.FontSize + 5f, 2f, 1f, FontStyles.Bold),
        ["contrast-pink"] = original => From("#FF8ADA", "#4A0730", 0.42f, original.FontSize + 5f, 2f, 1f, FontStyles.Bold),
    };

    public static void Initialize(ConfigFile config)
    {
        Config = config;
        Preset = config.Bind(Section, "Preset", "default", "Timer style preset. Use timerstyle preset <name> to change it.");
        FaceColor = config.Bind(Section, "FaceColor", "", "Custom timer face color as a color name or #RRGGBB. Empty means preset/default.");
        OutlineColor = config.Bind(Section, "OutlineColor", "", "Custom timer outline color as a color name or #RRGGBB. Empty means preset/default.");
        OutlineWidth = config.Bind(Section, "OutlineWidth", -1f, "Custom timer outline width. Negative means preset/default.");
        FontSize = config.Bind(Section, "FontSize", -1f, "Custom timer font size. Negative means preset/default.");
        CharacterSpacing = config.Bind(Section, "CharacterSpacing", UnsetFloat, "Custom timer character spacing. Values below -9000 mean preset/default.");
        Alpha = config.Bind(Section, "Alpha", -1f, "Custom timer alpha from 0 to 1. Negative means preset/default.");
        FontStyle = config.Bind(Section, "FontStyle", "", "Custom timer font style. Empty means preset/default.");
    }

    public static string[] PresetNames()
    {
        return [.. Presets.Keys];
    }

    public static bool HasPreset(string preset)
    {
        return !string.IsNullOrWhiteSpace(preset) && Presets.ContainsKey(preset);
    }

    public static bool TryParseFontStyle(string value, out FontStyles style)
    {
        style = FontStyles.Normal;
        switch (value?.ToLowerInvariant())
        {
            case "normal":
                style = FontStyles.Normal;
                return true;
            case "bold":
                style = FontStyles.Bold;
                return true;
            case "italic":
                style = FontStyles.Italic;
                return true;
            case "bolditalic":
                style = FontStyles.Bold | FontStyles.Italic;
                return true;
            case "underline":
                style = FontStyles.Underline;
                return true;
            case "strikethrough":
                style = FontStyles.Strikethrough;
                return true;
            default:
                return false;
        }
    }

    public static void SetPreset(string preset)
    {
        Preset.Value = preset.ToLowerInvariant();
        ClearCustomEntries();
        SaveAndApply();
    }

    public static void Reset()
    {
        SetPreset("default");
    }

    public static void SetFaceColor(Color color)
    {
        MutateCustom(style => style.FaceColor = color);
    }

    public static void SetOutline(Color color, float? width)
    {
        MutateCustom(style =>
        {
            style.OutlineColor = color;
            style.OutlineWidth = width ?? (style.OutlineWidth > 0f ? style.OutlineWidth : 0.35f);
        });
    }

    public static void SetFontSize(float size)
    {
        MutateCustom(style => style.FontSize = size);
    }

    public static void SetCharacterSpacing(float spacing)
    {
        MutateCustom(style => style.CharacterSpacing = spacing);
    }

    public static void SetAlpha(float alpha)
    {
        MutateCustom(style => style.Alpha = alpha);
    }

    public static void SetFontStyle(FontStyles style)
    {
        MutateCustom(timerStyle => timerStyle.FontStyle = style);
    }

    public static void Apply()
    {
        TMP_Text timer = CL_UIManager.instance?.timer;
        if (timer == null) return;

        EnsureVanillaStyle(timer);
        TimerStyle style = ResolveStyle();
        timer.color = WithAlpha(style.FaceColor, style.Alpha);
        timer.outlineColor = style.OutlineColor;
        timer.outlineWidth = style.OutlineWidth;
        timer.fontSize = style.FontSize;
        timer.characterSpacing = style.CharacterSpacing;
        timer.fontStyle = style.FontStyle;
        timer.UpdateMeshPadding();
        timer.ForceMeshUpdate();
    }

    public static string Describe()
    {
        TimerStyle style = ResolveStyle();
        return $"preset={Preset.Value}, color={Colors.Str(style.FaceColor)}, outline={Colors.Str(style.OutlineColor)} {style.OutlineWidth:F2}, size={style.FontSize:F1}, spacing={style.CharacterSpacing:F1}, alpha={style.Alpha:F2}, style={style.FontStyle}";
    }

    private static void MutateCustom(Action<TimerStyle> mutate)
    {
        TimerStyle style = ResolveStyle();
        mutate(style);
        Preset.Value = "custom";
        FaceColor.Value = Colors.Str(style.FaceColor);
        OutlineColor.Value = Colors.Str(style.OutlineColor);
        OutlineWidth.Value = style.OutlineWidth;
        FontSize.Value = style.FontSize;
        CharacterSpacing.Value = style.CharacterSpacing;
        Alpha.Value = style.Alpha;
        FontStyle.Value = StyleToString(style.FontStyle);
        SaveAndApply();
    }

    private static TimerStyle ResolveStyle()
    {
        TimerStyle original = VanillaStyle ?? new TimerStyle
        {
            FaceColor = Color.white,
            OutlineColor = Color.black,
            OutlineWidth = 0f,
            FontSize = 36f,
            CharacterSpacing = 0f,
            Alpha = 1f,
            FontStyle = FontStyles.Normal,
        };

        TimerStyle style = Presets.TryGetValue(Preset?.Value ?? "default", out var getPreset)
            ? getPreset(original)
            : original.Copy();

        if (!string.IsNullOrWhiteSpace(FaceColor?.Value) && ColorUtility.TryParseHtmlString(FaceColor.Value, out Color face))
        {
            style.FaceColor = face;
        }
        if (!string.IsNullOrWhiteSpace(OutlineColor?.Value) && ColorUtility.TryParseHtmlString(OutlineColor.Value, out Color outline))
        {
            style.OutlineColor = outline;
        }
        if (OutlineWidth != null && OutlineWidth.Value >= 0f) style.OutlineWidth = OutlineWidth.Value;
        if (FontSize != null && FontSize.Value > 0f) style.FontSize = FontSize.Value;
        if (CharacterSpacing != null && CharacterSpacing.Value > -9000f) style.CharacterSpacing = CharacterSpacing.Value;
        if (Alpha != null && Alpha.Value >= 0f) style.Alpha = Mathf.Clamp01(Alpha.Value);
        if (!string.IsNullOrWhiteSpace(FontStyle?.Value) && TryParseFontStyle(FontStyle.Value, out FontStyles parsedStyle))
        {
            style.FontStyle = parsedStyle;
        }

        return style;
    }

    private static void EnsureVanillaStyle(TMP_Text timer)
    {
        if (VanillaStyle != null && VanillaTimer == timer) return;

        VanillaTimer = timer;
        VanillaStyle = new TimerStyle
        {
            FaceColor = timer.color,
            OutlineColor = timer.outlineColor,
            OutlineWidth = timer.outlineWidth,
            FontSize = timer.fontSize,
            CharacterSpacing = timer.characterSpacing,
            Alpha = timer.color.a,
            FontStyle = timer.fontStyle,
        };
    }

    private static void ClearCustomEntries()
    {
        FaceColor.Value = "";
        OutlineColor.Value = "";
        OutlineWidth.Value = -1f;
        FontSize.Value = -1f;
        CharacterSpacing.Value = UnsetFloat;
        Alpha.Value = -1f;
        FontStyle.Value = "";
    }

    private static void SaveAndApply()
    {
        Config?.Save();
        Apply();
    }

    private static TimerStyle From(string face, string outline, float outlineWidth, float fontSize, float spacing, float alpha, FontStyles fontStyle)
    {
        ColorUtility.TryParseHtmlString(face, out Color faceColor);
        ColorUtility.TryParseHtmlString(outline, out Color outlineColor);
        return new TimerStyle
        {
            FaceColor = faceColor,
            OutlineColor = outlineColor,
            OutlineWidth = outlineWidth,
            FontSize = fontSize,
            CharacterSpacing = spacing,
            Alpha = alpha,
            FontStyle = fontStyle,
        };
    }

    private static Color WithAlpha(Color color, float alpha)
    {
        color.a = alpha;
        return color;
    }

    private static string StyleToString(FontStyles style)
    {
        if (style == (FontStyles.Bold | FontStyles.Italic)) return "bolditalic";
        if ((style & FontStyles.Bold) != 0) return "bold";
        if ((style & FontStyles.Italic) != 0) return "italic";
        if ((style & FontStyles.Underline) != 0) return "underline";
        if ((style & FontStyles.Strikethrough) != 0) return "strikethrough";
        return "normal";
    }
}
