namespace KittenApp.Web.Controllers
{
    using KittenApp.Models;
    using KittenApp.Web.Attributes;
    using KittenApp.Web.Models.BindingModels;
    using KittenApp.Web.Models.ViewModels;
    using SimpleMvc.Common;
    using SimpleMvc.Framework.Attributes.Methods;
    using SimpleMvc.Framework.Interfaces;
    using System.Linq;
    using System.Text;
    using Microsoft.EntityFrameworkCore;

    public class KittensController : BaseController
    {
        [HttpGet]
        [AuthorizeLogin]
        public IActionResult Add()
        {
            return this.View();
        }

        [HttpPost]
        [AuthorizeLogin]
        public IActionResult Add(AddKittenModel model)
        {
            if (!this.IsValidModel(model))
            {
                this.Model.Data["error"] = ErrorMessages.InvalidAddKittenModel;
                return this.View();
            }

            var breed = this.Context.Breeds.FirstOrDefault(b => b.Name == model.Breed);

            if (breed == null)
            {
                this.Model.Data["error"] = ErrorMessages.InvalidAddKittenModel;
                return this.View();
            }

            var kitten = new Kitten()
            {
                Age = model.Age,
                Name = model.Name,
                Breed = new Breed()
                {
                    Name = model.Breed
                }
            };

            this.Context.Kittens.Add(kitten);
            this.Context.SaveChanges();

            return this.RedirectToAction("/kittens/all");
        }

        [HttpGet]
        [AuthorizeLogin]
        public IActionResult All()
        {
            var kittens = this.Context
                .Kittens
                .Include(k => k.Breed)
                .Select(AllKittensViewModel.FromKittens)
                .ToList();

            var kittensResult = new StringBuilder();

            kittensResult.Append(@"<div class=""row text-center"">");

            for (int i = 0; i < kittens.Count; i++)
            {
                var kitten = kittens[i];
                kittensResult.Append(
                    $@"<div class=""col-4"">
                            <img src=""../../Content/img/munchkin.jpg"" alt=""{kitten.Name}"" />
                            <div>
                                <h5>Name: {kitten.Name}</h5>
                            </div>
                             <div>
                                <h5>Age: {kitten.Age}</h5>
                            </div> 
                            <div>
                                <h5>Breed: {kitten.Breed}</h5>
                            </div>
                      </div>");

                if (i % 3 == 2)
                {
                    kittensResult.Append(@"</div><div class=""row text-center"">");
                }
            }

            kittensResult.Append("</div>");

            this.Model.Data["result"] = kittensResult.ToString();

            return this.View();
        }
    }
}