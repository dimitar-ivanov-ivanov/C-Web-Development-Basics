namespace Exam.Models
{
    using SoftUni.WebServer.Common;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class User
    {
        public User()
        {
            this.Orders = new List<Order>();
        }

        public int Id { get; set; }

        [Required]
        [MinLength(Constants.MinUsernameLength)]
        [MaxLength(Constants.MaxUsernameLength)]
        public string Username { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Required]
        [MinLength(Constants.MinFullNameLength)]
        [MaxLength(Constants.MaxFullNameLength)]
        public string FullName { get; set; }

        [Required]
        [MinLength(Constants.MinEmailLength)]
        [MaxLength(Constants.MaxEmailLength)]
        public string Email { get; set; }

        [Required]
        public int RoleId { get; set; }

        public Role Role { get; set; }

        public ICollection<Order> Orders { get; set; }
    }
}
