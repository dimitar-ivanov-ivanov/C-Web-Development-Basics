namespace SimpleMvc.Framework
{
    public class MvcContext
    {
        private static MvcContext instance;
        private static readonly object instanceLock = new object();

        private MvcContext() { }

        public static MvcContext Get
        {
            get
            {
                if (instance == null)
                {
                    //This achieves thead safety
                    lock (instanceLock)
                    {
                        if (instance == null)
                        {
                            instance = new MvcContext();
                        }
                    }
                }

                return instance;
            }
        }

        public string AssemblyName { get; set; }

        public string ControllersFolders { get; set; }

        public string ModelsFolder { get; set; }

        public string ViewsFolder { get; set; }

        public string ControllerSuffix { get; set; }
    }
}
