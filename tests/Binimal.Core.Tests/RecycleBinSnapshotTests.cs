using Binimal;
using Xunit;

namespace Binimal.Core.Tests;

public sealed class RecycleBinSnapshotTests
{
    [Fact]
    public void SnapshotWithNoItemsIsEmpty()
    {
        var snapshot = new RecycleBinSnapshot(0, 0);

        Assert.True(snapshot.IsEmpty);
    }

    [Fact]
    public void EmptySnapshotHasClearStatusText()
    {
        var snapshot = new RecycleBinSnapshot(0, 0);

        Assert.Equal("Binimal — Recycle Bin is empty", snapshot.StatusText);
    }

    [Fact]
    public void CombiningDriveSnapshotsSumsItemsAndBytes()
    {
        var combined = RecycleBinSnapshot.Combine(new[]
        {
            new RecycleBinSnapshot(2, 1024),
            new RecycleBinSnapshot(3, 2048),
        });

        Assert.Equal(5, combined.ItemCount);
        Assert.Equal(3072, combined.SizeInBytes);
    }

    [Fact]
    public void SingleItemUsesSingularStatusText()
    {
        var snapshot = new RecycleBinSnapshot(1, 42);

        Assert.Equal("Binimal — 1 item", snapshot.StatusText);
    }
}
