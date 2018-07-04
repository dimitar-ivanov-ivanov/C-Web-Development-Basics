namespace KittenApp.Web.Controllers
{
    using SimpleMvc.Framework.Attributes.Methods;
    using SimpleMvc.Framework.Interfaces;

    public class HomeController : BaseController
    {
        [HttpGet]
        public IActionResult Index()
        {
            if (this.User.IsAuthenticated)
            {
                this.Model.Data["userDisplay"] = "normal";
                this.Model.Data["username"] = this.User.Name;
                this.Model.Data["guestDisplay"] = "none";
            }
            else
            {
                this.Model.Data["userDisplay"] = "none";
                this.Model.Data["username"] = string.Empty;
                this.Model.Data["guestDisplay"] = "normal";
            }

            return this.View();
        }
    }
}
