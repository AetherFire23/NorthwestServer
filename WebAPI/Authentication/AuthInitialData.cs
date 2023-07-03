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
            Id = new Guid("4e9ec37d-71d9-4e66-8970-45424db1eeb1"),
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
