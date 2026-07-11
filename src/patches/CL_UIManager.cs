using HarmonyLib;
using MoreCommands.Common;

namespace MoreCommands.Patches;

[HarmonyPatch(typeof(CL_UIManager), "Start")]
public static class CL_UIManager_Start_Patcher
{
    static void Postfix()
    {
        TimerStyleController.Apply();
    }
}
