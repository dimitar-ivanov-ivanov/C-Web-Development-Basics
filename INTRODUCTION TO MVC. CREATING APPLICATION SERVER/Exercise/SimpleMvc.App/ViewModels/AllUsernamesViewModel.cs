﻿namespace SimpleMvc.App.ViewModels
{
    using System.Collections.Generic;

    public class AllUsernamesViewModel
    {
        public IList<string> Usernames { get; set; }

        public IList<int> Ids { get; set; }
    }
}
