using System;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using MoreCommands.Common;
using UnityEngine.SceneManagement;

namespace MoreCommands;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    public static ManualLogSource Beep;
    private readonly Harmony Harmony = new Harmony(MyPluginInfo.PLUGIN_GUID);

    private void Awake()
    {
        Beep = Logger;
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
