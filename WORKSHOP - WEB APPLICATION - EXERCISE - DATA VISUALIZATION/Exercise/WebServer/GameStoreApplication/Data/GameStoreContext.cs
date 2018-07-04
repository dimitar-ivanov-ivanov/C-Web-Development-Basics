namespace HTTPServer.GameStoreApplication.Data
{
    using HTTPServer.GameStoreApplication.Models;
    using Microsoft.EntityFrameworkCore;

    public class GameStoreContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Game> Games { get; set; }

        public DbSet<UserGame> UserGames { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Configuration.ConnectionString);
            }
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<UserGame>()
                .HasKey(ug => new { ug.UserId, ug.GameId });

            modelBuilder.Entity<UserGame>()
                .HasOne(ug => ug.Game)
                .WithMany(g => g.Users)
                .HasForeignKey(ug => ug.GameId);

            modelBuilder.Entity<UserGame>()
              .HasOne(ug => ug.User)
              .WithMany(u => u.Games)
              .HasForeignKey(ug => ug.UserId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
