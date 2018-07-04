namespace WebServer.Server.Routing
{
    using System.Collections.Generic;
    using System.Linq;
    using Server.Handlers;
    using Server.Routing.Contracts;

    public class RoutingContext : IRoutingContext
    {
        private readonly List<string> parameters;

        public RoutingContext()
        {
            this.parameters = new List<string>();
        }

        public RoutingContext(RequestHandler requestHandler, IEnumerable<string> parameters)
        {
            this.RequestHandler = requestHandler;
            this.parameters = parameters.ToList();
        }

        public IEnumerable<string> Parameters => parameters;

        public RequestHandler RequestHandler { get; }
    }
}
