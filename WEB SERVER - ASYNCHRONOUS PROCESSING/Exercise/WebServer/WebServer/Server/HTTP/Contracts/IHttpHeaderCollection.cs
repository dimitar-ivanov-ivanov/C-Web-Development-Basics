namespace WebServer.Server.HTTP.Contracts
{
    using System.Collections.Generic;

    public interface IHttpHeaderCollection : IEnumerable<HttpHeader>
    {
        void Add(HttpHeader header);

        bool ContainsKey(string key);

        HttpHeader GetHeader(string key);
    }
}
