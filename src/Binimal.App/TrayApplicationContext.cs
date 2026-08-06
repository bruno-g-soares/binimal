namespace Binimal;

using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

internal sealed class TrayApplicationContext : ApplicationContext, ITrayView
{
    private readonly Icon _emptyIcon;
    private readonly Icon _fullIcon;
    private readonly NotifyIcon _notifyIcon;
    private readonly ContextMenuStrip _menu;
    private readonly Timer _timer;
    private readonly RecycleBinCoordinator _coordinator;
    private readonly ToolStripMenuItem _startupItem;
    private bool _disposed;

    public TrayApplicationContext()
    {
        _emptyIcon = IconResources.Load("Binimal.Assets.bin-empty.ico");
        _fullIcon = IconResources.Load("Binimal.Assets.bin-full.ico");

        _menu = new ContextMenuStrip();
        var openItem = new ToolStripMenuItem("Open Recycle Bin")
        {
            Font = new Font(_menu.Font, FontStyle.Bold),
        };
        openItem.Click += (_, _) => OpenRecycleBin();

        var emptyItem = new ToolStripMenuItem("Empty Recycle Bin");
        emptyItem.Click += (_, _) => EmptyRecycleBin();

        _startupItem = new ToolStripMenuItem("Start with Windows")
        {
            Checked = StartupManager.IsEnabled(),
            CheckOnClick = true,
        };
        _startupItem.Click += (_, _) => UpdateStartupPreference();

        var exitItem = new ToolStripMenuItem("Exit");
        exitItem.Click += (_, _) => ExitThread();

        _menu.Items.Add(openItem);
        _menu.Items.Add(emptyItem);
        _menu.Items.Add(new ToolStripSeparator());
        _menu.Items.Add(_startupItem);
        _menu.Items.Add(new ToolStripSeparator());
        _menu.Items.Add(exitItem);

        _notifyIcon = new NotifyIcon
        {
            ContextMenuStrip = _menu,
            Icon = _emptyIcon,
            Text = "Binimal",
            Visible = true,
        };
        _notifyIcon.MouseClick += (_, eventArgs) =>
        {
            if (eventArgs.Button == MouseButtons.Left)
            {
                OpenRecycleBin();
            }
        };

        _coordinator = new RecycleBinCoordinator(new NativeRecycleBinService(), this);
        _timer = new Timer { Interval = 2000 };
        _timer.Tick += (_, _) => RefreshSafely();
        _timer.Start();

        RefreshSafely();
    }

    public void Show(RecycleBinSnapshot snapshot)
    {
        _notifyIcon.Icon = snapshot.IsEmpty ? _emptyIcon : _fullIcon;
        _notifyIcon.Text = TruncateTooltip(snapshot.StatusText);
    }

    protected override void ExitThreadCore()
    {
        DisposeResources();
        base.ExitThreadCore();
    }

    private static string TruncateTooltip(string text) =>
        text.Length <= 63 ? text : text.Substring(0, 63);

    private static void OpenRecycleBin()
    {
        try
        {
            Process.Start("explorer.exe", "shell:RecycleBinFolder");
        }
        catch (Exception exception)
        {
            ShowError("Binimal could not open the Recycle Bin.", exception);
        }
    }

    private void EmptyRecycleBin()
    {
        try
        {
            _coordinator.Empty();
        }
        catch (Exception exception)
        {
            ShowError("Binimal could not empty the Recycle Bin.", exception);
        }
    }

    private void RefreshSafely()
    {
        try
        {
            _coordinator.Refresh();
        }
        catch
        {
            _notifyIcon.Text = "Binimal — Recycle Bin status unavailable";
        }
    }

    private void UpdateStartupPreference()
    {
        try
        {
            StartupManager.SetEnabled(_startupItem.Checked);
            _startupItem.Checked = StartupManager.IsEnabled();
        }
        catch (Exception exception)
        {
            _startupItem.Checked = false;
            ShowError("Binimal could not update the startup setting.", exception);
        }
    }

    private static void ShowError(string message, Exception exception)
    {
        MessageBox.Show(
            $"{message}\n\n{exception.Message}",
            "Binimal",
            MessageBoxButtons.OK,
            MessageBoxIcon.Error);
    }

    private void DisposeResources()
    {
        if (_disposed)
        {
            return;
        }

        _disposed = true;
        _timer.Stop();
        _timer.Dispose();
        _notifyIcon.Visible = false;
        _notifyIcon.Dispose();
        _menu.Dispose();
        _emptyIcon.Dispose();
        _fullIcon.Dispose();
    }
}
