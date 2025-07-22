namespace eMart.Service.DataModels
{
    public class User
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Email { get; set; }
        public byte[]? PasswordHash { get; set; }
        public byte[]? PasswordSolt { get; set; }
        public string Name { get; set; }
        public string Role { get; set; } = "user";
        public string? ProfilePicture { get; set; }
        public bool DarkMode { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public string? UpdatedBy { get; set; }
        public bool? IsDeleted { get; set; }
        public string? DeletedBy { get; set; }

        // Navigation Properties
        public ICollection<UserPreference> Preferences { get; set; }
        public ICollection<Product> Products { get; set; }
        public ICollection<Order> Orders { get; set; }
        public ICollection<Bid> Bids { get; set; }
        public ICollection<Favorite> Favorites { get; set; }
        public ICollection<RecentlyViewed> RecentlyViewedItems { get; set; }
        public ICollection<UserToken> UserTokens { get; set; }
    }
}
