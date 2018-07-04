namespace MeTube.App.Models.ViewModels
{
    using MeTube.Models;
    using System;

    public class TubeIndexViewModel
    {
        public string YoutubeId { get; set; }

        public string Title { get; set; }

        public string Author { get; set; }

        public static Func<Tube, TubeIndexViewModel> FromTube =>
            tube => new TubeIndexViewModel()
            {
                Author = tube.Author,
                YoutubeId = tube.YoutubeId,
                Title = tube.Title
            };
    }
}
