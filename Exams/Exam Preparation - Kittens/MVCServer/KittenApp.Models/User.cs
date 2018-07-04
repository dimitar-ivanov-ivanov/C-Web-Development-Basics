namespace KittenApp.Models
{
    using SimpleMvc.Common;
    using System.ComponentModel.DataAnnotations;

    public class User
    {
        public int Id { get; set; }

        [Required]
        [MinLength(Constants.MinUsernameLength)]
        [MaxLength(Constants.MaxUsernameLength)]
        public string Username { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Required]
        [MinLength(Constants.MinEmailLenght)]
        [MaxLength(Constants.MaxEmailLength)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}