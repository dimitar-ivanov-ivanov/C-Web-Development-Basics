namespace SimpleMvc.App.Views.User
{
    using SimpleMvc.Framework.Interfaces;
    using System.Text;

    public class Register : IRenderable
    {
        public string Render()
        {
            var builder = new StringBuilder();
            builder
                .AppendLine("<p><a href=\"/home/index\">&#10096;Home</a></p>")
                .AppendLine("<h3>Register new user</h3>")
                .AppendLine("<form action=\"register\" method=\"post\">")
                    .AppendLine("Username: <input type=\"text\" name=\"Username\"/>")
                    .AppendLine("</br>")
                    .AppendLine("Password: <input type=\"password\" name=\"Password\"/>")
                    .AppendLine("</br>")
                    .AppendLine("<input type=\"submit\" value=\"Register\"/>")
                .AppendLine("</form>");

            return builder.ToString();
        }
    }
}
