using eMart.Service.Core.Dtos.Common;
using eMart.Service.Core.Dtos.Enums;
using eMart.Service.Core.Dtos.Product;
using eMart.Service.Core.Dtos.User;
using eMart.Service.Core.Interfaces;
using eMart.Service.DataModels;
using eMart.Service.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System.Security.Cryptography.Xml;

namespace eMart.Service.Core.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly eMartDbContext dbContext;
        public ProductRepository(eMartDbContext _dbContext) 
        {
            dbContext = _dbContext;
        }
        public async Task<ProductCommonResponseDto> CreateProduct(ProductCreateRequestDto productCreateRequestDto, UserDto userDto)
        {
            try
            {
                if (productCreateRequestDto == null || userDto == null || string.IsNullOrWhiteSpace(userDto.Id))
                {
                    Console.WriteLine("Invalid product data or user.");
                    return null;
                }
                var newProduct = new Product
                {
                    Name = productCreateRequestDto.Name,
                    Description = productCreateRequestDto.Description,
                    Price = productCreateRequestDto.Price,
                    CategoryId = productCreateRequestDto.CategoryId,
                    Brand = productCreateRequestDto.Brand,
                    Stock = productCreateRequestDto.Stock,
                    ImageUrl = productCreateRequestDto.ImageUrl,
                    SellerId = userDto.Id,
                    Status = (int)ProductStatusEnum.Active,
                    CreatedBy = userDto.Id,
                    CreatedAt = DateTime.UtcNow,
                    IsDeleted = false
                };
                await dbContext.Products.AddAsync(newProduct);
                await dbContext.SaveChangesAsync();
                return new ProductCommonResponseDto
                {
                    Id = newProduct.Id,
                    Name = newProduct.Name,
                    Description = newProduct.Description,
                    Price = newProduct.Price,
                    CategoryId = newProduct.CategoryId,
                    Brand = newProduct.Brand,
                    Stock = newProduct.Stock,
                    ImageUrl = newProduct.ImageUrl,
                    SellerId = newProduct.SellerId,
                    Status = newProduct.Status,
                    CreatedAt = newProduct.CreatedAt,
                    CreatedBy = newProduct.CreatedBy
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in CreateProduct: {ex.Message}");
                return null;
            }
        }

        public async Task<ProductCommonResponseDto> DeleteProduct(string id, UserDto userDto)
        {
            if (string.IsNullOrWhiteSpace(id) || userDto == null || string.IsNullOrWhiteSpace(userDto.Id))
            {
                Console.WriteLine("Invalid product id or user.");
                return null;
            }
            var existingPoduct = await dbContext.Products.FirstOrDefaultAsync(x => x.Id == id && x.IsDeleted == false);
            if (existingPoduct == null)
            {
                Console.WriteLine("Product not found or already deleted.");
                return null;
            }
            existingPoduct.IsDeleted = true;
            existingPoduct.DeletedBy = userDto.Id;
            await dbContext.SaveChangesAsync();
            return new ProductCommonResponseDto
            {
                Id = existingPoduct.Id,
                Name = existingPoduct.Name,
                Description = existingPoduct.Description,
                Price = existingPoduct.Price,
                CategoryId = existingPoduct.CategoryId,
                Brand = existingPoduct.Brand,
                Stock = existingPoduct.Stock,
                ImageUrl = existingPoduct.ImageUrl,
                SellerId = existingPoduct.SellerId,
                Status = existingPoduct.Status,
                CreatedAt = existingPoduct.CreatedAt,
                CreatedBy = existingPoduct.CreatedBy,
                IsDeleted = true,
                DeletedBy = userDto.Id
            };
        }

        public async Task<List<ProductCommonResponseDto>> GetProduct(UserDto userDto)
        {
            if (userDto == null || string.IsNullOrWhiteSpace(userDto.Id))
            {
                Console.WriteLine("Invalid user.");
                return new List<ProductCommonResponseDto>();
            }
            var favoriteProductIds = await dbContext.Favorites.Where(f => f.UserId == userDto.Id).Select(f => f.ProductId).ToListAsync();
            var products = await dbContext.Products.Where(x => x.IsDeleted == false).ToListAsync();
            return products.Select(product => new ProductCommonResponseDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                CategoryId = product.CategoryId,
                Brand = product.Brand,
                Stock = product.Stock,
                ImageUrl = product.ImageUrl,
                SellerId = product.SellerId,
                Status = product.Status,
                Rating = product.Rating,
                NumReviews = product.NumReviews,
                CreatedAt = product.CreatedAt,
                CreatedBy = product.CreatedBy,
                UpdatedAt = product.UpdatedAt,
                UpdatedBy = product.UpdatedBy,
                IsDeleted = product.IsDeleted,
                DeletedBy = product.DeletedBy,
                IsFavorite = favoriteProductIds.Contains(product.Id)
            }).ToList();
        }

        public async Task<ProductCommonResponseDto> GetProductById(string id, UserDto userDto)
        {
            if (string.IsNullOrWhiteSpace(id) || userDto == null || string.IsNullOrWhiteSpace(userDto.Id))
            {
                Console.WriteLine("Invalid product id or user.");
                return null;
            }
            var product = await dbContext.Products.FirstOrDefaultAsync(x => x.Id == id && x.IsDeleted == false);
            if (product == null)
            {
                Console.WriteLine("Product not found or deleted.");
                return null;
            }
            var favoriteProductIds = await dbContext.Favorites.Where(f => f.UserId == userDto.Id).Select(f => f.ProductId).ToListAsync();
            var productDto = new ProductCommonResponseDto
            {
                Id = id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                CategoryId = product.CategoryId,
                Brand = product.Brand,
                Stock = product.Stock,
                ImageUrl = product.ImageUrl,
                SellerId = product.SellerId,
                Status = product.Status,
                Rating = product.Rating,
                NumReviews = product.NumReviews,
                CreatedAt = product.CreatedAt,
                CreatedBy = product.CreatedBy,
                UpdatedAt = product.UpdatedAt,
                UpdatedBy = product.UpdatedBy,
                IsDeleted = product.IsDeleted,
                IsFavorite = favoriteProductIds.Contains(product.Id)
            };
            return productDto;
        }

        public async Task<List<ProductCommonResponseDto>> GetProductsByCategoryId(string id, UserDto userDto)
        {
            if (string.IsNullOrWhiteSpace(id) || userDto == null || string.IsNullOrWhiteSpace(userDto.Id))
            {
                Console.WriteLine("Invalid category id or user.");
                return new List<ProductCommonResponseDto>();
            }
            var query = dbContext.Products.Where(x => x.CategoryId == id && x.IsDeleted == false);
            var favoriteProductIds = await dbContext.Favorites.Where(f => f.UserId == userDto.Id).Select(f => f.ProductId).ToListAsync();
            var products = await query.Select(p => new ProductCommonResponseDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                CategoryId = p.CategoryId,
                Brand = p.Brand,
                Stock = p.Stock,
                ImageUrl = p.ImageUrl,
                SellerId = p.SellerId,
                Status = p.Status,
                Rating = p.Rating,
                NumReviews = p.NumReviews,
                CreatedAt = p.CreatedAt,
                CreatedBy = p.CreatedBy,
                UpdatedAt = p.UpdatedAt,
                UpdatedBy = p.UpdatedBy,
                IsDeleted = p.IsDeleted,
                IsFavorite = favoriteProductIds.Contains(p.Id)
            }).ToListAsync();
            return products;
        }

        public async Task<ProductCommonResponseDto> UpdateProduct(string id, ProductCreateRequestDto productCreateRequestDto, UserDto userDto)
        {
            if (string.IsNullOrWhiteSpace(id) || productCreateRequestDto == null || userDto == null || string.IsNullOrWhiteSpace(userDto.Id))
            {
                Console.WriteLine("Invalid product id, data, or user.");
                return null;
            }
            var product = await dbContext.Products.FirstOrDefaultAsync(x => x.Id == id && x.IsDeleted == false);
            if (product == null)
            {
                Console.WriteLine("Product not found or deleted.");
                return null;
            }
            product.Name = productCreateRequestDto.Name;
            product.Description = productCreateRequestDto.Description;
            product.Price = productCreateRequestDto.Price;
            product.CategoryId = productCreateRequestDto.CategoryId;
            product.Brand = productCreateRequestDto.Brand;
            product.Stock = productCreateRequestDto.Stock;
            product.ImageUrl = productCreateRequestDto.ImageUrl;
            product.SellerId = userDto.Id;
            product.Status = productCreateRequestDto.Status;
            product.UpdatedAt = DateTime.UtcNow;
            product.UpdatedBy = userDto.Id;
            await dbContext.SaveChangesAsync();
            return new ProductCommonResponseDto()
            {
                Id = product.Id,
                Name = productCreateRequestDto.Name,
                Description = productCreateRequestDto.Description,
                Price = productCreateRequestDto.Price,
                CategoryId = productCreateRequestDto.CategoryId,
                Brand = productCreateRequestDto.Brand,
                Stock = productCreateRequestDto.Stock,
                ImageUrl = productCreateRequestDto.ImageUrl,
                SellerId = userDto.Id,
                Status = productCreateRequestDto.Status,
                UpdatedAt = DateTime.UtcNow,
                UpdatedBy = userDto.Id,
                IsDeleted = false
            };
        }
    }
}
