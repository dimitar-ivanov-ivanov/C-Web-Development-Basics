namespace Exam.App.Models.BindingModels
{
    using SoftUni.WebServer.Common;
    using System.ComponentModel.DataAnnotations;

    public class UserLoginBindingModel
    {
        [Required]
        [MinLength(Constants.MinUsernameLength)]
        [MaxLength(Constants.MaxUsernameLength)]
        public string Username { get; set; }

        [Required]
        [MinLength(Constants.MinPasswordLength)]
        [MaxLength(Constants.MaxPasswordLength)]
        public string Password { get; set; }
    }
}
