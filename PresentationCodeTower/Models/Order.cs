namespace PresentationCodeTower.Models
{
    public class Order : BaseModel
    {
        public List<Product> Products { get; set; }
        public User User { get; set; }

        public Order(
            User User,
            List<Product> products)
        {
            this.Products = products;
            this.User = User;
            this.CreatedAt = DateTime.Now;
        }
    }
}
