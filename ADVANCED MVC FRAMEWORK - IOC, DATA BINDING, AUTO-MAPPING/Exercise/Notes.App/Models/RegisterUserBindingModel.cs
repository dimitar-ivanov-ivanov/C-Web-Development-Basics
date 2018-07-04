namespace Notes.App.Models
{
    using System.ComponentModel.DataAnnotations;

    public class RegisterUserBindingModel
    {
        [Required]
        [MinLength(5)]
        public string Username { get; set; }

        [Required]
        [MinLength(5)]
        public string Password { get; set; }

        [Required]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
    }
}