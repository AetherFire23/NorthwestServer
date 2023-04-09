using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Shared_Resources.Entities;
using Shared_Resources.Enums;
using Shared_Resources.Interfaces;

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
