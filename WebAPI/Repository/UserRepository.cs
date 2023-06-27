using Microsoft.EntityFrameworkCore;
using WebAPI.Authentication;
using Shared_Resources.DTOs;
using Shared_Resources.Entities;
using Shared_Resources.Models.Requests;

namespace WebAPI.Repository.Users
{
    public class UserRepository : IUserRepository
    {
        private readonly AuthenticationContext _authContext;

        public UserRepository(AuthenticationContext context)
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

            
            return hasFoundUser? user : null;
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

        //{
        //    if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));
        //    if (string.IsNullOrEmpty(password)) throw new ArgumentNullException(nameof(password));
        //    if (string.IsNullOrEmpty(email)) throw new ArgumentNullException(nameof(email));

        //    if (await _authContext.Users.AnyAsync(u => u.Name == name))
        //    {
        //        throw new UserNameExistsException(name);
        //    }

        //    var newId = Guid.NewGuid();

        //    var newUser = new User
        //    {
        //        Id = newId,
        //        Name = name,
        //        PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
        //        Email = email,
        //        LastLoginDate = DateTime.UtcNow,
        //        RegistrationDate = DateTime.UtcNow,
        //        IsActive = true,
        //        UserRoles = new List<UserRole>
        //        {
        //            new UserRole 
        //            { 
        //                UserId = newId,
        //                RoleId = RoleCache.GetRoleId(RoleName.Standard)
        //            }
        //        }
        //    };

        //    _authContext.Users.Add(newUser);

        //    await _authContext.SaveChangesAsync();

        //    return newUser;
        //}
    }
}
