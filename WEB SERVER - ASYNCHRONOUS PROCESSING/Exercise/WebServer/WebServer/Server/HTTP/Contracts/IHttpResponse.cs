namespace WebServer.Server.HTTP.Contracts
{
    using Server.Enums;

    public interface IHttpResponse
    {
        HttpStatusCode StatusCode { get; }

        IHttpHeaderCollection HeaderCollection { get; }
    }
}
