namespace Binimal;

using System;

public static class StartupCommand
{
    public static bool Matches(string? registeredCommand, string executablePath)
    {
        if (executablePath == null)
        {
            throw new ArgumentNullException(nameof(executablePath));
        }

        var expectedCommand = $"\"{executablePath}\"";
        return string.Equals(registeredCommand, expectedCommand, StringComparison.OrdinalIgnoreCase);
    }
}
