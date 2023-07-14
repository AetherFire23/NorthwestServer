using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared_Resources.Entities;
namespace WebAPI.EntityConfigurations;

public class RoomConfigurationProfile : IEntityTypeConfiguration<Room>
{
    public void Configure(EntityTypeBuilder<Room> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Ignore(e => e.AdjacentRoomNames);
    }
}
