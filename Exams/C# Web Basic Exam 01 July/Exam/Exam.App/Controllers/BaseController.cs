namespace Exam.App.Controllers
{
    using Exam.Data;
    using Microsoft.EntityFrameworkCore;
    using SoftUni.WebServer.Mvc.Controllers;
    using SoftUni.WebServer.Mvc.Interfaces;
    using System.Linq;
    using System.Runtime.CompilerServices;

    public abstract class BaseController : Controller
    {
        protected BaseController()
        {
            this.Context = new ExamContext();
            this.ViewData["error"] = string.Empty;
            this.ViewData["foodChecked"] = string.Empty;
            this.ViewData["domesticChecked"] = string.Empty;
            this.ViewData["healthChecked"] = string.Empty;
            this.ViewData["cosmeticChecked"] = string.Empty;
            this.ViewData["otherChecked"] = string.Empty;
        }

        public ExamContext Context { get; set; }

        protected bool IsAdmin()
        {
            var user = this.Context
                .Users
                .Include(u => u.Role)
                .FirstOrDefault(u => u.Username == this.User.Name);

            return user.Role.Name == "Admin";
        }

        protected IActionResult RedirectToHome()
        {
            return this.RedirectToAction("/");
        }

        protected virtual IActionResult BuildErrorView([CallerMemberName]string callerName = "")
        {
            this.ViewData["error"] = "You have errors in your form.";
            return this.View(callerName);
        }

        public override void OnAuthentication()
        {
            this.ViewData["topMenu"] = this.User.IsAuthenticated ?
                @"<ul class=""navbar-nav right-side"">
                    <li class=""nav-item"">
                        <a class=""nav-link nav-link-white"" href=""/"">Home</a>
                    </li>
                </ul>
                <ul class=""navbar-nav left-side"">
                    <li class=""nav-item"">
                        <a class=""nav-link nav-link-white"" href=""/users/logout"">Logout</a>
                    </li>
                </ul>"
                 :
               @"<ul class=""navbar-nav"">
                     <li class=""nav-item"">
                        <a class=""nav-link nav-link-white"" href=""/"">Home</a>
                    </li>
                    <li class=""nav-item"">
                        <a class=""nav-link nav-link-white"" href=""/users/login"">Login</a>
                    </li>
                    <li class=""nav-item"">
                        <a class=""nav-link nav-link-white"" href=""/users/register"">Register</a>
                    </li>
                </ul>";

            base.OnAuthentication();
        }
    }
}
