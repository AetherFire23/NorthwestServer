﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebAPI.Entities;
namespace WebAPI.EntityConfigurations;

public class RoomConfigurationProfile : IEntityTypeConfiguration<Room>
{
    public void Configure(EntityTypeBuilder<Room> builder)
    {
        _ = builder.HasKey(e => e.Id);
        _ = builder.Ignore(e => e.AdjacentRoomNames);
    }
}
