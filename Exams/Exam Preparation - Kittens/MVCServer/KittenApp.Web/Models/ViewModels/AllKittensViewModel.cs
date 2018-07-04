namespace KittenApp.Web.Models.ViewModels
{
    using KittenApp.Models;
    using System;

    public class AllKittensViewModel
    {
        public string Name { get; set; }

        public int Age { get; set; }

        public string Breed { get; set; }

        public static Func<Kitten, AllKittensViewModel> FromKittens =>
           kitten => new AllKittensViewModel()
           {
               Age = kitten.Age,
               Breed = kitten.Breed.Name,
               Name = kitten.Name
           };
    }
}
