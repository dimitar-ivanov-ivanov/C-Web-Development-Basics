namespace WebServer.Application.Controllers
{
    using WebServer.Application.Views;
    using WebServer.Server.Enums;
    using WebServer.Server.HTTP.Contracts;
    using WebServer.Server.HTTP.Response;

    public class HomeController
    {
        public IHttpResponse Index()
        {
            return new ViewResponse(HttpStatusCode.Ok, new HomeIndexView());
        }
    }
}
