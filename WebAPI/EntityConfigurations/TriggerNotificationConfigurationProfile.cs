using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebAPI.Entities;

namespace WebAPI.EntityConfigurations;

public class TriggerNotificationConfigurationProfile : IEntityTypeConfiguration<TriggerNotification>
{
    public void Configure(EntityTypeBuilder<TriggerNotification> builder)
    {
        _ = builder.HasKey(e => e.Id);
        //builder.Property(x => x.NotificationType)
        //    .HasColumnType("nvarchar(30)");
    }
}
