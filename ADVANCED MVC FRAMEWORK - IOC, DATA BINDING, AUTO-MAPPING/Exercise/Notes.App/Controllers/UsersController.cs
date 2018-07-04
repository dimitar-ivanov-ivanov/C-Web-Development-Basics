namespace Notes.App.Controllers
{
    using Notes.App.Models;
    using Notes.Models;
    using SimpleMvc.Framework.Attributes.Methods;
    using SimpleMvc.Framework.Interfaces;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;

    public class UsersController : BaseController
    {
        [HttpGet]
        public IActionResult Register()
        {
            return this.View();
        }

        [HttpPost]
        public IActionResult Register(RegisterUserBindingModel model)
        {
            if (!this.IsValidModel(model))
            {
                return this.View();
            }

            var sh256 = new SHA256Managed();

            var bytes = Encoding.UTF8.GetBytes(model.Password);

            var passwordHash = sh256.ComputeHash(bytes);

            var user = new User()
            {
                Username = model.Username,
                PasswordHash = string.Join("", passwordHash)
            };

            this.Context.Users.Add(user);
            this.Context.SaveChanges();

            //Log in
            this.SignIn(user.Username);
            return RedirectToAction("/home/index");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginUserBindingModel model)
        {
            var foundUser = Context.Users.FirstOrDefault(u => u.Username == model.Username);

            if (foundUser == null)
            {
                return RedirectToAction("/home/login");
            }

            Context.SaveChanges();
            this.SignIn(foundUser.Username);

            return RedirectToAction("/home/index");
        }

        [HttpGet]
        public IActionResult All()
        {
            if (!this.User.IsAuthenticated)
            {
                return RedirectToAction("/users/login");
            }

            var users = new Dictionary<int, string>();

            users = this.Context.Users.ToDictionary(u => u.Id, u => u.Username);

            var builder = new StringBuilder();

            foreach (var user in users)
            {
                builder.AppendLine($@"<li><a href=""/users/profile?id={user.Key}"">{user.Value}</a></li>");
            }

            this.Model["users"] = builder.ToString();

            return this.View();
        }

        [HttpGet]
        public IActionResult Profile()
        {
            return this.View();
        }

        [HttpPost]
        public IActionResult Logout()
        {
            this.SignOut();

            return RedirectToAction("/home/index");
        }
    }
}