namespace HTTPServer.ByTheCakeApplication.Controllers
{
    using Models;
    using Infrastructure;
    using Server.Http.Contracts;
    using Server.Http.Response;
    using System.Linq;
    using System;
    using HTTPServer.ByTheCakeApplication.Data;
    using System.Text;
    using HTTPServer.Server.Http;

    public class ShoppingController : Controller
    {
        public IHttpResponse Order(IHttpRequest req)
        {
            var id = int.Parse(req.UrlParameters["id"]);

            using (var context = new ByTheCakeContext())
            {
                var product = context.Products.Find(id);
                var user = GetUser(req);

                if (product == null)
                {
                    return new NotFoundResponse();
                }

                var shoppingCart = req.Session.Get<ShoppingCart>(ShoppingCart.SessionKey);

                shoppingCart.ProductIds.Add(id);
            }

            var redirectUrl = "/search";

            const string searchTermKey = "searchTerm";

            if (req.UrlParameters.ContainsKey(searchTermKey))
            {
                redirectUrl = $"{redirectUrl}?{searchTermKey}={req.UrlParameters[searchTermKey]}";
            }

            return new RedirectResponse(redirectUrl);
        }

        public IHttpResponse ShowCart(IHttpRequest req)
        {
            var shoppingCart = req.Session.Get<ShoppingCart>(ShoppingCart.SessionKey);

            if (!shoppingCart.ProductIds.Any())
            {
                this.ViewData["cartItems"] = "No items in your cart";
                this.ViewData["totalCost"] = "0.00";
            }
            else
            {
                using (var context = new ByTheCakeContext())
                {
                    var productIds = shoppingCart.ProductIds;

                    //TODO
                    var productsInCart = productIds
                        .Select(p => context.Products.Find(p));

                    var items = productsInCart
                                .Select(p => $"<div>{p.Name} - ${p.Price:F2}</div><br />");

                    var totalPrice = productsInCart.Sum(p => p.Price);

                    this.ViewData["cartItems"] = string.Join(string.Empty, items);
                    this.ViewData["totalCost"] = $"{totalPrice:F2}";
                }
            }

            return this.FileViewResponse(@"shopping\cart");
        }

        public IHttpResponse FinishOrder(IHttpRequest req)
        {
            using (var context = new ByTheCakeContext())
            {
                var user = GetUser(req);
                if (user == null)
                {
                    return new BadRequestResponse();
                }

                var userId = context.Users.FirstOrDefault(u => u.Id == user.Id).Id;

                var shoppingCart = req.Session.Get<ShoppingCart>(ShoppingCart.SessionKey);

                var order = new Order()
                {
                    RegistrationDate = DateTime.UtcNow,
                    UserId = userId
                };

                var products = context.Products
                    .Where(p => shoppingCart.ProductIds.Contains(p.Id))
                    .ToList();

                context.Orders.Add(order);
                user.Orders.Add(order);

                context.SaveChanges();

                foreach (var product in products)
                {
                    var productOrder = new ProductOrder()
                    {
                        Order = order,
                        Product = product
                    };

                    order.Products.Add(productOrder);
                    product.Orders.Add(productOrder);
                }

                context.SaveChanges();
                shoppingCart.ProductIds.Clear();
            }

            return this.FileViewResponse(@"shopping\finish-order");
        }

        public IHttpResponse ListOrders(IHttpRequest req)
        {
            using (var context = new ByTheCakeContext())
            {
                var orders = context.Orders
                    .OrderByDescending(o => o.RegistrationDate);

                var result = new StringBuilder();

                foreach (var order in orders)
                {
                    var productOrderSum = context.ProductOrders
                        .Where(po => po.OrderId == order.Id)
                        .Sum(p => p.Product.Price);

                    result.AppendLine("<tr>" +
                                        $"<td><a href=\"/orderDetails/{order.Id}\">{order.Id}</a></td>" +
                                        $"<td>{order.RegistrationDate:dd-MM-yyyy}</td>" +
                                        $"<td>${productOrderSum:f2}</td>" +
                                      "</tr>");
                }

                this.ViewData["results"] = result.ToString();
            }

            return this.FileViewResponse(@"shopping\orders");
        }

        public IHttpResponse Details(IHttpRequest req)
        {
            var id = int.Parse(req.UrlParameters["id"]);

            using (var context = new ByTheCakeContext())
            {
                var order = context.Orders.Find(id);

                if (order == null)
                {
                    return new BadRequestResponse();
                }

                var productIds = context.ProductOrders
                    .Where(po => po.OrderId == order.Id)
                    .Select(po => po.ProductId)
                    .ToList();

                var products = context.Products
                    .Where(p => productIds.Contains(p.Id))
                    .ToList();

                var result = new StringBuilder();

                foreach (var product in products)
                {
                    result.AppendLine("<tr>" +
                                         $@"<td><a href=""cakeDetails/{product.Id}"">{product.Name}</a></td>" +
                                         $"<td>${product.Price:f2}</td>" +
                                      "</tr>");
                }

                this.ViewData["Id"] = order.Id.ToString();
                this.ViewData["results"] = result.ToString();
                this.ViewData["registrationDate"] = order.RegistrationDate
                    .ToString("dd-MM-yyyy");
            }

            return this.FileViewResponse(@"shopping\order-details");
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