using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Northwest.Persistence.Entities;

namespace Northwest.Persistence.EntityConfigurations;

public class RoomConfigurationProfile : IEntityTypeConfiguration<Room>
{
    public void Configure(EntityTypeBuilder<Room> builder)
    {
        _ = builder.HasKey(e => e.Id);
        _ = builder.Ignore(e => e.AdjacentRoomNames);
    }
}
