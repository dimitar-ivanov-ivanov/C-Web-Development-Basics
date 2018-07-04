namespace Notes.App.Controllers
{
    using Notes.Data;
    using SimpleMvc.Framework.Controllers;

    public abstract class BaseController : Controller
    {
        protected BaseController()
            : this(new NotesContext())
        {
            this.Context = new NotesContext();
        }

        protected BaseController(NotesContext notesContext)
        {
            this.Context = notesContext;
        }

        public NotesContext Context { get; set; }
    }
}