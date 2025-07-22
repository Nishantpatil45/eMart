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
            var favoriteProducts = await dbContext.Favorites.Where(f => f.UserId == userDto.Id).ToListAsync();

            if (favoriteProducts.Any())
            {
                return favoriteProducts.Select(favoriteProducts => new FavoriteCommonResponseDto
                {
                    UserId = userDto.Id,
                    ProductId = favoriteProducts.ProductId
                }).ToList();
            }
            else
            {
                throw new Exception(CommonMessages.FavoriteNotFound);
            }
        }

        public async Task<FavoriteCommonResponseDto> RemoveFromFavorite(string id, UserDto userDto)
        {
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
