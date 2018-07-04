namespace HTTPServer.GameStoreApplication.Controllers
{
    using HTTPServer.GameStoreApplication.Common;
    using HTTPServer.GameStoreApplication.Constants;
    using HTTPServer.GameStoreApplication.Models;
    using HTTPServer.GameStoreApplication.Services.Contracts;
    using HTTPServer.Server.Http;
    using HTTPServer.Server.Http.Contracts;

    public class AccountController : BaseController
    {
        public AccountController(IHttpRequest request, IUserDataService userDataService, IGameDataService gameDataService, HeaderPathFinder pathFinder)
            : base(request, userDataService, gameDataService, pathFinder)
        {
        }

        public IHttpResponse RegisterGet()
        {
            this.ViewData["error"] = string.Empty;
            this.ViewData["errorDisplay"] = "none";

            return this.FileViewResponse(Paths.RegisterView, this.PathFinder.FindHeaderPath(this.Request));
        }

        public IHttpResponse RegisterPost()
        {
            //Create model
            var viewModel = GetRegisterViewModel();

            if (this.Request == null)
            {
                return this.RedirectResponse(Paths.RegisterPath);
            }

            //Validate model and put error messages accordingly
            var isValid = this.ValidateRegisterViewModel(viewModel);

            if (!isValid.Valid)
            {
                this.ViewData["error"] = isValid.Message;
                this.ViewData["errorDisplay"] = "flex";

                return this.FileViewResponse(Paths.RegisterView, this.PathFinder.FindHeaderPath(this.Request));
            }

            //Add user to database
            this.UserDataService.AddUser(new User()
            {
                Email = viewModel.Email,
                FullName = viewModel.FullName,
                Password = viewModel.Password
            });

            //Redirect to login path for the user to login after registration
            return this.RedirectResponse(Paths.LoginPath);
        }

        public IHttpResponse LoginGet()
        {
            this.ViewData["error"] = string.Empty;
            this.ViewData["errorDisplay"] = "none";

            return this.FileViewResponse(Paths.LoginView, this.PathFinder.FindHeaderPath(this.Request));
        }

        public IHttpResponse LoginPost()
        {
            //Create model
            var viewModel = GetLoginViewModel();

            if (this.Request == null)
            {
                return this.RedirectResponse(Paths.LoginPath);
            }

            //Validate model
            var isValid = this.ValidateLoginViewModel(viewModel);

            if (!isValid.Valid)
            {
                this.ViewData["error"] = isValid.Message;
                this.ViewData["errorDisplay"] = "flex";

                return this.FileViewResponse(Paths.LoginView, this.PathFinder.FindHeaderPath(this.Request));
            }

            //GetUser
            var currentUser = this.UserDataService.UserByEmailAndPassword(viewModel.Email, viewModel.Password);

            //Add user to session
            this.Request.Session.Add(SessionStore.CurrentUserKey, currentUser.Id);

            //Redirect to home after login
            return this.RedirectResponse(Paths.HomePath);
        }

        public IHttpResponse Logout()
        {
            //Remove the logged in user
            //DONT CLEAR THE SESSION - it will remove the shopping cart
            this.Request.Session.Remove(SessionStore.CurrentUserKey);

            //Redirect to home after logout
            return this.RedirectResponse(Paths.HomePath);
        }
    }
}