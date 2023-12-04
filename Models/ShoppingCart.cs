namespace Projet.Models
{
    public class ShoppingCart
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public List<CartItem> Items { get; set; }

        public decimal TotalPrice
        {
            get
            {
                if (Items != null && Items.Count > 0)
                {
                    decimal total = 0;
                    foreach (var item in Items)
                    {
                        total += item.Quantity * item.Product.Price;
                    }
                    return total;
                }
                return 0;
            }
        }
    }

    public class CartItem
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public Product Product { get; set; }
    }
}
