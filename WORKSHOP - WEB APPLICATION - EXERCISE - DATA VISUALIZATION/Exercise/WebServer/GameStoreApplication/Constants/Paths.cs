namespace HTTPServer.GameStoreApplication.Constants
{
    public class Paths
    {
        public const string RegisterView = @"account\register";

        public const string LoginView = @"account\login";

        public const string HomeView = "index";

        public const string UserHeaderView = @"headers\user";

        public const string AdminHeaderView = @"headers\admin";

        public const string GuestHeaderView = @"headers\guest";

        public const string AddGameView = @"games\add-game";

        public const string ListGamesView = @"games\list-games";

        public const string DeleteGameView = @"games\delete-game";

        public const string EditGameView = @"games\edit-game";

        public const string DetailsGameView = @"games\details-game";

        public const string CartView = @"shopping\cart";

        public const string RegisterPath = "/register";

        public const string LoginPath = "/login";

        public const string HomePath = "/";

        public const string AddGamePath = "/add-game";

        public const string ListGamesPath = "/list-games";

        public const string DeleteGamePath = "/delete-game/{0}";

        public const string EditGamePath = "/edit-game/{0}";

        public const string DetailsGamePath = @"/details-game/{0}";

        public const string BuyGamePath = @"/buy-game/{0}";

        public const string RemoveGamePath = @"/remove-game/{0}";

        public const string OrderLoginPath = @"/order/login";

        public const string CartPath = @"/cart";
    }
}
