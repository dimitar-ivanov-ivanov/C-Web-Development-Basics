namespace KittenApp.Web.Models.BindingModels
{
    using SimpleMvc.Common;
    using System.ComponentModel.DataAnnotations;

    public class UserLoginModel
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
