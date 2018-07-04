namespace WebServer.Server.HTTP
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using Server.Enums;
    using Server.Exceptions;
    using Server.HTTP.Contracts;

    public class HttpRequest : IHttpRequest
    {
        private readonly string requestString;

        public HttpRequest(string requestString)
        {
            this.requestString = requestString;

            this.HeaderCollection = new HttpHeaderCollection();
            this.UrlParameters = new Dictionary<string, string>();
            this.QueryParameters = new Dictionary<string, string>();
            this.FormData = new Dictionary<string, string>();

            this.ParseRequest(requestString);
        }

        public IDictionary<string, string> FormData { get; }

        public IHttpHeaderCollection HeaderCollection { get; }

        public string Path { get; private set; }

        public IDictionary<string, string> QueryParameters { get; }

        public HttpRequestMethod RequestMethod { get; private set; }

        public string Url { get; private set; }

        public IDictionary<string, string> UrlParameters { get; }

        private void ParseRequest(string requestString)
        {
            var requestLines = requestString
                .Split(new[] { Environment.NewLine }, StringSplitOptions.None);

            var requestLine = requestLines[0].Trim()
                .Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);

            if (requestLine.Length != 3 || requestLine[2].ToLower() != "http/1.1")
            {
                throw new BadRequestException("Invalid request line");
            }

            this.RequestMethod = this.ParseRequestMethod(requestLine[0].ToUpper());
            this.Url = requestLine[1];

            this.Path = this.Url
                .Split(new[] { '?', '#' }, StringSplitOptions.RemoveEmptyEntries)[0];

            this.ParseHeaders(requestLines);
            this.ParseParameters();

            if (this.RequestMethod == HttpRequestMethod.Post)
            {
                this.ParseQuery(requestLines[requestLines.Length - 1], this.FormData);
            }
        }

        private void ParseParameters()
        {
            if (!this.Url.Contains("?"))
            {
                return;
            }

            var query = this.Url.Split(new[] { "?" }, StringSplitOptions.RemoveEmptyEntries)[1];
            this.ParseQuery(query, this.QueryParameters);
        }

        private void ParseQuery(string query, IDictionary<string, string> dict)
        {
            if (!query.Contains("="))
            {
                return;
            }

            var queryPairs = query.Split(new[] { "&" }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var queryPair in queryPairs)
            {
                var queryArgs = queryPair.Split(new[] { "=" }, StringSplitOptions.RemoveEmptyEntries);

                if (queryPair.Length != 2)
                {
                    continue;
                }

                dict.Add(
                    WebUtility.UrlDecode(queryArgs[0]),
                    WebUtility.UrlDecode(queryArgs[1]));
            }
        }

        private void ParseHeaders(string[] requestLines)
        {
            var endIndex = Array.IndexOf(requestLines, string.Empty);

            for (int i = 1; i < endIndex; i++)
            {
                var headerArgs = requestLines[i]
                    .Split(new[] { ": " }, StringSplitOptions.RemoveEmptyEntries);

                var header = new HttpHeader(headerArgs[0], headerArgs[1].Trim());
                this.HeaderCollection.Add(header);
            }

            if (!this.HeaderCollection.ContainsKey("Host"))
            {
                throw new BadRequestException("No header with name host");
            }
        }

        private HttpRequestMethod ParseRequestMethod(string method)
        {
            HttpRequestMethod parseMethod;

            if (!Enum.TryParse(method, true, out parseMethod))
            {
                throw new BadRequestException("Request isn't valid.");
            }

            return parseMethod;
        }

        public void AddUrlParameter(string key, string value)
        {
            this.UrlParameters[key] = value;
        }

        public override string ToString()
        {
            return this.requestString;
        }
    }
}
