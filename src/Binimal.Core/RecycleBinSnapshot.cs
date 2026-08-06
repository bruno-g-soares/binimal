namespace Binimal;

using System;
using System.Collections.Generic;

public sealed class RecycleBinSnapshot
{
    public RecycleBinSnapshot(long itemCount, long sizeInBytes)
    {
        ItemCount = itemCount;
        SizeInBytes = sizeInBytes;
    }

    public long ItemCount { get; }

    public long SizeInBytes { get; }

    public bool IsEmpty => ItemCount == 0;

    public string StatusText => IsEmpty
        ? "Binimal — Recycle Bin is empty"
        : ItemCount == 1
            ? "Binimal — 1 item"
            : $"Binimal — {ItemCount} items";

    public static RecycleBinSnapshot Combine(IEnumerable<RecycleBinSnapshot> snapshots)
    {
        if (snapshots is null)
        {
            throw new ArgumentNullException(nameof(snapshots));
        }

        long itemCount = 0;
        long sizeInBytes = 0;
        foreach (var snapshot in snapshots)
        {
            itemCount += snapshot.ItemCount;
            sizeInBytes += snapshot.SizeInBytes;
        }

        return new RecycleBinSnapshot(itemCount, sizeInBytes);
    }
}
