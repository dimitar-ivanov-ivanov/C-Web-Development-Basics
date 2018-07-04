namespace MeTube.App.Models.ViewModels
{
    using System;
    using MeTube.Models;

    public class TubeProfileViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Author { get; set; }

        public static Func<Tube, TubeProfileViewModel> FromTube =>
            tube => new TubeProfileViewModel()
            {
                Author = tube.Author,
                Id = tube.Id,
                Title = tube.Title
            };
    }
}
