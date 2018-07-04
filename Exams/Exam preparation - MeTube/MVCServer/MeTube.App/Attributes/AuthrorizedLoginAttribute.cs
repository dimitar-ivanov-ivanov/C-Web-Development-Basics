namespace MeTube.App.Attributes
{
    using SimpleMvc.Framework.Attributes.Security;
    using WebServer.Http.Contracts;
    using WebServer.Http.Response;

    public class AuthrorizedLoginAttribute : PreAuthorizeAttribute
    {
        public override IHttpResponse GetResponse(string message)
        {
            return new RedirectResponse("/users/login");
        }
    }
}