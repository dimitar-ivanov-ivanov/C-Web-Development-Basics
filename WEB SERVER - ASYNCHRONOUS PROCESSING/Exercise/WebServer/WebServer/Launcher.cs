namespace WebServer
{
    using WebServer.Application;
    using WebServer.Server.Contracts;
    using WebServer.Server.Routing;

    public class Launcher : IRunnable
    {
        private Server.WebServer webServer;

        public static void Main(string[] args)
        {
            new Launcher().Run();
        }

        public void Run()
        {
            var app = new MainApplication();
            var routeConfig = new AppRouteConfig();
            app.Start(routeConfig);

            this.webServer = new Server.WebServer(8230, routeConfig);
            this.webServer.Run();
        }
    }
}
