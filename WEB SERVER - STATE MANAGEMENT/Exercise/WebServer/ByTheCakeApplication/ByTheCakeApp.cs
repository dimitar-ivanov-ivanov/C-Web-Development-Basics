namespace HTTPServer.ByTheCakeApplication
{
    using HTTPServer.ByTheCakeApplication.Controllers;
    using HTTPServer.Server.Contracts;
    using HTTPServer.Server.Routing.Contracts;

    public class ByTheCakeApp : IApplication
    {
        public void Configure(IAppRouteConfig appRouteConfig)
        {
            appRouteConfig.Get("/",
                req => new HomeController().Index());

            appRouteConfig.Get("/about",
                req => new HomeController().About());

            appRouteConfig
                  .Get("/add", req => new CakesController().Add());

            appRouteConfig
                .Post("/add",
                req => new CakesController().Add(
                    req.FormData["name"], req.FormData["price"]));

            appRouteConfig
                .Get("/search",
                req => new CakesController().Search(req.UrlParameters));

            appRouteConfig
                .Get("/login",
                req => new AccountController().Login());

            appRouteConfig
                .Post("/login",
                req => new AccountController()
                .Login(req.FormData["name"], req.FormData["password"]));
        }
    }
}
