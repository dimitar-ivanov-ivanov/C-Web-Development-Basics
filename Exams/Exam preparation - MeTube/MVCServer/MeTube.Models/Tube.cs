namespace MeTube.Models
{
    using SimpleMvc.Common;
    using System.ComponentModel.DataAnnotations;

    public class Tube
    {
        public int Id { get; set; }

        [Required]
        [MinLength(Constants.MinTitleLength)]
        [MaxLength(Constants.MaxTitleLength)]
        public string Title { get; set; }

        [Required]
        [MinLength(Constants.MinAuthorLength)]
        [MaxLength(Constants.MaxAuthorLength)]
        public string Author { get; set; }

        [Required]
        [MaxLength(Constants.MaxDescriptionLength)]
        public string Description { get; set; }

        [Required]
        [MinLength(Constants.YoutubeIdLength)]
        [MaxLength(Constants.YoutubeIdLength)]
        public string YoutubeId { get; set; }

        public int Views { get; set; }

        [Required]
        public int UploaderId { get; set; }

        public User Uploader { get; set; }
    }
}
