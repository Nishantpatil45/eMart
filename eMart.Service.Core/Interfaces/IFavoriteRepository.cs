using eMart.Service.Core.Dtos.Favorite;
using eMart.Service.Core.Dtos.User;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eMart.Service.Core.Interfaces
{
    public interface IFavoriteRepository
    {
        Task<FavoriteCommonResponseDto> AddToFavorite(string id, UserDto userDto);

        Task<List<FavoriteCommonResponseDto>> GetAllFavoriteForLoggedInUser(UserDto userDto);

        Task<FavoriteCommonResponseDto> RemoveFromFavorite(string id, UserDto userDto);
    }
}
