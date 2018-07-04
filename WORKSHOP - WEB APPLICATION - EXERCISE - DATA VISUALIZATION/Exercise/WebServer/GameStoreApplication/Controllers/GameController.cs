namespace HTTPServer.GameStoreApplication.Controllers
{
    using HTTPServer.GameStoreApplication.Common;
    using HTTPServer.GameStoreApplication.Constants;
    using HTTPServer.GameStoreApplication.Models;
    using HTTPServer.GameStoreApplication.Services.Contracts;
    using HTTPServer.GameStoreApplication.ViewModels;
    using HTTPServer.Server.Http.Contracts;
    using System.Text;

    public class GameController : BaseController
    {
        private readonly Authenticator authenticator;

        public GameController(IHttpRequest request, IUserDataService userDataService, IGameDataService gameDataService, HeaderPathFinder pathFinder, Authenticator authenticator)
           : base(request, userDataService, gameDataService, pathFinder)
        {
            this.authenticator = authenticator;
        }

        public IHttpResponse AddGameGet()
        {
            //Accessible by admin only
            if (!this.authenticator.UserIsAdmin())
            {
                return this.RedirectResponse(Paths.HomePath);
            }

            this.ViewData["error"] = string.Empty;
            this.ViewData["errorDisplay"] = "none";

            return this.FileViewResponse(Paths.AddGameView, PathFinder.FindHeaderPath(this.Request));
        }

        public IHttpResponse AddGamePost()
        {
            //Accessible by admin only
            if (!this.authenticator.UserIsAdmin())
            {
                return this.RedirectResponse(Paths.HomePath);
            }

            //Get viewmodel
            var viewModel = GetGameViewModel();

            if (this.Request == null)
            {
                return this.RedirectResponse(Paths.RegisterPath);
            }

            //Validate viewmodel
            var isValid = this.ValidateGameViewModel(viewModel);

            if (!isValid.Valid)
            {
                this.ViewData["error"] = isValid.Message;
                this.ViewData["errorDisplay"] = "flex";

                return this.FileViewResponse(Paths.AddGameView, this.PathFinder.FindHeaderPath(this.Request));
            }

            AddGameToDatabase(viewModel);

            //Redirect to page with listed games
            return this.RedirectResponse(Paths.ListGamesPath);
        }

        public IHttpResponse ListGames()
        {
            //Accessible by admin only
            if (!this.authenticator.UserIsAdmin())
            {
                return this.RedirectResponse(Paths.HomePath);
            }

            var allGames = new StringBuilder();

            var games = this.GameDataService.Context.Games;

            var count = 1;

            //Present the game in the given html format
            foreach (var game in games)
            {
                allGames.AppendLine(@"<tr class=""table - warning"">" +
                                             $@"<th scope = ""row"">{count}</th>" +
                                             $@"<td>{game.Title}</td>" +
                                             $@"<td>{game.Size} GB</td>" +
                                             $@"<td>{game.Price:f0} &euro;</td>" +
                                             @"<td>" +
                                             $@"    <a href = ""{string.Format(Paths.EditGamePath, game.Id)}"" class=""btn btn-warning btn-sm"">Edit</a>" +
                                             $@"    <a href = ""{string.Format(Paths.DeleteGamePath, game.Id)}"" class=""btn btn-danger btn-sm"">Delete</a>" +
                                             @"</td>" +
                                    @"</tr>");

                count++;
            }

            this.ViewData["allGames"] = allGames.ToString();

            //Redirect to page with listed games
            return this.FileViewResponse(Paths.ListGamesView, this.PathFinder.FindHeaderPath(this.Request));
        }

        public IHttpResponse EditGet()
        {
            //Accessible by admin only
            if (!this.authenticator.UserIsAdmin())
            {
                return this.RedirectResponse(Paths.HomePath);
            }

            var id = int.Parse(this.Request.UrlParameters["id"]);

            var game = this.GameDataService.FindGame(id);

            //If game doesn't exist redirect to home
            if (game == null)
            {
                return this.RedirectResponse(Paths.HomePath);
            }

            //Get the needed information for the game
            this.ViewData["title"] = game.Title;
            this.ViewData["description"] = game.Description;
            this.ViewData["thumbnail"] = game.ThumbnailURL;
            this.ViewData["price"] = game.Price.ToString();
            this.ViewData["size"] = game.Size.ToString();
            this.ViewData["video-id"] = game.TrailerId;
            this.ViewData["release-date"] = game.ReleaseDate.ToString("yyyy-MM-dd");
            this.ViewData["error"] = string.Empty;
            this.ViewData["errorDisplay"] = "none";

            return this.FileViewResponse(Paths.EditGameView, PathFinder.FindHeaderPath(this.Request));
        }

        public IHttpResponse EditPost()
        {
            //Accessible by admin only
            if (!this.authenticator.UserIsAdmin())
            {
                return this.RedirectResponse(Paths.HomePath);
            }

            var viewModel = GetGameViewModel();

            var id = int.Parse(this.Request.UrlParameters["id"]);

            //Validate game model
            var isValid = this.ValidateGameViewModel(viewModel);

            if (!isValid.Valid)
            {
                this.ViewData["error"] = isValid.Message;
                this.ViewData["errorDisplay"] = "flex";

                return this.FileViewResponse(Paths.EditGameView, this.PathFinder.FindHeaderPath(this.Request));
            }

            //Edit game
            this.GameDataService.EditGame(viewModel, id);

            //Redirect to games list
            return this.RedirectResponse(Paths.ListGamesPath);
        }

        public IHttpResponse DeleteGet()
        {
            //Accessible by admin only
            if (!this.authenticator.UserIsAdmin())
            {
                return this.RedirectResponse(Paths.HomePath);
            }

            var id = int.Parse(this.Request.UrlParameters["id"]);

            var game = this.GameDataService.FindGame(id);

            //If game doesn't exist go back to home
            if (game == null)
            {
                return this.RedirectResponse(Paths.HomePath);
            }

            //Get the needed information for the game
            this.ViewData["title"] = game.Title;
            this.ViewData["description"] = game.Description;
            this.ViewData["thumbnail"] = game.ThumbnailURL;
            this.ViewData["price"] = game.Price.ToString();
            this.ViewData["size"] = game.Size.ToString();
            this.ViewData["video-id"] = game.TrailerId;
            this.ViewData["release-date"] = game.ReleaseDate.ToString("yyyy-MM-dd");

            return this.FileViewResponse(Paths.DeleteGameView, PathFinder.FindHeaderPath(this.Request));
        }

        public IHttpResponse DeletePost()
        {
            //Accessible by admin only
            if (!this.authenticator.UserIsAdmin())
            {
                return this.RedirectResponse(Paths.HomePath);
            }

            var id = int.Parse(this.Request.UrlParameters["id"]);

            //Delete game
            this.GameDataService.DeleteGame(id);

            //Redirect to list games
            return this.RedirectResponse(Paths.ListGamesPath);
        }

        public IHttpResponse DetailsGet()
        {
            var id = int.Parse(this.Request.UrlParameters["id"]);

            var game = this.GameDataService.FindGame(id);

            //Get the needed info for the game
            this.ViewData["video-id"] = game.TrailerId;
            this.ViewData["description"] = game.Description;
            this.ViewData["price"] = game.Price.ToString("F0");
            this.ViewData["size"] = game.Size.ToString();
            this.ViewData["release-date"] = game.ReleaseDate.ToString("MM/dd/yyyy");
            this.ViewData["adminRights"] = string.Empty;
            this.ViewData["buyPath"] = string.Format(string.Format(Paths.BuyGamePath, id));

            //If user is admin give him special options
            if (this.authenticator.UserIsAdmin())
            {
                this.ViewData["adminRights"] =
                    $@"<a class=""btn btn-warning"" href=""{string.Format(Paths.EditGamePath, game.Id)}"">Edit</a> " +
                    $@"<a class=""btn btn-danger"" href=""{string.Format(Paths.DeleteGamePath, game.Id)}"">Delete</a>";
            }

            return this.FileViewResponse(Paths.DetailsGameView, PathFinder.FindHeaderPath(this.Request));
        }

        private void AddGameToDatabase(GameViewModel viewModel)
        {
            var game = new Game()
            {
                Description = viewModel.Description,
                Price = viewModel.Price,
                ReleaseDate = viewModel.ReleaseDate,
                Size = viewModel.Size,
                ThumbnailURL = viewModel.ThumbnailURL,
                Title = viewModel.Title,
                TrailerId = viewModel.TrailerId,
            };

            this.GameDataService.AddGame(game);
        }
    }
}