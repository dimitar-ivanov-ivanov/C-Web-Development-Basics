namespace SimpleMvc.Framework
{
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Reflection;

    public class MvcEngine
    {
        public static void Run(WebServer.WebServer server, DbContext context)
        {
            ConfigureDatabaseContext(context);
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

        public static void ConfigureDatabaseContext(DbContext context)
        {
            using (context)
            {
                context.Database.Migrate();
            }
        }

        private static void ConfigureMvcContext(MvcContext context)
        {
            context.ViewsFolder = "Views";
            context.ModelsFolder = "Models";
            context.ControllersFolders = "Controllers";
            context.ControllerSuffix = "Controller";
            context.AssemblyNameFileSuffix = ".dll";
            context.AssemblyName = Assembly.GetEntryAssembly().GetName().Name;
        }
    }
}