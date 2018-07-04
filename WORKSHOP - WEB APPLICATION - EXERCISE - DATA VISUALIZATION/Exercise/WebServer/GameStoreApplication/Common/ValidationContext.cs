namespace HTTPServer.GameStoreApplication.Common
{
    public class ValidationContext
    {
        public ValidationContext(bool valid, string message)
        {
            this.Valid = valid;
            this.Message = message;
        }

        public bool Valid { get; }

        public string Message { get; }
    }
}
