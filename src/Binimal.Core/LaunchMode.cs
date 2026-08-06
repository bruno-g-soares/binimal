namespace Binimal;

using System;

public static class LaunchMode
{
    public static bool IsSelfTest(string[] arguments)
    {
        if (arguments is null)
        {
            throw new ArgumentNullException(nameof(arguments));
        }

        return arguments.Length == 1 &&
            string.Equals(arguments[0], "--self-test", StringComparison.Ordinal);
    }
}
