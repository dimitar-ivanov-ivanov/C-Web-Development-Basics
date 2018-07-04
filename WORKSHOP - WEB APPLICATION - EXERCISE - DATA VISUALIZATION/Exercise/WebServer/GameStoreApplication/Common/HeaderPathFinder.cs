namespace HTTPServer.GameStoreApplication.Common
{
    using HTTPServer.GameStoreApplication.Constants;
    using HTTPServer.GameStoreApplication.Data;
    using HTTPServer.Server.Http;
    using HTTPServer.Server.Http.Contracts;
    using System.Linq;

    public class HeaderPathFinder
    {
        public string FindHeaderPath(IHttpRequest req)
        {
            //Depending on the role of the user the header may be different find the correct header
            //The headers are 3 - admin,user,guest

            var currentUserId = (int?)req.Session.Get(SessionStore.CurrentUserKey);

            if (currentUserId == null)
            {
                return Paths.GuestHeaderView;
            }

            using (var context = new GameStoreContext())
            {
                var currentUser = context.Users.FirstOrDefault(u => u.Id == currentUserId.Value);

                if (currentUser == null)
                {
                    return Paths.GuestHeaderView;
                }

                if (currentUser.IsAdmin)
                {
                    return Paths.AdminHeaderView;
                }

                return Paths.UserHeaderView;
            }
        }
    }
}
