namespace HTTPServer.GameStoreApplication.Controllers
{
    using HTTPServer.GameStoreApplication.Common;
    using HTTPServer.GameStoreApplication.Constants;
    using HTTPServer.GameStoreApplication.Infrastructure;
    using HTTPServer.GameStoreApplication.Services.Contracts;
    using HTTPServer.GameStoreApplication.ViewModels;
    using HTTPServer.Server.Http.Contracts;
    using System;
    using System.Globalization;
    using System.Linq;

    public abstract class BaseController : Controller
    {
        protected const string Success = "Success";

        protected BaseController(IHttpRequest request, IUserDataService userDataService, IGameDataService gameDataService, HeaderPathFinder pathFinder)
        {
            this.PathFinder = pathFinder;
            this.Request = request;
            this.UserDataService = userDataService;
            this.GameDataService = gameDataService;

            //Add the shopping cart immediately - so that no matter where we enter the site,it will always be present
            InitializeShoppingCart();
        }

        protected HeaderPathFinder PathFinder { get; private set; }

        protected IHttpRequest Request { get; private set; }

        protected IUserDataService UserDataService { get; private set; }

        protected IGameDataService GameDataService { get; private set; }

        protected override string AlternativePath => "GameStoreApplication";
        
        protected LoginViewModel GetLoginViewModel()
        {
            return new LoginViewModel()
            {
                Email = this.Request.FormData["email"],
                Password = this.Request.FormData["password"]
            };
        }

        protected RegisterViewModel GetRegisterViewModel()
        {
            return new RegisterViewModel()
            {
                Email = this.Request.FormData["email"],
                Password = this.Request.FormData["password"],
                ConfirmPassword = this.Request.FormData["confirmPassword"],
                FullName = this.Request.FormData["fullName"]
            };
        }

        protected GameViewModel GetGameViewModel()
        {
            var date = this.Request.FormData["release-date"];

            var viewModel = new GameViewModel()
            {
                Description = this.Request.FormData["description"],
                Price = decimal.Parse(this.Request.FormData["price"]),
                Size = decimal.Parse(this.Request.FormData["size"]),
                ReleaseDate = DateTime.ParseExact(this.Request.FormData["release-date"], ValidationConstraints.ReleaseDateFormat, CultureInfo.InvariantCulture),
                Title = this.Request.FormData["title"],
                TrailerId = this.Request.FormData["video-id"]
            };

            //Thumbnail url is not required
            if (this.Request.FormData.ContainsKey("thumbnail"))
            {
                viewModel.ThumbnailURL = this.Request.FormData["thumbnail"];
            }

            return viewModel;
        }

        protected ValidationContext ValidateRegisterViewModel(RegisterViewModel viewModel)
        {
            var userEmail = viewModel.Email;
            var userPassword = viewModel.Password;
            var confirmPassword = viewModel.ConfirmPassword;

            //Validate email
            if (GameStoreValidator.IsNullOrEmpty(userEmail))
            {
                return new ValidationContext(false, ErrorMessages.InvalidEmail);
            }

            var isValidEmail = userEmail.Contains("@") &&
                               userEmail.Contains(".");

            if (!isValidEmail)
            {
                return new ValidationContext(false, ErrorMessages.InvalidEmail);
            }

            //Check for existing email
            if (this.UserDataService.ExistsByEmail(userEmail))
            {
                return new ValidationContext(false, ErrorMessages.EmailIsNotUnique);
            }

            //Validate password
            if (GameStoreValidator.IsNullOrEmpty(userPassword) ||
                GameStoreValidator.IsNullOrEmpty(confirmPassword))
            {
                return new ValidationContext(false, ErrorMessages.InvalidPassword);
            }

            var isValidPassword = GameStoreValidator.IsEqualOrLongerThan(userPassword, ValidationConstraints.MinPasswordLength) &&
                userPassword.Any(up => char.IsDigit(up)) &&
                userPassword.Any(up => char.IsLower(up)) &&
                userPassword.Any(up => char.IsUpper(up));

            //Check if passwords are invalid and don't match, so that we can combine the error messages
            if (!isValidPassword && !GameStoreValidator.AreEqual(userPassword, confirmPassword))
            {
                return new ValidationContext(false, ErrorMessages.InvalidPassword + "<br/>" +
                    ErrorMessages.PasswordsDontMatch);
            }
 
            if (!isValidPassword)
            {
                return new ValidationContext(false, ErrorMessages.InvalidPassword);
            }

            //Check if both passwords are equal
            if (!GameStoreValidator.AreEqual(userPassword, confirmPassword))
            {
                return new ValidationContext(false, ErrorMessages.PasswordsDontMatch);
            }

            return new ValidationContext(true, Success);
        }

        protected ValidationContext ValidateLoginViewModel(LoginViewModel viewModel)
        {
            var userEmail = viewModel.Email;
            var userPassword = viewModel.Password;

            var userExists = this.UserDataService.UserByEmailAndPassword(userEmail, userPassword);
            
            //Check if user exists
            if (userExists == null)
            {
                return new ValidationContext(false, ErrorMessages.UserDoesntExist);
            }

            return new ValidationContext(true, Success);
        }

        protected ValidationContext ValidateGameViewModel(GameViewModel viewModel)
        {
            var title = viewModel.Title;
            var price = viewModel.Price;
            var size = viewModel.Size;
            var trailerId = viewModel.TrailerId;
            var thumbnailUrl = viewModel.ThumbnailURL;
            var description = viewModel.Description;

            //Validate title
            if (!(GameStoreValidator.IsEqualOrBiggerThan(title[0], 'A') &&
                  GameStoreValidator.IsEqualOrLesserThan(title[0], 'Z') &&
                  GameStoreValidator.IsEqualOrBiggerThan(title.Length, ValidationConstraints.MinTitleLength) &&
                  GameStoreValidator.IsEqualOrLesserThan(title.Length, ValidationConstraints.MaxTitleLength)))
            {
                return new ValidationContext(false, ErrorMessages.TitleIsInvalid);
            }

            //Validate description
            if (!GameStoreValidator.IsEqualOrBiggerThan(description.Length, ValidationConstraints.DescriptionLength))
            {
                return new ValidationContext(false, ErrorMessages.DescriptionIsInvalid);
            }

            //Validate price
            if (!(GameStoreValidator.IsPositive(price) &&
                  GameStoreValidator.CheckStringPrecision(price, ValidationConstraints.PricePrecision)))
            {
                return new ValidationContext(false, ErrorMessages.PriceIsInvalid);
            }

            //Validate size
            if (!(GameStoreValidator.IsPositive(size) &&
                  GameStoreValidator.CheckStringPrecision(size, ValidationConstraints.SizePrecision)))
            {
                return new ValidationContext(false, ErrorMessages.SizeIsInvalid);
            }

            //Validate trailerId
            if (!GameStoreValidator.StringLengthIsEqualTo(trailerId, ValidationConstraints.TrailerIdLength))
            {
                return new ValidationContext(false, ErrorMessages.TrailerIsInvalid);
            }

            //Check if thumbnail is present and if yes validate it
            if (thumbnailUrl != null && !GameStoreValidator.StringStartsWithAny(thumbnailUrl, ValidationConstraints.ThumbnailUrls))
            {
                return new ValidationContext(false, ErrorMessages.ThumbnailURLIsInvalid);
            }

            return new ValidationContext(true, Success);
        }

        private void InitializeShoppingCart()
        {
            if (!this.Request.Session.Contains(ShoppingCart.SessionKey))
            {
                this.Request.Session.Add(ShoppingCart.SessionKey, new ShoppingCart());
            }
        }
    }
}
