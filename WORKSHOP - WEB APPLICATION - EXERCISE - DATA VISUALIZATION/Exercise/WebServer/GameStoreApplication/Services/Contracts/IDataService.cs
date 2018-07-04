namespace HTTPServer.GameStoreApplication.Services.Contracts
{
    using HTTPServer.GameStoreApplication.Data;

    public interface IDataService
    {
        GameStoreContext Context { get; }
    }
}
