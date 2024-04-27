using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Northwest.Persistence.Entities;

namespace Northwest.Persistence.EntityConfigurations;

public class PlayerConfigurationProfile : IEntityTypeConfiguration<Player>
{
    public void Configure(EntityTypeBuilder<Player> builder)
    {
        _ = builder.HasKey(e => e.Id);
    }
}
