namespace HTTPServer.GameStoreApplication.Constants
{
    public class ValidationConstraints
    {
        public const int MaxEmailLength = 30;

        public const int MinFullNameLength = 5;

        public const int MaxFullNameLength = 70;

        public const int MinPasswordLength = 6;

        public const int MaxPasswordLength = 50;

        public const int MinTitleLength = 3;

        public const int MaxTitleLength = 100;

        public const int PricePrecision = 2;

        public const int SizePrecision = 1;

        public const int PriceAndSizeMin = 0;

        public const int TrailerIdLength = 11;

        public readonly static string[] ThumbnailUrls = new[] { "http://", "https://", null };

        public const int DescriptionLength = 20;

        public const string ReleaseDateFormat = "yyyy-MM-dd";
    }
}
