namespace SimpleMvc.Framework
{
    using System;
    using System.Reflection;

    public class MvcEngine
    {
        public static void Run(WebServer.WebServer server)
        {
            ConfigureMvcContext(MvcContext.Get);

            while (true)
            {
                try
                {
                    server.Run();
                }
                catch (Exception e)
                {
                    //Log errors
                    Console.WriteLine(e.Message);
                }
            }
        }

        private static void ConfigureMvcContext(MvcContext context)
        {
            context.ViewsFolder = "Views";
            context.ModelsFolder = "Models";
            context.ControllersFolders = "Controllers";
            context.ControllerSuffix = "Controller";
            context.AssemblyName = Assembly.GetEntryAssembly().GetName().Name;
        }
    }
}