namespace HTTPServer.GameStoreApplication.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using HTTPServer.GameStoreApplication.Common;
    using HTTPServer.GameStoreApplication.Constants;
    using HTTPServer.GameStoreApplication.Models;
    using HTTPServer.GameStoreApplication.Services.Contracts;
    using HTTPServer.Server.Http;
    using HTTPServer.Server.Http.Contracts;

    public class HomeController : BaseController
    {
        private readonly Authenticator authenticator;

        public HomeController(IHttpRequest request, IUserDataService userDataService, IGameDataService gameDataService,
            HeaderPathFinder pathFinder, Authenticator authenticator)
           : base(request, userDataService, gameDataService, pathFinder)
        {
            this.authenticator = authenticator;
        }

        public IHttpResponse HomeGet()
        {
            //Get all games 
            this.ViewData["content"] = ListGames(this.GameDataService.Context.Games.ToList());

            return this.FileViewResponse(Paths.HomeView, this.PathFinder.FindHeaderPath(this.Request));
        }

        public IHttpResponse HomePost()
        {
            var filter = this.Request.FormData["filter"];

            var games = new List<Game>();

            var currentUserId = (int?)this.Request.Session.Get(SessionStore.CurrentUserKey);

            //If user doesn't exist we go back to home
            if (!currentUserId.HasValue)
            {
                return HomeGet();
            }

            //Get all games and filter them
            if (filter == "Owned")
            {
                games = this.GameDataService.GetOwnedGames(filter, currentUserId.Value).ToList();
            }
            else if (filter == "All")
            {
                games = this.GameDataService.Context.Games.ToList();
            }

            this.ViewData["content"] = ListGames(games);

            return this.FileViewResponse(Paths.HomeView, this.PathFinder.FindHeaderPath(this.Request));
        }

        private string ListGames(List<Game> games)
        {
            var gamesResults = new StringBuilder();

            //Counting the games in a row because we can have a maximum of 3 games in one row
            var count = 0;
            gamesResults.AppendLine(@"<div class=""card-group"">");

            foreach (var game in games)
            {
                gamesResults
                    .AppendLine(@"<div class=""card col-4 thumbnail"">")
                    .AppendLine(@"<img class=""card-image-top img-fluid img-thumbnail""");

                //if thumbnnail doesn't exist take the thumbnail of the trailer
                if (game.ThumbnailURL == null)
                {
                    gamesResults.AppendLine($@"onerror=""this.src = 'https://i.ytimg.com/vi/{game.TrailerId}/maxresdefault.jpg';""");
                }

                gamesResults
                    .AppendLine($@"src=""{game.ThumbnailURL}""/>")
                    .AppendLine(@"<div class=""card-body"">")
                    .AppendLine($@"<h4 class=""card-title"">{game.Title}</h4>")
                    .AppendLine($@"<p class=""card-text""><strong>Price</strong> - {game.Price:f0}&euro;</p>")
                    .AppendLine($@"<p class=""card-text""><strong>Size</strong> - {game.Size} GB</p>")
                    .AppendLine($@"<p class=""card-text"">{new string(game.Description.Take(300).ToArray())}</p>")
                    .AppendLine(@"</div>")
                    .AppendLine($@"<div class=""card-footer"">");

                //if user is admin give him special options
                if (authenticator.UserIsAdmin())
                {
                    gamesResults
                        .AppendLine($@"<a class=""card-button btn btn-warning"" name=""edit"" href=""{string.Format(Paths.EditGamePath, game.Id)}"">Edit</a>")
                        .AppendLine($@"<a class=""card-button btn btn-danger"" name=""delete"" href=""{string.Format(Paths.DeleteGamePath, game.Id)}"">Delete</a>");

                }

                gamesResults
                    .AppendLine($@"<a class=""card-button btn btn-outline-primary"" name=""info"" href=""{string.Format(Paths.DetailsGamePath, game.Id)}"">Info</a>")
                    .AppendLine($@"<a class=""card-button btn btn-primary"" name=""buy"" href=""{string.Format(Paths.BuyGamePath, game.Id)}"">Buy</a>")
                    .AppendLine("</div>")
                    .AppendLine("</div>");

                count++;

                if (count % 3 == 0)
                {
                    gamesResults.AppendLine("</div>");
                    gamesResults.AppendLine(@"<div class=""card-group"">");
                }
            }

            var str = gamesResults.ToString();

            return gamesResults.ToString();
        }
    }
}