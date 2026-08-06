namespace Binimal;

using System;
using System.Threading;
using System.Windows.Forms;

internal static class Program
{
    [STAThread]
    private static int Main(string[] arguments)
    {
        if (LaunchMode.IsSelfTest(arguments))
        {
            try
            {
                _ = new NativeRecycleBinService().Query();
                return 0;
            }
            catch
            {
                return 1;
            }
        }

        using var mutex = new Mutex(true, @"Local\Binimal.SingleInstance", out var isFirstInstance);
        if (!isFirstInstance)
        {
            return 0;
        }

        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new TrayApplicationContext());
        return 0;
    }
}
