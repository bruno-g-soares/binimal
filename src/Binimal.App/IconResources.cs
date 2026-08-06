namespace Binimal;

using System;
using System.Drawing;
using System.IO;
using System.Reflection;

internal static class IconResources
{
    public static Icon Load(string resourceName)
    {
        var assembly = Assembly.GetExecutingAssembly();
        using var stream = assembly.GetManifestResourceStream(resourceName)
            ?? throw new InvalidOperationException($"Missing embedded icon: {resourceName}");
        using var icon = new Icon(stream);
        return (Icon)icon.Clone();
    }
}
