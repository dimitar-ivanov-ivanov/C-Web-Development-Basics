namespace MeTube.App.Models.ViewModels
{
    using MeTube.Models;
    using System;

    public class TubeDetailsViewModel
    {
        public string Title { get; set; }

        public string YoutubeId { get; set; }

        public string Author { get; set; }

        public int Views { get; set; }

        public string Description { get; set; }

        public static Func<Tube, TubeDetailsViewModel> FromTube =>
             tube => new TubeDetailsViewModel()
             {
                 Author = tube.Author,
                 Description = tube.Description,
                 Views = tube.Views,
                 YoutubeId = tube.YoutubeId,
                 Title = tube.Title
             };
    }
}
