namespace HTTPServer.GameStoreApplication.Models
{
    public class UserGame
    {
        public User User { get; set; }

        public Game Game { get; set; }

        public int UserId { get; set; }

        public int GameId { get; set; }
    }
}
