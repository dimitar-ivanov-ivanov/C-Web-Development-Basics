namespace HTTPServer.GameStoreApplication.Models
{
    using HTTPServer.GameStoreApplication.Constants;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Game
    {
        public Game()
        {
            this.Users = new HashSet<UserGame>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(ValidationConstraints.MinTitleLength)]
        [MaxLength(ValidationConstraints.MaxTitleLength)]
        public string Title { get; set; }

        [Required]
        public decimal Price { get; set; }

        //In GB
        [Required]
        public decimal Size { get; set; }

        [Required]
        [StringLength(ValidationConstraints.TrailerIdLength)]
        public string TrailerId { get; set; }

        public string ThumbnailURL { get; set; }

        [Required]
        [MinLength(ValidationConstraints.DescriptionLength)]
        public string Description { get; set; }

        [Required]
        public DateTime ReleaseDate { get; set; }

        public ICollection<UserGame> Users{ get; set; }
    }
}
