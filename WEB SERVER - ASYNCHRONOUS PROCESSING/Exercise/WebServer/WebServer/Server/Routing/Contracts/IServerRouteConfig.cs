namespace WebServer.Server.Routing.Contracts
{
    using System.Collections.Generic;
    using Server.Enums;

    public interface IServerRouteConfig
    {
        IDictionary<HttpRequestMethod, IDictionary<string, IRoutingContext>> Routes { get; }
    }
}
