namespace SimpleMvc.Framework.Controllers
{
    using SimpleMvc.Framework.Interfaces;
    using SimpleMvc.Framework.Interfaces.Generic;
    using SimpleMvc.Framework.ViewEngine;
    using SimpleMvc.Framework.ViewEngine.Generic;
    using System.Runtime.CompilerServices;

    public abstract class Controller
    {
        protected IActionResult View([CallerMemberName]string caller = "")
        {
            string fullQualifiedName = this.GetFullyQualifiedName(caller, this.GetType().Name);

            return new ActionResult(fullQualifiedName);
        }

        protected IActionResult View(string controller, string action)
        {
            var fullQualifiedName = this.GetFullyQualifiedName(action, controller);

            return new ActionResult(fullQualifiedName);
        }

        protected IActionResult<T> View<T>(T model, [CallerMemberName]string caller = "")
        {
            var fullQualifiedName = this.GetFullyQualifiedName(caller, this.GetType().Name);

            return new ActionResult<T>(fullQualifiedName, model);
        }

        protected IActionResult<T> View<T>(string controller, string action, T model)
        {
            var fullQualifiedName = this.GetFullyQualifiedName(action, controller);

            return new ActionResult<T>(fullQualifiedName, model);
        }

        private string GetFullyQualifiedName(string caller, string controller = null)
        {
            var controllerName = controller != null ? controller : this.GetType().Name;

            controllerName = controllerName.Replace(MvcContext.Get.ControllerSuffix, string.Empty);

            var fullQualifiedName = string.Format(
                "{0}.{1}.{2}.{3}, {0}",
                MvcContext.Get.AssemblyName,
                MvcContext.Get.ViewsFolder,
                controllerName,
                caller);
            return fullQualifiedName;
        }
    }
}
