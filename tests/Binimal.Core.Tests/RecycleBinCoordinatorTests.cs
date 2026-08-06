using Binimal;
using Xunit;

namespace Binimal.Core.Tests;

public sealed class RecycleBinCoordinatorTests
{
    [Fact]
    public void RefreshPublishesCurrentSnapshotToTray()
    {
        var expected = new RecycleBinSnapshot(4, 4096);
        var service = new FakeRecycleBinService(expected);
        var view = new RecordingTrayView();
        var coordinator = new RecycleBinCoordinator(service, view);

        coordinator.Refresh();

        Assert.Same(expected, view.LastSnapshot);
    }

    [Fact]
    public void EmptyRefreshesTrayAfterServiceCompletes()
    {
        var service = new FakeRecycleBinService(new RecycleBinSnapshot(3, 300));
        var view = new RecordingTrayView();
        var coordinator = new RecycleBinCoordinator(service, view);

        coordinator.Empty();

        Assert.True(service.EmptyCalled);
        Assert.NotNull(view.LastSnapshot);
        Assert.True(view.LastSnapshot!.IsEmpty);
    }

    private sealed class FakeRecycleBinService : IRecycleBinService
    {
        private RecycleBinSnapshot _snapshot;

        public FakeRecycleBinService(RecycleBinSnapshot snapshot) => _snapshot = snapshot;

        public bool EmptyCalled { get; private set; }

        public RecycleBinSnapshot Query() => _snapshot;

        public void Empty()
        {
            EmptyCalled = true;
            _snapshot = new RecycleBinSnapshot(0, 0);
        }
    }

    private sealed class RecordingTrayView : ITrayView
    {
        public RecycleBinSnapshot? LastSnapshot { get; private set; }

        public void Show(RecycleBinSnapshot snapshot) => LastSnapshot = snapshot;
    }
}
