namespace Exam.App.Models.BindingModels
{
    using SoftUni.WebServer.Common;
    using System.ComponentModel.DataAnnotations;

    public class ProductCreateBindingModel
    {
        [Required]
        [MinLength(Constants.MinProductNameLength)]
        [MaxLength(Constants.MaxProductNameLength)]
        public string Name { get; set; }

        [Required]
        [Range(Constants.MinPrice, Constants.MaxPrice)]
        public decimal Price { get; set; }

        [Required]
        [MinLength(Constants.MinDescriptionLength)]
        [MaxLength(Constants.MaxDescriptionLength)]
        public string Description { get; set; }

        [Required]
        public string ProductType { get; set; }
    }
}
