namespace Exam.Models
{
    using System.ComponentModel.DataAnnotations;

    public class ProductType
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
