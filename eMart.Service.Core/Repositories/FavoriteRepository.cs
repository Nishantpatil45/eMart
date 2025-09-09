using eMart.Service.Core.Dtos.Common;
using eMart.Service.Core.Dtos.Favorite;
using eMart.Service.Core.Dtos.User;
using eMart.Service.Core.Interfaces;
using eMart.Service.DataModels;
using eMart.Service.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eMart.Service.Core.Repositories
{
    public class FavoriteRepository : IFavoriteRepository
    {
        private readonly eMartDbContext dbContext;
        public FavoriteRepository(eMartDbContext _dbContext)
        {
            dbContext = _dbContext;
        }
        public async Task<FavoriteCommonResponseDto> AddToFavorite(string id, UserDto userDto)
        {
            if (string.IsNullOrWhiteSpace(id) || userDto == null || string.IsNullOrWhiteSpace(userDto.Id))
            {
                // Log error
                Console.WriteLine("Invalid product id or user.");
                return null;
            }
            // Check if user or product is deleted
            var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == userDto.Id && (u.IsDeleted == null || u.IsDeleted == false));
            var product = await dbContext.Products.FirstOrDefaultAsync(p => p.Id == id && (p.IsDeleted == null || p.IsDeleted == false));
            if (user == null || product == null)
            {
                Console.WriteLine("User or product not found or deleted.");
                return null;
            }
            var productExits = await dbContext.Favorites.FirstOrDefaultAsync(x => x.ProductId == id && x.UserId == userDto.Id);
            if (productExits == null)
            {
                var favorite = new Favorite
                {
                    UserId = userDto.Id,
                    ProductId = id,
                    CreatedAt = DateTime.UtcNow
                };
                await dbContext.Favorites.AddAsync(favorite);
                await dbContext.SaveChangesAsync();
                return new FavoriteCommonResponseDto
                {
                    UserId = userDto.Id,
                    ProductId = id
                };
            }
            else
            {
                return null;
            }
        }

        public async Task<List<FavoriteCommonResponseDto>> GetAllFavoriteForLoggedInUser(UserDto userDto)
        {
            if (userDto == null || string.IsNullOrWhiteSpace(userDto.Id))
            {
                Console.WriteLine("Invalid user.");
                return new List<FavoriteCommonResponseDto>();
            }
            var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == userDto.Id && (u.IsDeleted == null || u.IsDeleted == false));
            if (user == null)
            {
                Console.WriteLine("User not found or deleted.");
                return new List<FavoriteCommonResponseDto>();
            }
            var favoriteProducts = await dbContext.Favorites
                .Where(f => f.UserId == userDto.Id)
                .Include(f => f.Product)
                .Where(f => f.Product != null && (f.Product.IsDeleted == null || f.Product.IsDeleted == false))
                .ToListAsync();
            return favoriteProducts.Select(favorite => new FavoriteCommonResponseDto
            {
                UserId = userDto.Id,
                ProductId = favorite.ProductId
            }).ToList();
        }

        public async Task<FavoriteCommonResponseDto> RemoveFromFavorite(string id, UserDto userDto)
        {
            if (string.IsNullOrWhiteSpace(id) || userDto == null || string.IsNullOrWhiteSpace(userDto.Id))
            {
                Console.WriteLine("Invalid product id or user.");
                return null;
            }
            var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == userDto.Id && (u.IsDeleted == null || u.IsDeleted == false));
            var product = await dbContext.Products.FirstOrDefaultAsync(p => p.Id == id && (p.IsDeleted == null || p.IsDeleted == false));
            if (user == null || product == null)
            {
                Console.WriteLine("User or product not found or deleted.");
                return null;
            }
            var productExits = await dbContext.Favorites.FirstOrDefaultAsync(x => x.ProductId == id && x.UserId == userDto.Id);
            if (productExits == null)
            {
                return null;
            }
            else
            {
                dbContext.Favorites.Remove(productExits);
                await dbContext.SaveChangesAsync();
                return new FavoriteCommonResponseDto
                {
                    UserId = userDto.Id,
                    ProductId = productExits.ProductId
                };
            }
        }
    }
}
