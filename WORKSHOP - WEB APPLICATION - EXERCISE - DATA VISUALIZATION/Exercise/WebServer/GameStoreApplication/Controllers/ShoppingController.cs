namespace HTTPServer.GameStoreApplication.Controllers
{
    using HTTPServer.GameStoreApplication.Common;
    using HTTPServer.GameStoreApplication.Constants;
    using HTTPServer.GameStoreApplication.Models;
    using HTTPServer.GameStoreApplication.Services.Contracts;
    using HTTPServer.GameStoreApplication.ViewModels;
    using HTTPServer.Server.Http;
    using HTTPServer.Server.Http.Contracts;
    using System.Linq;
    using System.Text;

    public class ShoppingController : BaseController
    {
        public ShoppingController(IHttpRequest request, IUserDataService userDataService, IGameDataService gameDataService, HeaderPathFinder pathFinder)
            : base(request, userDataService, gameDataService, pathFinder)
        {
        }

        public IHttpResponse BuyGet()
        {
            var cart = (ShoppingCart)this.Request.Session.Get(ShoppingCart.SessionKey);

            var gameId = int.Parse(this.Request.UrlParameters["id"]);

            var currentUserId = (int?)this.Request.Session.Get(SessionStore.CurrentUserKey);

            // A user can't buy one game twice

            if (!cart.GameIds.Contains(gameId) && this.GameDataService.GameExistById(gameId))
            {
                //If user is logged in the user doesn't own the game we can add it to the cart
                if (currentUserId.HasValue)
                {
                    var currentUser = this.UserDataService.FindUser(currentUserId.Value);

                    if (currentUser != null)
                    {
                        var userGameExists = this.UserDataService
                            .Context
                            .UserGames
                            .FirstOrDefault(ug => ug.GameId == gameId && ug.UserId == currentUserId) != null;

                        if (!userGameExists)
                        {
                            cart.GameIds.Add(gameId);
                        }
                    }
                }
                else
                {
                    //If user isn't logged in add the game freely
                    cart.GameIds.Add(gameId);
                }
            }

            return this.RedirectResponse(Paths.CartPath);
        }

        public IHttpResponse RemoveGet()
        {
            var cart = (ShoppingCart)this.Request.Session.Get(ShoppingCart.SessionKey);

            var id = int.Parse(this.Request.UrlParameters["id"]);

            //If game exists remove it from cart
            if (this.GameDataService.GameExistById(id))
            {
                cart.GameIds.Remove(id);
            }

            return this.RedirectResponse(Paths.CartPath);
        }

        public IHttpResponse CartGet()
        {
            var cart = (ShoppingCart)this.Request.Session.Get(ShoppingCart.SessionKey);

            var games = cart.GameIds
                .Select(g => this.GameDataService.FindGame(g))
                .ToList();

            var totalSum = games.Sum(g => g.Price);
            var result = new StringBuilder();

            //Get needed information for games in the cart
            foreach (var game in games)
            {
                result
                    .AppendLine(@"<div class=""list-group-item"">")
                    .AppendLine(@"<div class=""media"">")
                    .AppendLine($@"<a class=""btn btn-outline-danger btn-lg align-self-center mr-3"" href=""{string.Format(Paths.RemoveGamePath, game.Id)}"">X</a>")
                    .AppendLine($@"<img class=""d-flex mr-4 align-self-center img-thumbnail"" height=""127"" src=""{game.ThumbnailURL}"" width=""227"" alt=""Generic placeholder image"">")
                    .AppendLine(@"<div class=""media-body align-self-center"">")
                    .AppendLine($@"<a href=""{string.Format(Paths.DetailsGamePath, game.Id)}""><h4 class=""mb-1 list-group-item-heading"">{game.Title}</h4></a>")
                    .AppendLine($@"<p>{game.Description}</p>")
                    .AppendLine(@"</div>")
                    .AppendLine(@"<div class=""col-md-2 text-center align-self-center mr-auto"">")
                    .AppendLine($@"<h2>{game.Price.ToString("F0")}&euro;</h2>")
                    .AppendLine("</div>")
                    .AppendLine("</div>")
                    .AppendLine("</div>");

            }

            this.ViewData["totlaSum"] = totalSum.ToString("F0");
            this.ViewData["content"] = result.ToString();

            return this.FileViewResponse(Paths.CartView, PathFinder.FindHeaderPath(this.Request));
        }

        public IHttpResponse CartPost()
        {
            //Check if user is logged in 
            var currentUserId = (int?)this.Request.Session.Get(SessionStore.CurrentUserKey);

            //If user isn't logged in and tries to buy games from the cart have him log in 
            if (!currentUserId.HasValue)
            {
                return this.RedirectResponse(Paths.OrderLoginPath);
            }

            //Transfer the games to his account
            OrderGames(currentUserId);
            return CartGet();
        }

        public IHttpResponse OrderLoginGet()
        {
            //Log in after trying to order games from the cart
            this.ViewData["error"] = string.Empty;
            this.ViewData["errorDisplay"] = "none";

            return this.FileViewResponse(Paths.LoginView, this.PathFinder.FindHeaderPath(this.Request));
        }

        public IHttpResponse OrderLoginPost()
        {
            //Get viewmodel
            var viewModel = GetLoginViewModel();

            if (this.Request == null)
            {
                return this.RedirectResponse(Paths.LoginPath);
            }

            //Validate viewmodel
            var isValid = this.ValidateLoginViewModel(viewModel);

            if (!isValid.Valid)
            {
                this.ViewData["error"] = isValid.Message;
                this.ViewData["errorDisplay"] = "flex";

                return this.FileViewResponse(Paths.LoginView, this.PathFinder.FindHeaderPath(this.Request));
            }

            //Add user to session
            var currentUser = this.UserDataService.UserByEmailAndPassword(viewModel.Email, viewModel.Password);

            this.Request.Session.Add(SessionStore.CurrentUserKey, currentUser.Id);

            OrderGames(currentUser.Id);

            return CartGet();
        }

        private void OrderGames(int? currentUserId)
        {
            var currentUser = this.UserDataService.FindUser(currentUserId.Value);

            var cart = (ShoppingCart)this.Request.Session.Get(ShoppingCart.SessionKey);

            var games = cart.GameIds
                .Select(g => this.GameDataService.FindGame(g))
                .ToList();

            foreach (var game in games)
            {
                var userGameExists = this.UserDataService
                    .Context
                    .UserGames
                    .FirstOrDefault(ug => ug.GameId == game.Id && ug.UserId == currentUserId) != null;

                //If user hasn't bought the game add it to his account
                if (!userGameExists)
                {
                    var userGame = new UserGame()
                    {
                        GameId = game.Id,
                        UserId = currentUserId.Value
                    };
                 
                    this.UserDataService.AddUserGame(userGame, currentUserId.Value);
                    this.GameDataService.AddUserGame(userGame, game.Id);
                }
            }

            //reseting cart 
            this.Request.Session.Remove(ShoppingCart.SessionKey);
            this.Request.Session.Add(ShoppingCart.SessionKey, new ShoppingCart());
        }
    }
}