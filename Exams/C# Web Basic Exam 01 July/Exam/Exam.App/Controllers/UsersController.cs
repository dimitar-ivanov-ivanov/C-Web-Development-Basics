namespace Exam.App.Controllers
{
    using Exam.App.Models.BindingModels;
    using Exam.Models;
    using SoftUni.WebServer.Common;
    using SoftUni.WebServer.Mvc.Attributes.HttpMethods;
    using SoftUni.WebServer.Mvc.Interfaces;
    using System.Collections.Generic;
    using System.Linq;

    public class UsersController : BaseController
    {
        [HttpGet]
        public IActionResult Register() => this.User.IsAuthenticated ? this.RedirectToHome() : this.View();

        [HttpPost]
        public IActionResult Register(UserRegisterBindingModel model)
        {
            if (this.User.IsAuthenticated)
            {
                return this.RedirectToHome();
            }

            if (!this.IsValidModel(model))
            {
                return this.BuildErrorView();
            }

            string passwordHash = PasswordUtilities.GetPasswordHash(model.Password);

            var user = new User()
            {
                Username = model.Username,
                Email = model.Email,
                FullName = model.FullName,
                Role = this.Context.Roles.FirstOrDefault(u => u.Name == "User"),
                PasswordHash = passwordHash
            };

            if (this.Context.Users.Count() == 0)
            {
                user.Role = this.Context.Roles.FirstOrDefault(u => u.Name == "Admin");
            }

            using (this.Context)
            {
                this.Context.Users.Add(user);
                this.Context.SaveChanges();
            }

            this.SignIn(user.Username, user.Id, new List<string>() { "User", "Admin" });
            return this.RedirectToHome();
        }

        [HttpGet]
        public IActionResult Login() => this.User.IsAuthenticated ? this.RedirectToHome() : this.View();

        [HttpPost]
        public IActionResult Login(UserLoginBindingModel model)
        {
            if (this.User.IsAuthenticated)
            {
                return this.RedirectToHome();
            }

            if (!this.IsValidModel(model))
            {
                return this.BuildErrorView();
            }

            User user;
            using (this.Context)
            {
                user = this.Context.Users
                    .FirstOrDefault(u => u.Username == model.Username);
            }

            var passwordHash = PasswordUtilities.GetPasswordHash(model.Password);

            if (passwordHash != user.PasswordHash)
            {
                return this.BuildErrorView();
            }

            this.SignIn(user.Username, user.Id, new List<string>() { "User", "Admin" });
            return this.RedirectToHome();
        }

        [HttpGet]
        public IActionResult Logout()
        {
            if (this.User.IsAuthenticated)
            {
                this.SignOut();
            }

            return this.RedirectToHome();
        }

    }
}
