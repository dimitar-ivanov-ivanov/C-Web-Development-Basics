namespace HTTPServer.GameStoreApplication.Services
{
    using HTTPServer.GameStoreApplication.Data;
    using HTTPServer.GameStoreApplication.Services.Contracts;

    public abstract class DataService : IDataService
    {
        protected DataService(GameStoreContext gameStoreContext)
        {
            this.Context = gameStoreContext;
        }

        public GameStoreContext Context { get; private set; }
    }
}
