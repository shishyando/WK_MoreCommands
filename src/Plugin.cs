using System;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using MoreCommands.Common;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MoreCommands;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    public static ManualLogSource Beep;
    public static readonly KeyboardShortcut DefaultToggleConsoleShortcut = new KeyboardShortcut(KeyCode.F10);
    public static ConfigEntry<KeyboardShortcut> ToggleConsoleShortcut { get; private set; }
    public static KeyboardShortcut ActiveToggleConsoleShortcut
    {
        get
        {
            KeyboardShortcut shortcut = ToggleConsoleShortcut?.Value ?? DefaultToggleConsoleShortcut;
            return shortcut.MainKey == KeyCode.None ? DefaultToggleConsoleShortcut : shortcut;
        }
    }
    private readonly Harmony Harmony = new Harmony(MyPluginInfo.PLUGIN_GUID);

    private void Awake()
    {
        Beep = Logger;
        ToggleConsoleShortcut = Config.Bind(
            "Input",
            "ToggleConsoleShortcut",
            DefaultToggleConsoleShortcut,
            "Additional shortcut used to toggle the command console. The game's vanilla LeftShift + BackQuote shortcut remains available. Missing or None values fall back to F10.");
        TimerStyleController.Initialize(Config);
        CommandRegistry.InitializeCommands();
        Harmony.PatchAll();
        Beep.LogInfo($"{MyPluginInfo.PLUGIN_GUID} is loaded");

        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    public static void OnSceneUnloaded(Scene s) {
        if (s.name == "Game-Main")
        {
            CommandRegistry.OnExit();
        }
    }

    public static void Assert(bool condition)
    {
        if (!condition)
        {
            Beep.LogFatal($"Assert failed");
            throw new Exception($"[MoreCommands] Assert failed");
        }
    }
}
