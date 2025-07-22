namespace eMart.Service.DataModels
{
    public class Category
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }

        // Navigation Property
        public ICollection<Product> Products { get; set; }
    }
}
