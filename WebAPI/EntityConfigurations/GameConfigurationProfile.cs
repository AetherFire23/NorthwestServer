using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Shared_Resources.Entities;

namespace WebAPI.EntityConfigurations
{
    public class GameConfigurationProfile : IEntityTypeConfiguration<Game>
    {
        public void Configure(EntityTypeBuilder<Game> builder)
        {
            builder.HasKey(e => e.Id);
            builder.HasMany(p => p.PlayersInGame)
                .WithOne(p => p.Game)
                .HasForeignKey(p => p.GameId);
        }
    }
}
