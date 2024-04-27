using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Northwest.Persistence.Entities;

namespace Northwest.Persistence.EntityConfigurations;

public class GameConfigurationProfile : IEntityTypeConfiguration<Game>
{
    public void Configure(EntityTypeBuilder<Game> builder)
    {
        _ = builder.HasKey(e => e.Id);
        _ = builder.HasMany(p => p.PlayersInGame)
            .WithOne(p => p.Game)
            .HasForeignKey(p => p.GameId);
    }
}
