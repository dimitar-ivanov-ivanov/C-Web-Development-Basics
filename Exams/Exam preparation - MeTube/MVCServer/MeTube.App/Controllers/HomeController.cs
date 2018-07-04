namespace MeTube.App.Controllers
{
    using MeTube.App.Models.ViewModels;
    using SimpleMvc.Framework.Attributes.Methods;
    using SimpleMvc.Framework.Interfaces;
    using System.Linq;
    using System.Text;

    public class HomeController : BaseController
    {
        [HttpGet]
        public IActionResult Index()
        {
            if (this.User.IsAuthenticated)
            {
                var tubes = this.Context.Tubes
                    .Select(TubeIndexViewModel.FromTube)
                    .ToList();

                var tubesResult = new StringBuilder();

                tubesResult.AppendLine(
                    $@"<div class=""text-center"">
                          <h1>Welcome, {this.User.Name}</h1>
                      </div> 
                     ");

                var startingDivLenght = @"<div class = ""row text-center"">".Length;
                tubesResult.AppendLine(@"<div class=""row text-center"">");

                for (int i = 0; i < tubes.Count; i++)
                {
                    var tube = tubes[i];
                    tubesResult.AppendLine
                        ($@"
                           <div class=""col-4"">
                             <img class=""img-thumbnail tube-thumbnail"" src =""https://img.youtube.com/vi/{tube.YoutubeId}/0.jpg"" alt =""{tube.Title}"">
                             <div>
                               <h5>{tube.Title}</h5>
                               <h5>{tube.Author}</h5>
                             </div>
                           </div >
                          ");

                    if (i % 3 == 2)
                    {
                        tubesResult.AppendLine("</div>");
                        tubesResult.AppendLine(@"<div class=""row text-center"">");
                    }
                }

                //tubesResult = tubesResult.Remove(tubesResult.Length - startingDivLenght, startingDivLenght);
                tubesResult.AppendLine("</div>");

                this.Model.Data["result"] = tubesResult.ToString();
            }
            else
            {
                this.Model.Data["result"] =
                 @"
                       <div class=""jumbotron"">
                           <p class=""h1 display-3"">Welcome to MeTube&trade;!</p>
                           <p class=""h3"">The simplest, easiest to use, most comfortable Multimedia Application.</p>
                           <hr class=""my-3"">
                           <p><a href=""/users/login"">Login</a> if you have an account or<a href=""/users/register"">Register</a> now and start tubing.</p>
                       </div>
                     ";
            }

            return this.View();
        }
    }
}