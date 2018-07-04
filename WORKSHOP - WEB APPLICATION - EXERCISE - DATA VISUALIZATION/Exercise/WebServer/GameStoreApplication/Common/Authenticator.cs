namespace HTTPServer.GameStoreApplication.Common
{
    using HTTPServer.GameStoreApplication.Data;
    using HTTPServer.Server.Http;
    using HTTPServer.Server.Http.Contracts;
    using System.Linq;

    public class Authenticator
    {
        public Authenticator(IHttpRequest req)
        {
            this.Request = req;
        }

        public IHttpRequest Request { get; }

        public bool UserIsAdmin()
        {
            var userId = (int?)this.Request.Session.Get(SessionStore.CurrentUserKey);

            if (userId == null)
            {
                return false;
            }

            using (var context = new GameStoreContext())
            {
                var user = context.Users.FirstOrDefault(u => u.Id == userId.Value);

                if (user == null)
                {
                    return false;
                }

                return user.IsAdmin;
            }
        }
    }
}
