namespace Exam.App.Controllers
{
    using SoftUni.WebServer.Mvc.Attributes.HttpMethods;
    using SoftUni.WebServer.Mvc.Attributes.Security;
    using SoftUni.WebServer.Mvc.Interfaces;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;
    using Exam.App.Models.BindingModels;
    using Exam.Models;
    using Exam.App.Models.ViewModels;

    public class ProductsController : BaseController
    {
        [HttpGet]
        [Authorize]
        public IActionResult Create()
        {
            if (!IsAdmin())
            {
                return this.RedirectToHome();
            }

            return this.View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult Create(ProductCreateBindingModel model)
        {
            if (!IsAdmin())
            {
                return this.RedirectToHome();
            }

            if (!this.IsValidModel(model))
            {
                return this.BuildErrorView();
            }

            var product = new Product()
            {
                Description = model.Description,
                Name = model.Name,
                Price = model.Price,
                Type = this.Context.ProductTypes.FirstOrDefault(pt => pt.Name == model.ProductType)
            };

            this.Context.Products.Add(product);
            this.Context.SaveChanges();

            return this.RedirectToHome();
        }

        [HttpGet]
        [Authorize]
        public IActionResult Edit(int id)
        {
            if (!IsAdmin())
            {
                return this.RedirectToHome();
            }

            var product = this.Context
                .Products
                .Include(p => p.Type)
                .FirstOrDefault(p => p.Id == id);

            if (product == null)
            {
                return this.RedirectToHome();
            }

            var model = new[] { product }
            .Select(ProductEditViewModel.FromProduct)
            .First();

            switch (model.ProductType)
            {
                case "Food":
                    this.ViewData["foodChecked"] = "checked";
                    break;
                case "Domestic":
                    this.ViewData["domesticChecked"] = "checked";
                    break;
                case "Health":
                    this.ViewData["healthChecked"] = "checked";
                    break;
                case "Cosmetic":
                    this.ViewData["cosmeticChecked"] = "checked";
                    break;
                case "Other":
                    this.ViewData["otherChecked"] = "checked";
                    break;
                default:
                    break;
            }

            this.ViewData["name"] = model.Name;
            this.ViewData["price"] = model.Price.ToString();
            this.ViewData["description"] = model.Description;
            this.ViewData["id"] = product.Id.ToString();

            return this.View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult Edit(int id, ProductsEditBindingModel model)
        {
            if (!IsAdmin())
            {
                return this.RedirectToHome();
            }

            var product = this.Context
                .Products
                .Include(p => p.Type)
                .FirstOrDefault(p => p.Id == id);

            if (product == null)
            {
                return this.RedirectToHome();
            }

            product.Name = model.Name;
            product.Price = model.Price;
            product.Description = model.Description;
            product.Type = this.Context.ProductTypes.FirstOrDefault(pt => pt.Name == model.ProductType);

            this.Context.SaveChanges();

            return this.RedirectToHome();
        }

        [HttpGet]
        [Authorize]
        public IActionResult Delete(int id)
        {
            if (!IsAdmin())
            {
                return this.RedirectToHome();
            }

            var product = this.Context
                .Products
                .Include(p => p.Type)
                .FirstOrDefault(p => p.Id == id);

            if (product == null)
            {
                return this.RedirectToHome();
            }

            var model = new[] { product }
            .Select(ProductEditViewModel.FromProduct)
            .First();

            switch (model.ProductType)
            {
                case "Food":
                    this.ViewData["foodChecked"] = "checked";
                    break;
                case "Domestic":
                    this.ViewData["domesticChecked"] = "checked";
                    break;
                case "Health":
                    this.ViewData["healthChecked"] = "checked";
                    break;
                case "Cosmetic":
                    this.ViewData["cosmeticChecked"] = "checked";
                    break;
                case "Other":
                    this.ViewData["otherChecked"] = "checked";
                    break;
                default:
                    break;
            }

            this.ViewData["name"] = model.Name;
            this.ViewData["price"] = model.Price.ToString();
            this.ViewData["description"] = model.Description;
            this.ViewData["id"] = product.Id.ToString();

            return this.View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult Delete(int id, ProductDeleteBindingModel model)
        {
            if (!IsAdmin())
            {
                return this.RedirectToHome();
            }

            var product = this.Context
                .Products
                .Include(p => p.Type)
                .FirstOrDefault(p => p.Id == id);

            if (product == null)
            {
                return this.RedirectToHome();
            }

            this.Context.Products.Remove(product);
            this.Context.SaveChanges();

            return this.RedirectToHome();
        }

        [HttpGet]
        [Authorize]
        public IActionResult Details(int id)
        {
            if (!IsAdmin())
            {
                return this.RedirectToHome();
            }

            var product = this.Context
                 .Products
                 .Include(p => p.Type)
                 .FirstOrDefault(p => p.Id == id);

            if (product == null)
            {
                return this.RedirectToHome();
            }

            var model = new[] { product }
            .Select(ProductDetailsViewModel.FromProduct)
            .First();

            this.ViewData["name"] = model.Name;
            this.ViewData["type"] = model.ProductType;
            this.ViewData["price"] = model.Price.ToString();
            this.ViewData["description"] = model.Description;
            this.ViewData["id"] = model.Id.ToString();

            return this.View();
        }
    }
}
