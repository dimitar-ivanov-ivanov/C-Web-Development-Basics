namespace MeTube.App.Models.BindingModels
{
    using SimpleMvc.Common;
    using System.ComponentModel.DataAnnotations;

    public class TubeUploadBindingModel
    {
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
        public string YoutubeLink { get; set; }
    }
}
