namespace WebServer.Server.Handlers
{
    using System;
    using Server.Handlers.Contracts;
    using Server.HTTP;
    using Server.HTTP.Contracts;

    public abstract class RequestHandler : IRequestHandler
    {
        private readonly Func<IHttpRequest, IHttpResponse> handlingFunc;

        protected RequestHandler(Func<IHttpRequest, IHttpResponse> handlingFunc)
        {
            this.handlingFunc = handlingFunc;
        }

        public IHttpResponse Handle(IHttpContext context)
        {
            var response = this.handlingFunc(context.Request);

            if (!response.HeaderCollection.ContainsKey("Content-Type"))
            {
                response.HeaderCollection.Add(new HttpHeader("Content-Type", "text/plain"));
            }

            return response;
        }
    }
}
