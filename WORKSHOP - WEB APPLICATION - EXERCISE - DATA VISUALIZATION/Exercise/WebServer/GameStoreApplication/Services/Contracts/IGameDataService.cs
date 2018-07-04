namespace HTTPServer.GameStoreApplication.Services.Contracts
{
    using HTTPServer.GameStoreApplication.Models;
    using HTTPServer.GameStoreApplication.ViewModels;
    using System.Collections.Generic;

    public interface IGameDataService : IDataService
    {
        void AddGame(Game game);

        void DeleteGame(int id);

        ICollection<Game> GetOwnedGames(string filter, int userId);

        void AddUserGame(UserGame userGame, int gameId);

        void EditGame(GameViewModel viewModel, int gameId);

        Game FindGame(int id);

        bool GameExistById(int id);
    }
}