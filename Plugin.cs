using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace MoreCommands;

[BepInPlugin(GUID, Name, Version)]
public class MoreCommandsPlugin : BaseUnityPlugin
{
    private const string GUID = "shishyando.WK.MoreCommands";
    private const string Name = "MoreCommands";
    private const string Version = "0.1.0";

    public static new ManualLogSource Logger;
    private readonly Harmony Harmony = new Harmony(GUID);

    private void Awake()
    {
        Logger = base.Logger;
        Harmony.PatchAll(typeof(Patches.ENT_Player_Patcher));
        Logger.LogInfo($"{GUID} is loaded");
    }
}
