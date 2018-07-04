namespace KittenApp.Web.Controllers
{
    using KittenApp.Models;
    using KittenApp.Web.Models.BindingModels;
    using SimpleMvc.Common;
    using SimpleMvc.Framework.Attributes.Methods;
    using SimpleMvc.Framework.Interfaces;
    using System.Linq;

    public class UsersController : BaseController
    {
        [HttpGet]
        public IActionResult Register()
        {
            if (this.User.IsAuthenticated)
            {
                return this.RedirectHome();
            }

            return this.View();
        }

        [HttpPost]
        public IActionResult Register(UserRegisterModel model)
        {
            if (this.User.IsAuthenticated)
            {
                return this.RedirectHome();
            }

            if (!this.IsValidModel(model))
            {
                this.Model.Data["error"] = ErrorMessages.InvalidRegisterModel;
                return this.View();
            }

            var passwordString = string.Join("", new PasswordUtilities().GenerateHash(model.Password));

            var user = new User()
            {
                Email = model.Email,
                PasswordHash = passwordString,
                Username = model.Username
            };

            this.Context.Users.Add(user);
            this.Context.SaveChanges();
            this.SignIn(user.Username);

            return this.RedirectHome();
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (this.User.IsAuthenticated)
            {
                return this.RedirectHome();
            }

            return this.View();
        }

        [HttpPost]
        public IActionResult Login(UserLoginModel model)
        {
            if (this.User.IsAuthenticated)
            {
                return this.RedirectHome();
            }

            if (!this.IsValidModel(model))
            {
                this.Model.Data["error"] = ErrorMessages.InvalidLoginModel;
                return this.View();
            }

            var userExists = this.Context.Users.FirstOrDefault(u => u.Username == model.Username);

            if (userExists == null)
            {
                this.Model.Data["error"] = ErrorMessages.InvalidLoginModel;
                return this.View();
            }

            this.SignIn(model.Username);

            return this.RedirectHome();
        }

        [HttpGet]
        public IActionResult Logout()
        {
            if (!this.User.IsAuthenticated)
            {
                return this.RedirectHome();
            }

            this.SignOut();

            return this.RedirectHome();
        }
    }
}
