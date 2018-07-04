namespace SimpleMvc.Framework.Controllers
{
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using SimpleMvc.Framework.ActionResults;
    using SimpleMvc.Framework.Interfaces;
    using SimpleMvc.Framework.Models;
    using SimpleMvc.Framework.Security;
    using SimpleMvc.Framework.Views;
    using WebServer.Http;
    using WebServer.Http.Contracts;

    public abstract class Controller
    {
        protected Controller()
        {
            this.Model = new ViewModel();
            this.User = new Authentication();
        }

        protected ViewModel Model { get; }

        public IHttpRequest Request { get; set; }

        protected Authentication User { get; private set; }

        protected IViewable View([CallerMemberName] string caller = "")
        {
            this.InitializeViewModelData();

            var controllerName = this.GetType()
                .Name
                .Replace(MvcContext.Get.ControllerSuffix, string.Empty);

            var fullQualifiedName = string.Format(
                "{0}\\{1}\\{2}",
                MvcContext.Get.ViewsFolder,
                controllerName,
                caller);

            var view = new View(fullQualifiedName, this.Model.Data);

            return new ViewResult(view);
        }

        protected IRedirectable RedirectToAction(string redirectUrl)
        {
            return new RedirectResult(redirectUrl);
        }

        protected bool IsValidModel(object bindingModel)
        {
            foreach (var property in bindingModel.GetType().GetProperties())
            {
                var validationAttributes = property
                    .GetCustomAttributes(true)
                    .Where(a => a is ValidationAttribute);

                foreach (ValidationAttribute attribute in validationAttributes)
                {
                    var propertyValue = property.GetValue(bindingModel);

                    var validationResult = attribute.GetValidationResult
                        (propertyValue, new ValidationContext(bindingModel));

                    if (validationResult != ValidationResult.Success)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        protected internal void InitializeUser()
        {
            var user = this.Request.Session.Get<string>(SessionStore.CurrentUserKey);

            if (user != null)
            {
                this.User = new Authentication(user);
            }
            else
            {
                this.User = new Authentication();
            }
        }

        private void InitializeViewModelData()
        {
            this.Model["displayType"] = this.User.IsAuthenticated ? "block" : "none";
        }

        protected void SignIn(string username)
        {
            this.Request.Session.Add(SessionStore.CurrentUserKey, username);
            InitializeUser();
        }

        protected void SignOut()
        {
            this.Request.Session.Clear();
            InitializeUser();
        }
    }
}