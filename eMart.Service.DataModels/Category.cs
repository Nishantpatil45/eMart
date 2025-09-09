namespace eMart.Service.DataModels
{
    public class Category
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        public bool? IsDeleted { get; set; } = false;
        // Navigation Property
        public ICollection<Product> Products { get; set; }
    }
}
