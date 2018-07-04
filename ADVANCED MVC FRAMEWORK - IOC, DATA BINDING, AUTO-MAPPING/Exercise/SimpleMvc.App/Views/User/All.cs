namespace SimpleMvc.App.Views.User
{
    using SimpleMvc.App.ViewModels;
    using SimpleMvc.Framework.Interfaces.Generic;
    using System.Text;

    public class All : IRenderable<AllUsernamesViewModel>
    {
        public AllUsernamesViewModel Model { get; set; }

        public string Render()
        {
            var sb = new StringBuilder();
            sb.AppendLine("<p><a href=\"/home/index\">&#10096;Home</a></p>")
            sb.AppendLine("<h3> All users</h3>");
            sb.AppendLine("<ul>");

            for (int i = 0; i < this.Model.Ids.Count; i++)
            {
                sb.AppendLine($@"<li><a href= ""/user/profile?id={this.Model.Ids[i]}"">{this.Model.Usernames[i]}</a></li>");
            }

            sb.AppendLine("</ul>");

            return sb.ToString();
        }
    }
}
