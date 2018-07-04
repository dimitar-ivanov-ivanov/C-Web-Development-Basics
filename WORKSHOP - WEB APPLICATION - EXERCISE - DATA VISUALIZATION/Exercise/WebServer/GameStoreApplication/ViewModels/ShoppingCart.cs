namespace HTTPServer.GameStoreApplication.ViewModels
{
    using System.Collections.Generic;

    public class ShoppingCart
    {
        public ShoppingCart()
        {
            this.GameIds = new LinkedList<int>();
        }

        public const string SessionKey = "^%Current_Shopping_Cart%^";

        public ICollection<int> GameIds { get; private set; }
    }
}
