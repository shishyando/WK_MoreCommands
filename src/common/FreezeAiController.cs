namespace MoreCommands.Common;

public static class FreezeAiController
{
    public static bool Frozen { get; private set; }
    private static bool NoTargetSnapshot;
    private static CL_GameManager NoTargetOwner;

    public static void SetFrozen(bool frozen)
    {
        if (Frozen == frozen) return;

        Frozen = frozen;
        CL_GameManager manager = CL_GameManager.gMan;
        if (Frozen)
        {
            NoTargetOwner = manager;
            if (manager != null)
            {
                NoTargetSnapshot = CL_GameManager.noTarget;
                CL_GameManager.noTarget = true;
            }
            return;
        }

        if (manager != null
            && manager == NoTargetOwner
            && CL_GameManager.noTarget)
        {
            CL_GameManager.noTarget = NoTargetSnapshot;
        }
        NoTargetOwner = null;
    }
}
