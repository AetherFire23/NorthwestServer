using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebAPI.Entities;

namespace WebAPI.EntityConfigurations;

public class PlayerConfigurationProfile : IEntityTypeConfiguration<Player>
{
    public void Configure(EntityTypeBuilder<Player> builder)
    {
        _ = builder.HasKey(e => e.Id);
    }
}
