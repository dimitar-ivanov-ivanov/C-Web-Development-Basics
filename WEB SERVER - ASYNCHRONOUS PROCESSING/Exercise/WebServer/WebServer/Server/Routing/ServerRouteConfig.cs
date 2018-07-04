namespace WebServer.Server.Routing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using Server.Enums;
    using Server.Routing.Contracts;

    public class ServerRouteConfig : IServerRouteConfig
    {
        private readonly IDictionary<HttpRequestMethod, IDictionary<string, IRoutingContext>> routes;

        public ServerRouteConfig(IAppRouteConfig appRouteConfig)
        {
            this.routes = new Dictionary<HttpRequestMethod, IDictionary<string, IRoutingContext>>();

            var availableMethods = Enum
                .GetValues(typeof(HttpRequestMethod))
                .Cast<HttpRequestMethod>();

            foreach (var method in availableMethods)
            {
                this.routes[method] = new Dictionary<string, IRoutingContext>();
            }

            this.InitializeRouteConfig(appRouteConfig);
        }

        public IDictionary<HttpRequestMethod, IDictionary<string, IRoutingContext>> Routes => this.routes;

        private void InitializeRouteConfig(IAppRouteConfig appRouteConfig)
        {
            foreach (var kvp in appRouteConfig.Routes)
            {
                foreach (var requestHandler in kvp.Value)
                {
                    var args = new List<string>();

                    var parsedRegex = this.ParseRoute(requestHandler.Key, args);

                    var routingContext = new RoutingContext(requestHandler.Value, args);

                    this.Routes[kvp.Key].Add(parsedRegex, routingContext);
                }
            }
        }

        private string ParseRoute(string key, List<string> args)
        {
            var parsedRegex = new StringBuilder();

            parsedRegex.Append("^/");

            if (key == "/")
            {
                return $"^{key}$";
            }

            var tokens = key.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

            this.ParseTokens(args, tokens, parsedRegex);

            return parsedRegex.ToString();

        }

        private void ParseTokens(List<string> args, string[] tokens, StringBuilder parsedRegex)
        {
            for (int idx = 0; idx < tokens.Length; idx++)
            {
                var end = idx == tokens.Length - 1 ? "$" : "/";
                if (!tokens[idx].StartsWith("{") && !tokens[idx].EndsWith("}"))
                {
                    parsedRegex.Append($"{tokens[idx]}{end}");
                    continue;
                }

                var pattern = "<\\w+>";
                var regex = new Regex(pattern);
                var match = regex.Match(tokens[idx]);

                if (!match.Success)
                {
                    continue;
                }

                var paramName = match.Groups[0].Value.Substring(1, match.Groups[0].Length - 2);
                args.Add(paramName);

                parsedRegex.Append($"{tokens[idx].Substring(1, tokens[idx].Length - 2)}{end}");
            }
        }
    }
}
