namespace Exam.Models
{
    using SoftUni.WebServer.Common;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Product
    {
        public Product()
        {
            this.Orders = new List<Order>();
        }

        public int Id { get; set; }

        [Required]
        [MinLength(Constants.MinProductNameLength)]
        [MaxLength(Constants.MaxProductNameLength)]
        public string Name { get; set; }

        [Required]
        [Range(Constants.MinPrice, Constants.MaxPrice)]
        public decimal Price { get; set; }

        [Required]
        [MinLength(Constants.MinDescriptionLength)]
        [MaxLength(Constants.MaxDescriptionLength)]
        public string Description { get; set; }

        [Required]
        public int TypeId { get; set; }

        public ProductType Type { get; set; }

        public ICollection<Order> Orders { get; set; }
    }
}
