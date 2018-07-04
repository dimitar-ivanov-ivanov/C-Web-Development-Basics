namespace KittenApp.Web.Controllers
{
    using KittenApp.Data;
    using SimpleMvc.Framework.Controllers;
    using SimpleMvc.Framework.Interfaces;

    public abstract class BaseController : Controller
    {
        protected BaseController()
        {
            this.Model.Data["error"] = string.Empty;
            this.Context = new KittenContext();
        }

        protected KittenContext Context { get; set; }

        protected IActionResult RedirectHome()
        {
            return this.RedirectToAction("/");
        }

        public override void OnAuthentication()
        {
            if (this.User.IsAuthenticated)
            {
                this.Model.Data["userDisplay"] = "flex";
                this.Model.Data["guestDisplay"] = "none";
            }
            else
            {
                this.Model.Data["userDisplay"] = "none";
                this.Model.Data["guestDisplay"] = "flex";
            }
        }
    }
}
