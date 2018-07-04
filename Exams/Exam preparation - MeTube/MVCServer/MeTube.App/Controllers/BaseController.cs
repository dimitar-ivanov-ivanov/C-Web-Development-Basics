namespace MeTube.App.Controllers
{
    using MeTube.Data;
    using MeTube.Models;
    using Microsoft.EntityFrameworkCore;
    using SimpleMvc.Framework.Controllers;
    using SimpleMvc.Framework.Interfaces;
    using System.Linq;

    public abstract class BaseController : Controller
    {
        protected BaseController()
        {
            this.Model.Data["error"] = string.Empty;
            this.Context = new MeTubeContext();
        }

        protected MeTubeContext Context { get; set; }

        protected User DbUser { get; private set; }

        protected IActionResult RedirectHome()
        {
            return this.RedirectToAction("/home/index");
        }

        public override void OnAuthentication()
        {
            this.Model.Data["topMenu"] = this.User.IsAuthenticated ?
                @"<li class=""nav-item active col-md-3"">
                      <a class=""nav-link h5"" href=""/home/index"">Home</a>
                   </li>
                   <li class=""nav-item active col-md-3"">
                      <a class=""nav-link h5"" href=""/users/profile"">Profile</a>
                   </li>
                   <li class=""nav-item active col-md-3"">
                      <a class=""nav-link h5"" href=""/tubes/upload"">Upload</a>
                   </li>
                   <li class=""nav-item active col-md-3"">
                      <a class=""nav-link h5"" href=""/users/logout"">Logout</a>
                   </li>
                 " :
                 @"<li class=""nav-item active col-md-4"">
                      <a class=""nav-link h5"" href=""/home/index"">Home</a>
                   </li>
                   <li class=""nav-item active col-md-4"">
                      <a class=""nav-link h5"" href=""/users/login"">Login</a>
                   </li>
                   <li class=""nav-item active col-md-4"">
                      <a class=""nav-link h5"" href=""/users/register"">Register</a>
                   </li>
                 ";

            if (this.User.IsAuthenticated)
            {
                this.DbUser = this.Context.Users
                    .Include(u => u.Tubes)
                    .FirstOrDefault(u => u.Username == this.User.Name);
            }

            base.OnAuthentication();
        }
    }
}