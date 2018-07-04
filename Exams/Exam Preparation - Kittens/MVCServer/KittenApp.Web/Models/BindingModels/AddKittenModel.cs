namespace KittenApp.Web.Models.BindingModels
{
    using SimpleMvc.Common;
    using System.ComponentModel.DataAnnotations;
    
    public class AddKittenModel
    {
        [Required]
        [MinLength(Constants.MinNameLength)]
        [MaxLength(Constants.MaxNameLength)]
        public string Name { get; set; }

        [Required]
        [Range(Constants.MinAge, Constants.MaxAge)]
        public int Age { get; set; }

        [Required]
        public string Breed { get; set; }
    }
}