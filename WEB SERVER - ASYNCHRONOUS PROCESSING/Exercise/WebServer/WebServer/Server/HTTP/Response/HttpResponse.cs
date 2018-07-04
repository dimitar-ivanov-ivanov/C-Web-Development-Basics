namespace WebServer.Server.HTTP.Response
{
    using System.Text;
    using Server.HTTP.Contracts;

    public abstract class HttpResponse : IHttpResponse
    {
        private string statusCodeMessage => this.StatusCode.ToString();

        protected HttpResponse()
        {
            this.HeaderCollection = new HttpHeaderCollection();
        }

        public IHttpHeaderCollection HeaderCollection { get; }

        public Enums.HttpStatusCode StatusCode { get; protected set; }

        public override string ToString()
        {
            var response = new StringBuilder();

            var statusCodeNumber = (int)this.StatusCode;
            response.AppendLine($"HTTP/1.1 {statusCodeNumber} {this.statusCodeMessage}");

            response.AppendLine(this.HeaderCollection.ToString());

            return response.ToString();
        }
    }
}
