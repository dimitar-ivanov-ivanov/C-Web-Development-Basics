namespace Exam.App
{
    using SoftUni.WebServer.Mvc;
    using SoftUni.WebServer.Mvc.Routers;
    using SoftUni.WebServer.Server;
    using System.Globalization;
    using System.Threading;

    public class Launcher
    {
        public static void Main(string[] args)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

            var server = new WebServer(8000,
                 new ControllerRouter(),
                 new ResourceRouter());

            MvcEngine.Run(server);
        }
    }
}
