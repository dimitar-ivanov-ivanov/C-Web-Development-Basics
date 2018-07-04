namespace KittenApp.Models
{
    using SimpleMvc.Common;
    using System.ComponentModel.DataAnnotations;

    public class Kitten
    {
        public int Id { get; set; }

        [Required]
        [MinLength(Constants.MinNameLength)]
        [MaxLength(Constants.MaxNameLength)]
        public string Name { get; set; }

        [Required]
        [Range(Constants.MinAge, Constants.MaxAge)]
        public int Age { get; set; }

        [Required]
        public int BreedId { get; set; }

        public Breed Breed { get; set; }
    }
}