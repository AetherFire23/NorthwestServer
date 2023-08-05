using Microsoft.EntityFrameworkCore;
using Shared_Resources.Entities;
using Shared_Resources.Enums;
using WebAPI.Extensions;

namespace WebAPI.Authentication;

public class AuthenticationContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<Role> Roles { get; set; }

    public AuthenticationContext(DbContextOptions<AuthenticationContext> options)
    : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //Role role = new Role()
        //{
        //    Id = Guid.NewGuid(),
        //    RoleName = RoleName.PereNoel,
        //};

        //modelBuilder.Entity<Role>().HasData(role);

        //var user = new User()
        //{
        //    Id = Guid.NewGuid(),
        //    Name = "Fred",
        //    PasswordHash = BCrypt.Net.BCrypt.HashPassword("mereEnShorts"),

        //};

        //UserRole userRole = new UserRole()
        //{
        //    Id = Guid.NewGuid(),
        //};

        //modelBuilder.Entity<User>().HasData(user);
        //modelBuilder.Entity<UserRole>().HasData(new UserRole[] { userRole,});

        //modelBuilder.Entity<UserRole>()
        //    .HasOne(p => p.Role)
        //    .WithMany(p => p.UserRoles)
        //    .HasForeignKey(p => p.RoleId);

        //modelBuilder.Entity<UserRole>()
        //    .HasOne(p => p.User)
        //    .WithMany(user => user.UserRoles)
        //    .HasForeignKey(p => p.UserId);


        _ = modelBuilder.Entity<Role>()
            .Property(p => p.RoleName)
            .HasConversion(
            v => v.ToString(),
            v => v.ToEnum<RoleName>());




        //modelBuilder.Entity<UserRole>(
        //    ur =>
        //    {
        //        ur.HasOne<User>()
        //        .WithMany(u => u.UserRoles)
        //        .HasForeignKey(u => u.UserId);
        //
        //        ur.HasOne<Role>()
        //        .WithMany() // empty means others can have a role, but a role does not have a set of u
        //        .HasForeignKey(u => u.RoleId);
        //    });

    }
}
