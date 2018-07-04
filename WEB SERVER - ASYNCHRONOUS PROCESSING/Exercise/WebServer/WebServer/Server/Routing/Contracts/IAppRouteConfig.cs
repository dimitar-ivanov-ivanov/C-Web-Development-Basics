namespace WebServer.Server.Routing.Contracts
{
    using System.Collections.Generic;
    using Server.Enums;
    using Server.Handlers;

    public interface IAppRouteConfig
    {
        IReadOnlyDictionary<HttpRequestMethod, Dictionary<string, RequestHandler>> Routes
        {
            get;
        }

        void AddRoute(string route, RequestHandler httpHandler);
    }
}
