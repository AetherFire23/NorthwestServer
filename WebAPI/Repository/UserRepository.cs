using Microsoft.EntityFrameworkCore;
using WebAPI.Authentication;
using Shared_Resources.DTOs;
using Shared_Resources.Entities;
using Shared_Resources.Models.Requests;
using Shared_Resources.Enums;

namespace WebAPI.Repository.Users
{
    public class UserRepository : IUserRepository
    {
        private readonly PlayerContext _authContext;

        public UserRepository(PlayerContext context)
        {
            _authContext = context;
        }

        public async Task<UserDto> GetUserDtoById(Guid id)
        {
            var user = await GetUserById(id);

            var roleNames = user.UserRoles.Select(x => x.Role.RoleName).ToList();

            var userDto = new UserDto()
            {
                Id = user.Id,
                Name = user.Name,
                RoleNames = roleNames
            };
            return userDto;
        }

        public async Task<User?> GetUserByVerifyingCredentialsOrNull(LoginRequest loginRequest)
        {
            if (string.IsNullOrEmpty(loginRequest.UserName)) throw new ArgumentNullException(nameof(loginRequest.UserName));
            if (string.IsNullOrEmpty(loginRequest.PasswordAttempt)) throw new ArgumentNullException(nameof(loginRequest.PasswordAttempt));

            User? verifiedUser = await GetUserIfVerifiedOrNull(loginRequest).ConfigureAwait(false);
            return verifiedUser;
        }


        public async Task<User?> GetUserById(Guid id)
        {
            var user = await _authContext.Users
                .Include(u => u.UserRoles)
                .FirstOrDefaultAsync(u => u.Id == id);

            return user;
        }

        private async Task<User?> GetUserIfVerifiedOrNull(LoginRequest request)
        {
            var user = await GetUserByNameOrNull(request.UserName).ConfigureAwait(false);

            bool hasFoundUser = user is not null;
            if (!hasFoundUser) return null; // should make error message srsly


            return hasFoundUser ? user : null;
        }

        private async Task<bool> IsCorrectCredentials(LoginRequest request, User? user)
        {
            bool isCorrect = BCrypt.Net.BCrypt.Verify(request.PasswordAttempt, user.PasswordHash);
            return isCorrect;
        }

        private async Task<User?> GetUserByNameOrNull(string name)
        {
            var user = await _authContext.Users
                .Include(u => u.UserRoles)
                .ThenInclude(u => u.Role)
                .FirstOrDefaultAsync(u => u.Name == name);


            return user;
        }

        private async Task<User?> GetUserIfVerifiedOrNull(Guid id, string password)
        {
            var user = await GetUserById(id).ConfigureAwait(false);
            bool isCorrect = BCrypt.Net.BCrypt.Verify(user.PasswordHash, password);
            if (!isCorrect) throw new ArgumentException("Password incorrect");
            return isCorrect ? user : null;
        }

        public async Task<bool> IsUserExists(string userName, string email)
        {
            bool exists = await _authContext.Users.AnyAsync(x => x.Name == userName && x.Email == email);
            return exists;
        }

        public async Task<UserDto> CreateUser(RegisterRequest request)
        {
            var user = new User()
            {
                Id = Guid.NewGuid(),
                Email = request.Email,
                Name = request.UserName,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            };

            var role = await _authContext.Roles.SingleAsync(x => x.RoleName == RoleName.PereNoel);

            var userRole = new UserRole()
            {
                Id = Guid.NewGuid(),
                Role = role,
                User = user,
            };
            await _authContext.Users.AddAsync(user);
            await _authContext.SaveChangesAsync();

            await _authContext.UserRoles.AddAsync(userRole);
            await _authContext.SaveChangesAsync();

            var dto = await GetUserDtoById(user.Id);

            return dto;
        }
    }
}
