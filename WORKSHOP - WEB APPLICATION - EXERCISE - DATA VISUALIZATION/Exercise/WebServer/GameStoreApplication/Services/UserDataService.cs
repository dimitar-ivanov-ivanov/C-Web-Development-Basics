namespace HTTPServer.GameStoreApplication.Services
{
    using HTTPServer.GameStoreApplication.Data;
    using HTTPServer.GameStoreApplication.Models;
    using HTTPServer.GameStoreApplication.Services.Contracts;
    using System.Linq;

    public class UserDataService : DataService, IUserDataService
    {
        public UserDataService(GameStoreContext gameStoreContext)
            : base(gameStoreContext)
        {
        }

        public void AddUser(User user)
        {
            this.Context.Users.Add(user);

            this.Context.SaveChanges();
        }

        public void AddUserGame(UserGame userGame, int userId)
        {
            var user = FindUser(userId);

            if(user != null)
            {
                user.Games.Add(userGame);

                this.Context.SaveChanges();
            }
        }

        public bool ExistsByEmail(string email) =>
         this.Context.Users.Any(u => u.Email == email);

        public User FindUser(int id) =>
            this.Context.Users.FirstOrDefault(u => u.Id == id);

        public bool UserExistById(int id) =>
              FindUser(id) != null;

        public User UserByEmailAndPassword(string email, string password) =>
         this.Context.Users.FirstOrDefault
            (u => u.Email == email && u.Password == password);
    }
}