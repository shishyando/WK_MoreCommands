
namespace MoreCommands.Common;

public static class PerkChanger
{
    public static void MaxOutPerk(string perkId)
    {
        ENT_Player player = ENT_Player.playerObject;
        if (player == null) return;
        Perk old = player.GetPerk(perkId);
        if (old != null)
        {
            int toAdd = old.stackMax - old.stackAmount;
            if (toAdd > 0) old.AddStack(toAdd);
            return;
        }

        Perk template = CL_AssetManager.GetPerkAsset(perkId, "");
        if (template != null)
        {
            Perk perk = UnityEngine.Object.Instantiate(template);
            int toAdd = perk.stackMax;
            if (toAdd > 0) player.AddPerk(perk, toAdd);
            return;
        }

        Plugin.Beep.LogWarning($"Perk {perkId} not found!");
    }
}
