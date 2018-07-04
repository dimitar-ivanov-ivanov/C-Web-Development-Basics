namespace HTTPServer.GameStoreApplication
{
    using HTTPServer.GameStoreApplication.Common;
    using HTTPServer.GameStoreApplication.Controllers;
    using HTTPServer.GameStoreApplication.Data;
    using HTTPServer.GameStoreApplication.Services;
    using HTTPServer.Server.Routing.Contracts;
    using Microsoft.EntityFrameworkCore;

    public class GameStoreApp
    {
        public void Configure(IAppRouteConfig appRouteConfig)
        {
            this.ConfigureRoutes(appRouteConfig);
            this.ConfigureDatabase();
        }

        private void ConfigureRoutes(IAppRouteConfig appRouteConfig)
        {
            var context = new GameStoreContext();

            appRouteConfig
                .Get(
                     "/register",
                     req => new AccountController(
                         req,
                         new UserDataService(
                             context),
                         new GameDataService(
                             context),
                         new HeaderPathFinder())
                         .RegisterGet());


            appRouteConfig
                .Post(
                      "/register",
                      req => new AccountController(
                         req,
                         new UserDataService(
                             context),
                         new GameDataService(
                             context),
                         new HeaderPathFinder())
                         .RegisterPost());

            appRouteConfig
               .Get(
                    "/login",
                    req => new AccountController(
                        req,
                        new UserDataService(
                            context),
                        new GameDataService(
                            context),
                        new HeaderPathFinder())
                        .LoginGet());

            appRouteConfig
             .Post(
                  "/login",
                  req => new AccountController(
                      req,
                      new UserDataService(
                          context),
                      new GameDataService(
                          context),
                      new HeaderPathFinder())
                  .LoginPost());

            appRouteConfig
                .Get(
                "/logout",
                req => new AccountController(
                    req,
                    new UserDataService(
                        context),
                    new GameDataService(
                        context),
                    new HeaderPathFinder())
                    .Logout());

            appRouteConfig
                .Get(
                "/",
                     req => new HomeController(
                         req,
                         new UserDataService(
                             context),
                         new GameDataService(
                             context),
                         new HeaderPathFinder(),
                         new Authenticator(req))
                         .HomeGet());

            appRouteConfig
                .Post(
                "/",
                req => new HomeController(
                   req,
                   new UserDataService(
                       context),
                   new GameDataService(
                       context),
                   new HeaderPathFinder(),
                   new Authenticator(req))
                   .HomePost());

            appRouteConfig
                .Get(
                "/add-game",
                req => new GameController(
                    req,
                    new UserDataService(
                        context),
                    new GameDataService(
                        context),
                    new HeaderPathFinder(),
                    new Authenticator(req))
                    .AddGameGet());

            appRouteConfig
           .Post(
                "/add-game",
                req => new GameController(
                    req,
                    new UserDataService(
                        context),
                    new GameDataService(
                        context),
                    new HeaderPathFinder(),
                    new Authenticator(req))
                    .AddGamePost());

            appRouteConfig
                .Get(
                    "/list-games",
                     req => new GameController(
                         req,
                         new UserDataService(
                             context),
                         new GameDataService(
                             context),
                         new HeaderPathFinder(),
                         new Authenticator(req))
                         .ListGames());

            appRouteConfig
                .Get(
                    "/edit-game/{(?<id>[0-9]+)}",
                    req => new GameController(
                        req,
                        new UserDataService(
                            context),
                        new GameDataService(
                            context),
                        new HeaderPathFinder(),
                        new Authenticator(req))
                        .EditGet());

            appRouteConfig
                .Post(
                     "/edit-game/{(?<id>[0-9]+)}",
                     req => new GameController(
                         req,
                         new UserDataService(
                             context),
                         new GameDataService(
                             context),
                         new HeaderPathFinder(),
                         new Authenticator(req))
                         .EditPost());

            appRouteConfig
                .Get(
                    "/delete-game/{(?<id>[0-9]+)}",
                    req => new GameController(
                        req,
                        new UserDataService(
                            context),
                        new GameDataService(
                            context),
                        new HeaderPathFinder(),
                        new Authenticator(req))
                        .DeleteGet());

            appRouteConfig
                .Post(
                     "/delete-game/{(?<id>[0-9]+)}",
                     req => new GameController(
                         req,
                         new UserDataService(
                             context),
                         new GameDataService(
                             context),
                         new HeaderPathFinder(),
                         new Authenticator(req))
                         .DeletePost());

            appRouteConfig
               .Get(
                   "/details-game/{(?<id>[0-9]+)}",
                   req => new GameController(
                       req,
                       new UserDataService(
                           context),
                       new GameDataService(
                           context),
                       new HeaderPathFinder(),
                       new Authenticator(req))
                       .DetailsGet());

            appRouteConfig
             .Get(
                 "/buy-game/{(?<id>[0-9]+)}",
                 req => new ShoppingController(
                     req,
                     new UserDataService(
                         context),
                     new GameDataService(
                         context),
                     new HeaderPathFinder())
                     .BuyGet());

            appRouteConfig
             .Get(
                 "/remove-game/{(?<id>[0-9]+)}",
                 req => new ShoppingController(
                     req,
                     new UserDataService(
                         context),
                     new GameDataService(
                         context),
                     new HeaderPathFinder())
                     .RemoveGet());

            appRouteConfig
             .Get(
                 "/cart",
                 req => new ShoppingController(
                     req,
                     new UserDataService(
                         context),
                     new GameDataService(
                         context),
                     new HeaderPathFinder())
                     .CartGet());

            appRouteConfig
            .Post(
                "/cart",
                req => new ShoppingController(
                    req,
                    new UserDataService(
                        context),
                    new GameDataService(
                        context),
                    new HeaderPathFinder())
                    .CartPost());

            appRouteConfig
           .Get(
               "/order/login",
               req => new ShoppingController(
                   req,
                   new UserDataService(
                       context),
                   new GameDataService(
                       context),
                   new HeaderPathFinder())
                   .OrderLoginGet());

            appRouteConfig
           .Post(
               "/order/login",
               req => new ShoppingController(
                   req,
                   new UserDataService(
                       context),
                   new GameDataService(
                       context),
                   new HeaderPathFinder())
                   .OrderLoginPost());
        }

        private void ConfigureDatabase()
        {
            using (var context = new GameStoreContext())
            {
                context.Database.Migrate();
            }
        }
    }
}