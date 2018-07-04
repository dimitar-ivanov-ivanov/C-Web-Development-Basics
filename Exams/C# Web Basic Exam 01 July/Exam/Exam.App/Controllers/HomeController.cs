namespace Exam.App.Controllers
{
    using SoftUni.WebServer.Mvc.Interfaces;

    public class HomeController : BaseController
    {
        public IActionResult Index()
        {
            return this.View();
        }
    }
}
