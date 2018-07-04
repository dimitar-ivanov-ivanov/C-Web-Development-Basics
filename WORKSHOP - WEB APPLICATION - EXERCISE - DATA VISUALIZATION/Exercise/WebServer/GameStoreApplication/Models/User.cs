namespace HTTPServer.GameStoreApplication.Models
{
    using HTTPServer.GameStoreApplication.Constants;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class User
    {
        public User()
        {
            this.Games = new HashSet<UserGame>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(ValidationConstraints.MaxEmailLength)]
        public string Email { get; set; }

        [Required]
        [MinLength(ValidationConstraints.MinPasswordLength)]
        [MaxLength(ValidationConstraints.MaxPasswordLength)]
        public string Password { get; set; }

        [Required]
        [MinLength(ValidationConstraints.MinFullNameLength)]
        [MaxLength(ValidationConstraints.MaxFullNameLength)]
        public string FullName { get; set; }

        public bool IsAdmin { get; set; }

        public ICollection<UserGame> Games { get; set; }
    }
}