namespace HTTPServer
{
    using HTTPServer.ByTheCakeApplication;
    using HTTPServer.ByTheCakeApplication.Data;
    using HTTPServer.Server;
    using HTTPServer.Server.Routing;
    using System.Globalization;
    using System.Threading;

    public class Launcher
    {
        public const int Port = 8000;

        public static void Main(string[] args)
        {
            Run(args);
        }

        private static void Run(string[] args)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

            var application = new ByTheCakeApp();
            var appRouteConfig = new AppRouteConfig();
            application.Configure(appRouteConfig);

            var server = new WebServer(8000, appRouteConfig);

            server.Run();
        }
    }
}
