﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Northwest.Persistence.Entities;

namespace Northwest.Persistence.EntityConfigurations;

public class UserConfigurationProfile : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        _ = builder.HasKey(e => e.Id);
        _ = builder.HasMany(p => p.Players)
            .WithOne(p => p.User) // no User prop in Player
            .HasForeignKey(p => p.UserId);

        _ = builder.HasMany
            (p => p.UserLobbies)
            .WithOne(p => p.User);

    }
}
