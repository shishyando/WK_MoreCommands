public static class ArgParse
{
    public static int GetMult(string[] args, int fallback = 1)
    {
        if (args.Length > 0) int.TryParse(args[0], out fallback);
        return fallback;
    }

    public static bool ParseEnabled(bool enabled, string[] args)
    {
        if (args.Length == 0 || !bool.TryParse(args[0], out bool result))
        {
            return !enabled;
        }
        return result;
    }
}
