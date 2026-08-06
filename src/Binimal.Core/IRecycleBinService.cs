namespace Binimal;

public interface IRecycleBinService
{
    RecycleBinSnapshot Query();

    void Empty();
}
