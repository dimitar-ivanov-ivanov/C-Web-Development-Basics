namespace KittenApp.Web.Models.BindingModels
{
    using SimpleMvc.Common;
    using System.ComponentModel.DataAnnotations;

    public class UserRegisterModel
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
        [MinLength(Constants.MinEmailLenght)]
        [MaxLength(Constants.MaxEmailLength)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}
