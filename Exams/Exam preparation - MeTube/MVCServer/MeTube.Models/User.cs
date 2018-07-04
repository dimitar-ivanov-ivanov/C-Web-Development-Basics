namespace MeTube.Models
{
    using SimpleMvc.Common;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class User
    {
        public User()
        {
            this.Tubes = new List<Tube>();
        }

        public int Id { get; set; }

        [Required]
        [MinLength(Constants.MinUsernameLength)]
        [MaxLength(Constants.MaxUsernameLength)]
        public string Username { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Required]
        [MinLength(Constants.MinEmailLength)]
        [MaxLength(Constants.MaxEmailLength)]
        public string Email { get; set; }

        public ICollection<Tube> Tubes { get; set; }
    }
}
