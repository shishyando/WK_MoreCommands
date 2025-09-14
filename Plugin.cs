using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using MoreCommands.Common;
using UnityEngine.SceneManagement;

namespace MoreCommands;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class MoreCommandsPlugin : BaseUnityPlugin
{
    public static new ManualLogSource Logger;
    private readonly Harmony Harmony = new Harmony(MyPluginInfo.PLUGIN_GUID);

    private void Awake()
    {
        Logger = base.Logger;
        CommandRegistry.InitializeCommands();
        Harmony.PatchAll();
        Logger.LogInfo($"{MyPluginInfo.PLUGIN_GUID} is loaded");

        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    public static void OnSceneUnloaded(Scene s) {
        if (s.name == "Game-Main")
        {
            CommandRegistry.DisableAllTogglableCommands();
        }
    }
}
