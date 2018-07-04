namespace WebServer.Server.HTTP.Response
{
    public class RedirectResponse : HttpResponse
    {
        public RedirectResponse(string redirectUrl)
        {
            this.StatusCode = Enums.HttpStatusCode.Found;

            this.HeaderCollection.Add(new HttpHeader("Location", redirectUrl));
        }
    }
}
