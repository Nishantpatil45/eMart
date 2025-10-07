using eMart.Service.Core.Dtos.User;
using eMart.Service.Core.Interfaces;
using eMart.Service.DataModels;
using eMart.Service.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Text;

namespace eMart.Service.Core.Repositories
{
    public class UserRepository : RepositoryBase<UserRepository>, IUserRepository
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<UserRepository> _logger;
        public UserRepository(IConfiguration configuration, eMartDbContext dbContext, ILogger<UserRepository> logger) : base(dbContext)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<UserCommonResponseDto> CreateUser(UserCreateRequestDto userCreateRequestDto, UserDto userDto)
        {
            // Basic email format validation
            if (!Regex.IsMatch(userCreateRequestDto.Email ?? "", @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                _logger.LogWarning("Registration failed: Invalid email format for {Email}", userCreateRequestDto.Email);
                return null;
            }
            // Basic password strength validation
            if (string.IsNullOrWhiteSpace(userCreateRequestDto.Password) || userCreateRequestDto.Password.Length < 8)
            {
                _logger.LogWarning("Registration failed: Weak password for {Email}", userCreateRequestDto.Email);
                return null;
            }
            // Check for existing user by Email or Name, and not deleted
            var userExits = dbContext.Users.FirstOrDefault(x => (x.Email == userCreateRequestDto.Email || x.Name == userCreateRequestDto.Name) && (x.IsDeleted == null || x.IsDeleted == false));
            if (userExits != null)
            {
                _logger.LogWarning("Registration failed: User already exists for {Email} or {Name}", userCreateRequestDto.Email, userCreateRequestDto.Name);
                return null;
            }
            // Implement rate limiting here
            // Implement email verification flow here
            // Implement audit trail for user creation
            // For production, use a slow hash algorithm (e.g., bcrypt, PBKDF2, Argon2)
            CreatePasswordHash(userCreateRequestDto.Password, out byte[] passwordHash, out byte[] passwordSalt);
            // Map DTO to User entity
            var createdBy = userDto?.Name ?? "self";
            var user = new User
            {
                Id = Guid.NewGuid().ToString(),
                Email = userCreateRequestDto.Email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Name = userCreateRequestDto.Name,
                Role = userCreateRequestDto.Role,
                CreatedBy = createdBy,
                CreatedAt = DateTime.UtcNow,
                IsDeleted = false
            };
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

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        public async Task<UserDto> GetUserDetails(string emailId)
        {
            var user = await dbContext.Users.FirstOrDefaultAsync(x => x.Email == emailId && (x.IsDeleted == null || x.IsDeleted == false));
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

        public async Task<UserDto> GetUserDetailsById(string userId)
        {
            var user = await dbContext.Users.FirstOrDefaultAsync(x => x.Id == userId && (x.IsDeleted == null || x.IsDeleted == false));
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

        public async Task<UserDto> GetUserForAuthentication(string emailId)
        {
            var user = await dbContext.Users.FirstOrDefaultAsync(x => x.Email == emailId && (x.IsDeleted == null || x.IsDeleted == false));
            if (user == null)
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
