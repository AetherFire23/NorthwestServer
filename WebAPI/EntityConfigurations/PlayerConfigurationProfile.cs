using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared_Resources.Entities;

namespace WebAPI.EntityConfigurations
{
    public class PlayerConfigurationProfile : IEntityTypeConfiguration<Player>
    {
        public void Configure(EntityTypeBuilder<Player> builder)
        {
            builder.HasKey(e => e.Id);
        }
    }
}
