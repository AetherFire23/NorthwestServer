using Shared_Resources.Entities;
using Shared_Resources.Enums;

namespace WebAPI.Authentication
{
    public static class AuthInitialData
    {
        public static User MyUser = new User()
        {
            Id = Guid.NewGuid(),
            Name = "Fred",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("mereEnShorts"),
        };

        public static Role PereNoel = new Role()
        {
            Id = Guid.NewGuid(),
            RoleName = RoleName.PereNoel,
        };

        public static UserRole MyUserRole = new UserRole()
        {
            Id = Guid.NewGuid(),
            Role = PereNoel,
            User = MyUser,
        };
    }
}
