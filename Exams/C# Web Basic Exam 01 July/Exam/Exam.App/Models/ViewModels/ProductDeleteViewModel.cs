namespace Exam.App.Models.ViewModels
{
    using Exam.Models;
    using System;

    public class ProductDeleteViewModel
    {
        public string Name { get; set; }

        public decimal Price { get; set; }

        public string Description { get; set; }

        public string ProductType { get; set; }

        public static Func<Product, ProductDeleteViewModel> FromProduct =>
         product => new ProductDeleteViewModel()
         {
             Description = product.Description,
             Name = product.Name,
             Price = product.Price,
             ProductType = product.Type.Name
         };
    }
}
