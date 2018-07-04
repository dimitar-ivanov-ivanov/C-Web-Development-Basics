namespace WebServer.Server
{
    using System;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading.Tasks;
    using Server.Handlers;
    using Server.HTTP;
    using Server.HTTP.Contracts;
    using Server.Routing.Contracts;

    public class ConnectionHandler
    {
        private readonly Socket client;

        private readonly IServerRouteConfig serverRouteConfig;

        public ConnectionHandler(Socket client, IServerRouteConfig serverRouteConfig)
        {
            this.client = client;
            this.serverRouteConfig = serverRouteConfig;
        }

        public async Task ProcessRequestAsync()
        {
            var request = await this.ReadRequest();

            var httpContext = new HttpContext(request);

            var response = new HttpHandler(this.serverRouteConfig).Handle(httpContext);

            var toBytes = new ArraySegment<byte>(Encoding.UTF8.GetBytes(response.ToString()));

            await this.client.SendAsync(toBytes, SocketFlags.None);

            Console.WriteLine($"-----REQUEST-----");
            Console.WriteLine(request);

            Console.WriteLine($"-----RESPONSE-----");
            Console.WriteLine(response.ToString());

            this.client.Shutdown(SocketShutdown.Both);
        }

        public async Task<IHttpRequest> ReadRequest()
        {
            var result = new StringBuilder();

            var data = new ArraySegment<byte>(new byte[1024]);

            while (true)
            {
                int numberOfBytesRead = await this.client.ReceiveAsync(data.Array, SocketFlags.None);

                if (numberOfBytesRead == 0)
                {
                    break;
                }

                var bytesAsString = Encoding.UTF8.GetString(data.Array, 0, numberOfBytesRead);

                result.Append(bytesAsString);

                if (numberOfBytesRead < 1023)
                {
                    break;
                }
            }

            if (result.Length == 0)
            {
                return null;
            }

            return new HttpRequest(result.ToString());
        }
    }
}
