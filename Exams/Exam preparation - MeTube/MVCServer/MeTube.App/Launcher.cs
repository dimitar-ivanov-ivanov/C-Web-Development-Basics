namespace MeTube.App
{
    using MeTube.Data;
    using SimpleMvc.Framework;
    using SimpleMvc.Framework.Routers;

    public class Launcher
    {
        public static void Main(string[] args)
        {
            var server = new WebServer.WebServer(8000,
                  new ControllerRouter(),
                  new ResourceRouter());

            MvcEngine.Run(server, new MeTubeContext());
        }
    }
}