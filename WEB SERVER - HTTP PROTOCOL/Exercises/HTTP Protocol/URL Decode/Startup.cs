namespace URL_Decode
{
    using System;
    using System.Net;

    public class Startup
    {
        public static void Main(string[] args)
        {
            Execute();
        }

        private static void Execute()
        {
            var inputUrl = Console.ReadLine();
            var decodedUrl = WebUtility.UrlDecode(inputUrl);
            Console.WriteLine(decodedUrl);
        }
    }
}
