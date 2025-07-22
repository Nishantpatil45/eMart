using eMart.Service.Core.Dtos.User;
using eMart.Service.Core.Interfaces;
using eMart.Service.DataModels;
using eMart.Service.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace eMart.Service.Core.Repositories
{
    public class UserRepository : RepositoryBase<UserRepository>, IUserRepository
    {
        private readonly IConfiguration _configuration;
        public UserRepository(IConfiguration configuration, eMartDbContext dbContext) : base(dbContext) 
        { 
            _configuration = configuration;
        }

        public async Task<UserCommonResponseDto> CreateUser(UserCreateRequestDto userCreateRequestDto, UserDto userDto)
        {
            var userExits = dbContext.Users.FirstOrDefault(x => x.Name == userCreateRequestDto.Name || x.Role == userCreateRequestDto.Role);

            if (userExits == null)
            {
                CreatePasswordHash(userCreateRequestDto.Password, out byte[] passwordHsh, out byte[] passwordSalt);

                // Map DTO to User entity
                var user = new User
                {
                    Id = Guid.NewGuid().ToString(),
                    Email = userCreateRequestDto.Email,
                    PasswordHash = passwordHsh,
                    PasswordSolt = passwordSalt,
                    Name = userCreateRequestDto.Name,
                    Role = userCreateRequestDto.Role,
                    CreatedBy = userDto.Name,
                    CreatedAt = DateTime.UtcNow,
                    IsDeleted = false
                };

                // Save to database
                dbContext.Users.Add(user);
                dbContext.SaveChanges();
                return new UserCommonResponseDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    Name = user.Name,
                    Role = user.Role,
                    CreatedAt = user.CreatedAt
                };
            }
            else
            {
                return null;
            }
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSolt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSolt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public async Task<UserDto> GetUserDetails(string emailId)
        {
            var user = await dbContext.Users.FirstOrDefaultAsync(x => (x.Email == emailId)/* && x.IsDeleted == false*/);

            if (user == null)
            {
                return null;
            }

            var isValideUser = await dbContext.UserTokens.FirstOrDefaultAsync(u => u.UserId == user.Id);

            if (isValideUser == null)
            {
                return null;
            }

            return new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                Name = user.Name,
                Role = user.Role,
                CreatedBy = user.CreatedBy,
                UpdatedBy = user.UpdatedBy
            };
        }
    }
}

    
