namespace HTTPServer.GameStoreApplication.Services
{
    using HTTPServer.GameStoreApplication.Data;
    using HTTPServer.GameStoreApplication.Models;
    using HTTPServer.GameStoreApplication.Services.Contracts;
    using HTTPServer.GameStoreApplication.ViewModels;
    using System.Collections.Generic;
    using System.Linq;

    public class GameDataService : DataService, IGameDataService
    {
        public GameDataService(GameStoreContext gameStoreContext)
            : base(gameStoreContext)
        {
        }

        public void AddGame(Game game)
        {
            this.Context.Games.Add(game);

            this.Context.SaveChanges();
        }

        public void AddUserGame(UserGame userGame, int gameId)
        {
            var game = FindGame(gameId);

            if (game != null)
            {
                game.Users.Add(userGame);

                this.Context.SaveChanges();
            }
        }

        public void DeleteGame(int id)
        {
            var game = FindGame(id);

            this.Context.Games.Remove(game);

            this.Context.SaveChanges();
        }

        public void EditGame(GameViewModel viewModel, int gameId)
        {
            var game = FindGame(gameId);

            if (game == null)
            {
                return;
            }

            game.Title = viewModel.Title;
            game.Description = viewModel.Description;
            game.Price = viewModel.Price;
            game.ReleaseDate = viewModel.ReleaseDate;
            game.Size = viewModel.Size;
            game.ThumbnailURL = viewModel.ThumbnailURL;
            game.TrailerId = viewModel.TrailerId;

            this.Context.SaveChanges();
        }

        public Game FindGame(int id) =>
            this.Context.Games.FirstOrDefault(g => g.Id == id);

        public bool GameExistById(int id) =>
            FindGame(id) != null;

        public ICollection<Game> GetOwnedGames(string filter, int userId)
        {
            return this.Context
                .UserGames
                .Where(ug => ug.UserId == userId)
                .Select(ug => ug.Game)
                .ToList();
        }
    }
}
