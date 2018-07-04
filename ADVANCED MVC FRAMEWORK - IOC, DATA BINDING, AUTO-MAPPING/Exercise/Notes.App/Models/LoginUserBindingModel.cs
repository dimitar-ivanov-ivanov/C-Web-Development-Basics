namespace Notes.App.Models
{
    using System.ComponentModel.DataAnnotations;

    public class LoginUserBindingModel
    {
        [Required]
        [MinLength(5)]
        public string Username { get; set; }

        [Required]
        [MinLength(5)]
        public string Password { get; set; }
    }
}