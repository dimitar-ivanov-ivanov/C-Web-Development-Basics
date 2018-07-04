namespace MeTube.App.Models.BindingModels
{
    using SimpleMvc.Common;
    using System.ComponentModel.DataAnnotations;

    public class UserRegisterBindingModel
    {
        [Required]
        [MinLength(Constants.MinUsernameLength)]
        [MaxLength(Constants.MaxUsernameLength)]
        public string Username { get; set; }

        [Required]
        [MinLength(Constants.MinPasswordLength)]
        [MaxLength(Constants.MaxPasswordLength)]
        public string Password { get; set; }

        [Required]
        [Compare("Password")]
        [MinLength(Constants.MinPasswordLength)]
        [MaxLength(Constants.MaxPasswordLength)]
        public string ConfirmPassword { get; set; }

        [Required]
        [MinLength(Constants.MinEmailLength)]
        [MaxLength(Constants.MaxEmailLength)]
        public string Email { get; set; }
    }
}
