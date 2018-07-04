namespace MeTube.App.Controllers
{
    using MeTube.App.Attributes;
    using MeTube.App.Models.BindingModels;
    using MeTube.App.Models.ViewModels;
    using MeTube.Models;
    using SimpleMvc.Common;
    using SimpleMvc.Framework.Attributes.Methods;
    using SimpleMvc.Framework.Interfaces;
    using System.Linq;

    public class TubesController : BaseController
    {
        [HttpGet]
        [AuthrorizedLogin]
        public IActionResult Upload()
        {
            this.Model.Data["error"] = string.Empty;
            return this.View();
        }

        [HttpPost]
        [AuthrorizedLogin]
        public IActionResult Upload(TubeUploadBindingModel model)
        {
            if (!this.IsValidModel(model))
            {
                this.Model.Data["error"] = ErrorMessages.InvalidTubeAddModel;
                return this.View();
            }

            var youtubeId = string.Empty;

            if (model.YoutubeLink.Contains("youtube.com"))
            {
                youtubeId = model.YoutubeLink.Split("?v=")[1];
            }
            else
            {
                this.Model.Data["error"] = ErrorMessages.InvalidTubeAddModel;
                return this.View();
            }

            var tube = new Tube
            {
                Author = model.Author,
                Description = model.Description,
                Title = model.Title,
                UploaderId = this.DbUser.Id,
                Views = 0,
                YoutubeId = youtubeId
            };

            this.Context.Tubes.Add(tube);
            this.DbUser.Tubes.Add(tube);
            this.Context.SaveChanges();

            return this.RedirectToAction($"/tubes/details/?id={tube.Id}");
        }

        [HttpGet]
        [AuthrorizedLogin]
        public IActionResult Details(int id)
        {
            var tube = this.Context.Tubes.FirstOrDefault(t => t.Id == id);

            if (tube == null)
            {
                return this.RedirectHome();
            }

            tube.Views++;
            this.Context.SaveChanges();

            var model = new[] { tube }
            .Select(TubeDetailsViewModel.FromTube)
            .Single();

            this.Model.Data["title"] = model.Title;
            this.Model.Data["youtubeId"] = model.YoutubeId;
            this.Model.Data["author"] = model.Author;
            this.Model.Data["views"] = $"{model.Views} View{(model.Views == 1 ? "" : "s")}";
            this.Model.Data["description"] = model.Description;

            return this.View();
        }
    }
}
