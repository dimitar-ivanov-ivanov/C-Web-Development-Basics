﻿namespace SimpleMvc.App
{
    using SimpleMvc.Framework;
    using SimpleMvc.Framework.Routers;
    using WebServer;

    public class Launcher
    {
        public static void Main()
        {
            var server = new WebServer(8000, new ControllerRouter());
            MvcEngine.Run(server);
        }
    }
}
 