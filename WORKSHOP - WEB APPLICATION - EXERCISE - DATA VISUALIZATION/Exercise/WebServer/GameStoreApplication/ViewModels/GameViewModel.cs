namespace HTTPServer.GameStoreApplication.ViewModels
{
    using System;

    public class GameViewModel
    {
        public string Title { get; set; }

        public decimal Price { get; set; }

        public decimal Size { get; set; }

        public string TrailerId { get; set; }

        public string ThumbnailURL { get; set; }

        public string Description { get; set; }

        public DateTime ReleaseDate { get; set; }
    }
}
