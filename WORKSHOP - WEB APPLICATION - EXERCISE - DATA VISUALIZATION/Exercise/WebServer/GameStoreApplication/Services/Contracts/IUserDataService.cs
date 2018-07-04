namespace HTTPServer.GameStoreApplication.Services.Contracts
{
    using HTTPServer.GameStoreApplication.Models;

    public interface IUserDataService : IDataService
    {
        void AddUser(User user);

        void AddUserGame(UserGame userGame, int userId);

        User FindUser(int id);

        bool UserExistById(int id);

        bool ExistsByEmail(string email);

        User UserByEmailAndPassword(string email, string password);
    }
}