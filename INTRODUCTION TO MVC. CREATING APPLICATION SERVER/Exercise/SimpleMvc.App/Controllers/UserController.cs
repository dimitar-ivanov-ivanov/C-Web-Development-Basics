namespace SimpleMvc.App.Controllers
{
    using SimpleMvc.App.BindingModels;
    using SimpleMvc.App.ViewModels;
    using SimpleMvc.Data;
    using SimpleMvc.Domain;
    using SimpleMvc.Framework.Attributes.Methods;
    using SimpleMvc.Framework.Controllers;
    using SimpleMvc.Framework.Interfaces;
    using SimpleMvc.Framework.Interfaces.Generic;
    using System.Collections.Generic;
    using System.Linq;

    public class UserController : Controller
    {
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterUserBindingModel model)
        {
            using (var context = new NotesDb())
            {
                var user = new User()
                {
                    Password = model.Password,
                    Username = model.Username
                };

                context.Users.Add(user);
                context.SaveChanges();
            }

            return View();
        }

        [HttpGet]
        public IActionResult<AllUsernamesViewModel> All()
        {
            List<string> usernames = null;
            List<int> ids = null;

            using (var context = new NotesDb())
            {
                usernames = context.Users.Select(u => u.Username).ToList();
                ids = context.Users.Select(u => u.Id).ToList();
            }

            var viewModel = new AllUsernamesViewModel()
            {
                Usernames = usernames,
                Ids = ids
            };

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult<UserProfileViewModel> Profile(int id)
        {
            using (var context = new NotesDb())
            {

                var viewModel = context
                                .Users
                                .Where(u => u.Id == id)
                                .Select(u => new UserProfileViewModel
                                {
                                    UserId = u.Id,
                                    Username = u.Username,
                                    Notes = u.Notes
                                            .Select(n => new NoteViewModel
                                            {
                                                Title = n.Title,
                                                Content = n.Content
                                            })
                                            .ToList()
                                })
                                .FirstOrDefault();

                return View(viewModel);
            }
        }

        [HttpPost]
        public IActionResult<UserProfileViewModel> Profile(AddNoteBindingModel model)
        {
            using (var context = new NotesDb())
            {
                var user = context.Users.Find(model.UserId);

                var note = new Note()
                {
                    Title = model.Title,
                    Content = model.Content
                };

                context.Notes.Add(note);
                user.Notes.Add(note);
                context.SaveChanges();
            }

            return Profile(model.UserId);
        }
    }
}