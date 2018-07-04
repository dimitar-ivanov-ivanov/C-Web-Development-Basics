namespace HTTPServer.GameStoreApplication.Infrastructure
{
    using HTTPServer.Server.Enums;
    using HTTPServer.Server.Http.Contracts;
    using HTTPServer.Server.Http.Response;
    using HTTPServer.Server.Views;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public abstract class Controller
    {
        public const string DefaultPath = @"..\..\..\{0}\Resources\{1}.html";
        public const string ContentPlaceholder = "{{{content}}}";
        public const string HeaderPlaceholder = "{{{headerContent}}}";

        protected Controller()
        {
            this.ViewData = new Dictionary<string, string>
            {
                ["authDisplay"] = "block",
            };
        }

        protected IDictionary<string, string> ViewData { get; private set; }

        protected virtual string AlternativePath { get; }

        protected IHttpResponse RedirectResponse(string redirectUrl)
        {
            return new RedirectResponse(redirectUrl);
        }

        protected IHttpResponse FileViewResponse(string fileName, string headerNameFile)
        {
            var result = this.ProcessFileHtml(fileName, headerNameFile);

            if (this.ViewData.Any())
            {
                foreach (var value in this.ViewData)
                {
                    result = result.Replace($"{{{{{{{value.Key}}}}}}}", value.Value);
                }
            }

            return new ViewResponse(HttpStatusCode.Ok, new FileView(result));
        }

        private string ProcessFileHtml(string fileName, string headerFileName)
        {
            var layoutHtml = File.ReadAllText(string.Format(DefaultPath, AlternativePath, "layout"));

            var fileHtml = File
                .ReadAllText(string.Format(DefaultPath, AlternativePath, fileName));

            var headerHtml = File
                .ReadAllText(string.Format(DefaultPath, AlternativePath, headerFileName));

            var result = layoutHtml.Replace(ContentPlaceholder, fileHtml);
            result = result.Replace(HeaderPlaceholder, headerHtml);

            return result;
        }
    }
}
