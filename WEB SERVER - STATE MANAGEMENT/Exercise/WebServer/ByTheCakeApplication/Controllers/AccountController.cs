namespace HTTPServer.ByTheCakeApplication.Controllers
{
    using HTTPServer.ByTheCakeApplication.Infrastructure;
    using HTTPServer.Server.Http.Contracts;
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class AccountController : Controller
    {
        public IHttpResponse Login()
        {
            var loginValues = new Dictionary<string, string>
            {
                ["result"] = string.Empty,
            };

            return this.FileViewResponse(@"account\login", loginValues);
        }

        public IHttpResponse Login(string username, string password)
        {
            const string LoginMessage = "Hi {0}, your password is {1}";
            var result = string.Format(LoginMessage, username, password);

            var loginValues = new Dictionary<string, string>
            {
                ["result"] = result,
            };

            return this.FileViewResponse(@"account\login", loginValues);
        }
    }
}
