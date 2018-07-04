namespace HTTPServer
{
    using HTTPServer.ByTheCakeApplication;
    using HTTPServer.Server;
    using HTTPServer.Server.Routing;
    using System.Globalization;
    using System.Threading;

    public class Launcher
    {
        public static void Main(string[] args)
        {
            Run(args);
        }

        private static void Run(string[] args)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            var app = new ByTheCakeApp();
            var appRouteConfig = new AppRouteConfig();
            app.Configure(appRouteConfig);

            var server = new WebServer(8230, appRouteConfig);

            server.Run();
        }
    }
}
