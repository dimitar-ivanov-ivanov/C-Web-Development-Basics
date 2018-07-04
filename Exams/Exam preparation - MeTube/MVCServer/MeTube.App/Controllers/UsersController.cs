namespace MeTube.App.Controllers
{
    using MeTube.App.Attributes;
    using MeTube.App.Models.BindingModels;
    using MeTube.App.Models.ViewModels;
    using MeTube.Models;
    using SimpleMvc.Common;
    using SimpleMvc.Framework.Attributes.Methods;
    using SimpleMvc.Framework.Interfaces;
    using System.Linq;
    using System.Text;

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
        public IActionResult Register(UserRegisterBindingModel model)
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

            var passwordHash = new PasswordUtilities().GenerateHash(model.Password);

            var user = new User()
            {
                Email = model.Email,
                PasswordHash = string.Join("", passwordHash),
                Username = model.Username
            };

            this.Context.Users.Add(user);
            this.Context.SaveChanges();
            this.SignIn(user.Username, user.Id);

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
        public IActionResult Login(UserLoginBindingModel model)
        {
            if (this.User.IsAuthenticated)
            {
                return this.RedirectHome();
            }

            if (!this.IsValidModel(model))
            {
                this.Model.Data["error"] = ErrorMessages.InvalidLoginModel;
            }

            var passwordHash = string.Join("", new PasswordUtilities().GenerateHash(model.Password));

            var user = this.Context.Users
                .FirstOrDefault(u => u.Username == model.Username &&
                                u.PasswordHash == passwordHash);

            if (user == null)
            {
                this.Model.Data["error"] = ErrorMessages.InvalidLoginModel;
                return this.View();
            }

            this.SignIn(user.Username, user.Id);

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

        [HttpGet]
        [AuthrorizedLogin]
        public IActionResult Profile()
        {
            var tubes = this.Context.Tubes
                .Where(t => t.UploaderId == this.DbUser.Id)
                .Select(TubeProfileViewModel.FromTube)
                .ToList();

            this.Model.Data["username"] = this.DbUser.Username;
            this.Model.Data["email"] = this.DbUser.Email;

            var tubesResult = new StringBuilder();

            for (int i = 0; i < tubes.Count; i++)
            {
                tubesResult.AppendLine
                    (
                     $@"<tr>
                          <td>{i + 1}</td>
                          <td>{tubes[i].Title}</td>
                          <td>{tubes[i].Author}</td>
                          <td><a href=""/tubes/details?id={tubes[i].Id}"">Details</a></td>
                        </tr>
                      "
                    );
            }

            this.Model.Data["tubes"] = tubesResult.ToString();

            return this.View();
        }
    }
}