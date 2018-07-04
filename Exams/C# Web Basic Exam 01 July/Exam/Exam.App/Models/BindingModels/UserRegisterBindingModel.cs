namespace Exam.App.Models.BindingModels
{
    using SoftUni.WebServer.Common;
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
        [MinLength(Constants.MinPasswordLength)]
        [MaxLength(Constants.MaxPasswordLength)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        [Required]
        [MinLength(Constants.MinFullNameLength)]
        [MaxLength(Constants.MaxFullNameLength)]
        public string FullName { get; set; }

        [Required]
        [MinLength(Constants.MinEmailLength)]
        [MaxLength(Constants.MaxEmailLength)]
        public string Email { get; set; }
    }
}
