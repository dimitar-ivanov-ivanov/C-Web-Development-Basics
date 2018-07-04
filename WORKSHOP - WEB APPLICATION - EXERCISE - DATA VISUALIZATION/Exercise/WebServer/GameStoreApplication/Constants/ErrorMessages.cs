namespace HTTPServer.GameStoreApplication.Constants
{
    public class ErrorMessages
    {
        public const string EmailIsNotUnique =
           "Email is taken!";

        public const string InvalidEmail =
             "Invalid Email.It should contain at least one \".\" and one \"@\".";

        public const string InvalidPassword =
          "Password should be at least 6 symbols long and contain at least 1 uppercase, 1 lowercase letter and 1 digit";

        public const string PasswordsDontMatch =
           "Both passwords must match.";

        public const string UserDoesntExist =
            "User doesn't exist.";

        public const string TitleIsInvalid =
            "Title must start with an uppercase letter and be between 3 and 100 symbols.";

        public const string PriceIsInvalid =
             "Price must be a positive number with precision up to 2 digits after floating point.";

        public const string SizeIsInvalid =
            "Size must be a positive number with precision up to 1 digit after floating point.";

        public const string TrailerIsInvalid =
            "Trailer id must be exactly 11 characters long.";

        public const string ThumbnailURLIsInvalid =
            "Thumbnail should start with https:// or http:// protocol.";

        public const string DescriptionIsInvalid =
            "Description must be at least 20 symbols.";

    }
}
