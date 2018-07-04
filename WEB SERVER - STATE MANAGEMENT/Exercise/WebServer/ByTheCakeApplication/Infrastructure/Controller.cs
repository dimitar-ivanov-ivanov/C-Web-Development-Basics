namespace HTTPServer.ByTheCakeApplication.Infrastructure
{
    using HTTPServer.ByTheCakeApplication.Views;
    using HTTPServer.Server.Enums;
    using HTTPServer.Server.Http.Contracts;
    using HTTPServer.Server.Http.Response;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public class Controller
    {
        private const string DefaultPath = @"..\..\..\ByTheCakeApplication\Resources\{0}.html";
        private const string ContentPlaceholder = "{{{content}}}";

        public IHttpResponse FileViewResponse(string fileName)
        {
            var resultHtml = ProcessFileHtml(fileName);

            return new ViewResponse(HttpStatusCode.Ok, new FileView(resultHtml));
        }

        public IHttpResponse FileViewResponse(string fileName, Dictionary<string, string> values)
        {
            var resultHtml = ProcessFileHtml(fileName);

            if (values != null && values.Any())
            {
                foreach (var value in values)
                {
                    resultHtml = resultHtml.Replace($"{{{{{{{value.Key}}}}}}}", value.Value);
                }
            }

            return new ViewResponse(HttpStatusCode.Ok, new FileView(resultHtml));
        }

        private static string ProcessFileHtml(string fileName)
        {
            var layoutHtml = File.ReadAllText(string.Format(DefaultPath, "layout"));
            var fileHtml = File.ReadAllText(string.Format(DefaultPath, fileName));

            var result = layoutHtml.Replace(ContentPlaceholder, fileHtml);

            return result;
        }
    }
}