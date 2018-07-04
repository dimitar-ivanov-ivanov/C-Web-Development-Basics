namespace HTTPServer
{
    using HTTPServer.GameStoreApplication;
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

            //Initialize application
            var application = new GameStoreApp();

            var appRouteConfig = new AppRouteConfig();

            //Configure App Route Configuration
            application.Configure(appRouteConfig);

            var server = new WebServer(8000, appRouteConfig);

            server.Run();
        }
    }
}
