namespace SimpleMvc.Common
{
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;

    public class PasswordUtilities
    {
        public string[] GenerateHash(string password)
        {
            var sh256 = new SHA256Managed();

            var bytes = Encoding.UTF8.GetBytes(password);

            return sh256.ComputeHash(bytes)
                .Select(x => x.ToString("x2"))
                .ToArray();
        }
    }
}
