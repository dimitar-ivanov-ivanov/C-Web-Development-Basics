namespace SimpleMvc.App.Views.Home
{
    using SimpleMvc.Framework.Interfaces;
    using System.Text;

    public class Index : IRenderable
    {
        public string Render()
        {
            var sb = new StringBuilder();
            sb.AppendLine("<h1>NotesApp</h1>");
            sb.AppendLine("<br/>");
            sb.Append(@"<a href = ""/user/all"">All Users</a> ");
            sb.Append(@"<a href = ""/user/register"">Register Users</a>");

            return sb.ToString();
        }
    }
}
