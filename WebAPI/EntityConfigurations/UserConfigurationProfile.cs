using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Shared_Resources.Entities;

namespace WebAPI.EntityConfigurations
{
    public class UserConfigurationProfile : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(e => e.Id);
            builder.HasMany(p => p.Players)
                .WithOne(p => p.User) // no User prop in Player
                .HasForeignKey(p => p.UserId);

            builder.HasMany
                (p => p.UserLobbies)
                .WithOne(p => p.User);
                
        }
    }
}
