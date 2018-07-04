namespace HTTPServer.ByTheCakeApplication.Controllers
{
    using Data;
    using HTTPServer.Server.Http;
    using HTTPServer.Server.Http.Response;
    using Infrastructure;
    using Models;
    using Server.Http.Contracts;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class CakesController : Controller
    {
        public IHttpResponse Add()
        {
            this.ViewData["showResult"] = "none";

            return this.FileViewResponse(@"cakes\add");
        }

        public IHttpResponse Add(string name, string price, string imageUrl)
        {
            var product = new Product()
            {
                Name = name,
                Price = decimal.Parse(price),
                ImageUrl = imageUrl
            };

            using (var context = new ByTheCakeContext())
            {
                context.Products.Add(product);
                context.SaveChanges();
            }

            this.ViewData["name"] = name;
            this.ViewData["price"] = price;
            this.ViewData["imageUrl"] = imageUrl;
            this.ViewData["showResult"] = "block";

            return this.FileViewResponse(@"cakes\add");
        }

        public IHttpResponse Search(IHttpRequest req)
        {
            const string searchTermKey = "searchTerm";

            var urlParameters = req.UrlParameters;

            this.ViewData["results"] = string.Empty;
            this.ViewData["searchTerm"] = string.Empty;

            if (urlParameters.ContainsKey(searchTermKey))
            {
                var searchTerm = urlParameters[searchTermKey];

                this.ViewData["searchTerm"] = searchTerm;

                IEnumerable<string> cakeResults = null;

                using (var context = new ByTheCakeContext())
                {
                    var cakes = context.Products.AsQueryable();

                    if (searchTerm != "*")
                    {
                        cakes = context.Products
                            .Where(c => c.Name.ToLower().Contains(searchTerm.ToLower()));
                    }

                    cakeResults = cakes.Select
                        (c => $@"<div>
                                   <a href =""/cakeDetails/{c.Id}"">{c.Name}</a> - ${c.Price:F2} <a href=""/shopping/add/{c.Id}?searchTerm={searchTerm}"">Order</a>
                                 </div>")
                         .ToList();
                }

                var results = "No cakes found";

                if (cakeResults.Any())
                {
                    results = string.Join(Environment.NewLine, cakeResults);
                }

                this.ViewData["results"] = results;
            }
            else
            {
                this.ViewData["results"] = "Please, enter search term";
            }

            // View Shopping Cart
            this.ViewData["showCart"] = "none";

            var shoppingCart = req.Session.Get<ShoppingCart>(ShoppingCart.SessionKey);

            if (shoppingCart.ProductIds.Any())
            {
                var totalProducts = shoppingCart.ProductIds.Count;
                var totalProductsText = totalProducts != 1 ? "products" : "product";

                this.ViewData["showCart"] = "block";
                this.ViewData["products"] = $"{totalProducts} {totalProductsText}";
            }

            return this.FileViewResponse(@"cakes\search");
        }

        public IHttpResponse Details(IHttpRequest req)
        {
            var id = int.Parse(req.UrlParameters["id"]);

            Product cake = null;

            using (var context = new ByTheCakeContext())
            {
                cake = context.Products.Find(id);
            }

            if (cake == null)
            {
                return new BadRequestResponse();
            }

            this.ViewData["name"] = cake.Name;
            this.ViewData["price"] = cake.Price.ToString("F2");
            this.ViewData["imageUrl"] = cake.ImageUrl;

            return this.FileViewResponse(@"cakes\details");
        }

        private User GetUser(IHttpRequest req)
        {
            //Possible bug
            var userId = (int)req.Session.Get(SessionStore.CurrentUserKey);

            using (var context = new ByTheCakeContext())
            {
                return context.Users.Find(userId);
            }
        }
    }
}
