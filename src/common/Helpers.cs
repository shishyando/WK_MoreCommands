using System;

public static class Helpers
{
    public static bool Substr(string text, string check)
    {
        return check == "" || text?.IndexOf(check, StringComparison.OrdinalIgnoreCase) >= 0;
    }
}
