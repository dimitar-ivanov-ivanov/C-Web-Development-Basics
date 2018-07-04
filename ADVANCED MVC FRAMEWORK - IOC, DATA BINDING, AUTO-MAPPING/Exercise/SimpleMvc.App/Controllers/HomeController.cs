using SimpleMvc.Framework.Attributes.Methods;
using SimpleMvc.Framework.Controllers;
using SimpleMvc.Framework.Interfaces;

namespace SimpleMvc.App.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return this.View();
        }
    }
}
