using WebServer.Enums;

namespace WebServer.Http.Response
{
    public class UnauthorizedResponse : HttpResponse
    {
        public UnauthorizedResponse(string message)
        {
            this.StatusCode = HttpStatusCode.NotAuthorized;
            // TODO: Add message
        }
    }
}
