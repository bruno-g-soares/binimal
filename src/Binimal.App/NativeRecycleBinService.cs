namespace Binimal;

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

internal sealed class NativeRecycleBinService : IRecycleBinService
{
    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    private struct RecycleBinInfo
    {
        public uint Size;
        public long TotalSize;
        public long ItemCount;
    }

    [DllImport("shell32.dll", CharSet = CharSet.Unicode)]
    private static extern int SHQueryRecycleBin(string rootPath, ref RecycleBinInfo info);

    [DllImport("shell32.dll", CharSet = CharSet.Unicode)]
    private static extern int SHEmptyRecycleBin(IntPtr windowHandle, string? rootPath, uint flags);

    public RecycleBinSnapshot Query()
    {
        var snapshots = new List<RecycleBinSnapshot>();
        var successfulQueryCount = 0;

        foreach (var drive in DriveInfo.GetDrives())
        {
            try
            {
                if (!drive.IsReady || drive.DriveType == DriveType.CDRom || drive.DriveType == DriveType.Network)
                {
                    continue;
                }

                var info = new RecycleBinInfo
                {
                    Size = (uint)Marshal.SizeOf(typeof(RecycleBinInfo)),
                };

                if (SHQueryRecycleBin(drive.RootDirectory.FullName, ref info) >= 0)
                {
                    successfulQueryCount++;
                    snapshots.Add(new RecycleBinSnapshot(info.ItemCount, info.TotalSize));
                }
            }
            catch (IOException)
            {
                // A removable drive can disappear while Windows is enumerating it.
            }
            catch (UnauthorizedAccessException)
            {
                // Ignore drives whose Recycle Bin is unavailable to this user.
            }
        }

        if (successfulQueryCount == 0)
        {
            throw new InvalidOperationException("No eligible Recycle Bin drive query succeeded.");
        }

        return RecycleBinSnapshot.Combine(snapshots);
    }

    public void Empty()
    {
        var result = SHEmptyRecycleBin(IntPtr.Zero, null, 0);
        if (result < 0)
        {
            Marshal.ThrowExceptionForHR(result);
        }
    }
}
