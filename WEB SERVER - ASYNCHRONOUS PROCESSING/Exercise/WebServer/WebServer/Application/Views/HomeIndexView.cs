namespace WebServer.Application.Views
{
    using WebServer.Server.Contracts;

    public class HomeIndexView : IView
    {
        public string View()
        {
            return "<h1>Welcome</h1>";
        }
    }
}
