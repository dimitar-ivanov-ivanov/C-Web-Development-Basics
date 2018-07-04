namespace WebServer.Server.Handlers
{
    using System.Text.RegularExpressions;
    using Server.Exceptions;
    using Server.Handlers.Contracts;
    using Server.HTTP.Contracts;
    using Server.Routing.Contracts;

    public class HttpHandler : IRequestHandler
    {
        private readonly IServerRouteConfig serverRouteConfig;

        public HttpHandler(IServerRouteConfig serverRouteConfig)
        {
            this.serverRouteConfig = serverRouteConfig;
        }

        public IHttpResponse Handle(IHttpContext httpContext)
        {
            foreach (var kvp in this.serverRouteConfig.Routes
                [httpContext.Request.RequestMethod])
            {
                var pattern = kvp.Key;
                var regex = new Regex(pattern);
                var match = regex.Match(httpContext.Request.Path);

                if (!match.Success)
                {
                    continue;
                }

                foreach (var parameter in kvp.Value.Parameters)
                {
                    httpContext.Request.AddUrlParameter(parameter,
                        match.Groups[parameter].Value);
                }

                return kvp.Value.RequestHandler.Handle(httpContext);
            }

            throw new BadRequestException("Not Found Response");
        }
    }
}