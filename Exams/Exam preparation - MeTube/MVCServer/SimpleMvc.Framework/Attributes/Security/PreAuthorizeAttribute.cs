using System;
using WebServer.Http.Contracts;
using WebServer.Http.Response;

namespace SimpleMvc.Framework.Attributes.Security
{
    [AttributeUsage(AttributeTargets.Method)]
    public class PreAuthorizeAttribute : Attribute
    {
        public virtual IHttpResponse GetResponse(string message)
        {
            return new UnauthorizedResponse(message);
        }
    }
}
