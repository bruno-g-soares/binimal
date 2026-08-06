namespace Binimal;

using System;

public sealed class RecycleBinCoordinator
{
    private readonly IRecycleBinService _service;
    private readonly ITrayView _view;

    public RecycleBinCoordinator(IRecycleBinService service, ITrayView view)
    {
        _service = service ?? throw new ArgumentNullException(nameof(service));
        _view = view ?? throw new ArgumentNullException(nameof(view));
    }

    public void Refresh() => _view.Show(_service.Query());

    public void Empty()
    {
        _service.Empty();
        Refresh();
    }
}
