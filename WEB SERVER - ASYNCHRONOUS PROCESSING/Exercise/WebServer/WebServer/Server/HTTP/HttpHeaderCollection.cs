namespace WebServer.Server.HTTP
{
    using System.Collections;
    using System.Collections.Generic;
    using HTTP.Contracts;

    public class HttpHeaderCollection : IHttpHeaderCollection
    {
        private readonly IDictionary<string, HttpHeader> headers;

        public HttpHeaderCollection()
        {
            this.headers = new Dictionary<string, HttpHeader>();
        }

        public void Add(HttpHeader header)
        {
            this.headers.Add(header.Key, header);
        }

        public bool ContainsKey(string key)
        {
            return this.headers.ContainsKey(key);
        }

        public IEnumerator<HttpHeader> GetEnumerator()
        {
            foreach (var pair in this.headers)
            {
                yield return pair.Value;
            }
        }

        public HttpHeader GetHeader(string key)
        {
            return this.headers[key];
        }

        public override string ToString()
        {
            return string.Join("\n", this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}