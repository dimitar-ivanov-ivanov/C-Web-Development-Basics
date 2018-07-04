namespace HTTPServer.ByTheCakeApplication.Controllers
{
    using System;
    using System.Linq;
    using HTTPServer.ByTheCakeApplication.Data;
    using HTTPServer.ByTheCakeApplication.Utilities;
    using HTTPServer.Server.Common;
    using Infrastructure;
    using Models;
    using Server.Http;
    using Server.Http.Contracts;
    using Server.Http.Response;

    public class AccountController : Controller
    {
        public IHttpResponse Register()
        {
            this.ViewData["showError"] = "none";
            this.ViewData["authDisplay"] = "none";
            return this.FileViewResponse(@"account\register");
        }

        public IHttpResponse Register(IHttpRequest req)
        {
            const string formNameKey = "name";
            const string formUsernameKey = "username";
            const string formPasswordKey = "password";
            const string formConfirmPasswordKey = "confirm-password";

            if (!req.FormData.ContainsKey(formNameKey) ||
               !req.FormData.ContainsKey(formUsernameKey) ||
               !req.FormData.ContainsKey(formPasswordKey) ||
               !req.FormData.ContainsKey(formConfirmPasswordKey))
            {
                return new BadRequestResponse();
            }

            var name = req.FormData[formNameKey];
            var username = req.FormData[formUsernameKey];
            var password = req.FormData[formPasswordKey];
            var confirmPassword = req.FormData[formConfirmPasswordKey];

            try
            {
                CoreValidator.ThrowIfNotLongEnough(name, 3, nameof(name));
                CoreValidator.ThrowIfNotLongEnough(username, 3, nameof(name));
                CoreValidator.ThrowIfNullOrEmpty(password, nameof(password));
                CoreValidator.ThrowIfNullOrEmpty(confirmPassword, nameof(password));
                CoreValidator.ThrowIfNotEqual(password, confirmPassword, nameof(password), nameof(confirmPassword));
            }
            catch (ArgumentException)
            {
                this.ViewData["error"] = "You have empty fields";
                this.ViewData["showError"] = "block";

                return this.FileViewResponse(@"account\login");
            }

            var user = new User()
            {
                Name = name,
                Username = username,
                PasswordHash = PasswordUtilities.ComputeHash(password),
                RegistrationDate = DateTime.UtcNow,
                
            };

            using (var context = new ByTheCakeContext())
            {
                context.Users.Add(user);
                context.SaveChanges();
            }

            return CompleteLogin(req, user.Id);
        }

        public IHttpResponse Login()
        {
            this.ViewData["showError"] = "none";
            this.ViewData["authDisplay"] = "none";

            return this.FileViewResponse(@"account\login");
        }

        public IHttpResponse Login(IHttpRequest req)
        {
            const string formNameKey = "name";
            const string formPasswordKey = "password";

            if (!req.FormData.ContainsKey(formNameKey) ||
                !req.FormData.ContainsKey(formPasswordKey))
            {
                return new BadRequestResponse();
            }

            var name = req.FormData[formNameKey];
            var password = req.FormData[formPasswordKey];

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(password))
            {
                this.ViewData["error"] = "You have empty fields";
                this.ViewData["showError"] = "block";

                return this.FileViewResponse(@"account\login");
            }

            User dbUser = null;
            using (var context = new ByTheCakeContext())
            {
                dbUser = context.Users.FirstOrDefault(u => u.Username == name);
            }

            var passwordHash = PasswordUtilities.ComputeHash(password);

            if (dbUser == null || dbUser.PasswordHash != passwordHash)
            {
                return RejectLoginAttempt();
            }

            return CompleteLogin(req, dbUser.Id);
        }

        public IHttpResponse Logout(IHttpRequest req)
        {
            req.Session.Clear();

            return new RedirectResponse("/login");
        }

        public IHttpResponse Profile(IHttpRequest req)
        {
            var currentUserId = req.Session.Get<int>(SessionStore.CurrentUserKey);
            User currentUser = null;

            using (var context = new ByTheCakeContext())
            {
                currentUser = context.Users.Find(currentUserId);

                var ordersCount = context.Orders.Where(o => o.UserId == currentUser.Id).Count();

                this.ViewData["name"] = currentUser.Name;
                this.ViewData["registrationDate"] = currentUser.RegistrationDate.ToString("dd-MM-yyyy");
                this.ViewData["ordersCount"] = ordersCount.ToString();
            }

            return this.FileViewResponse(@"account\profile");
        }

        private IHttpResponse RejectLoginAttempt()
        {
            this.ViewData["error"] = "There is no such user";
            this.ViewData["showError"] = "block";
            this.ViewData["authDisplay"] = "none";

            return this.FileViewResponse(@"account\login");
        }

        private static IHttpResponse CompleteLogin(IHttpRequest req, int id)
        {
            req.Session.Add(SessionStore.CurrentUserKey, id);
            req.Session.Add(ShoppingCart.SessionKey, new ShoppingCart());

            return new RedirectResponse("/");
        }
    }
}