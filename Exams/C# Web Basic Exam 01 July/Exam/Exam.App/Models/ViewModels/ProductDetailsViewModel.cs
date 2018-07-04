namespace Exam.App.Models.ViewModels
{
    using Exam.Models;
    using System;

    public class ProductDetailsViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public string Description { get; set; }

        public string ProductType { get; set; }

        public static Func<Product, ProductDetailsViewModel> FromProduct =>
         product => new ProductDetailsViewModel()
         {
             Id = product.Id,
             Description = product.Description,
             Name = product.Name,
             Price = product.Price,
             ProductType = product.Type.Name
         };
    }
}
