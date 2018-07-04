using HTTPServer.Server.Contracts;

namespace HTTPServer.Server.Views
{
    public class FileView : IView
    {
        private readonly string htmlFile;

        public FileView(string htmlFile)
        {
            this.htmlFile = htmlFile;
        }

        public string View() => this.htmlFile;
    }
}
