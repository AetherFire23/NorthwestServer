using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared_Resources.Entities;

namespace WebAPI.EntityConfigurations
{
    public class TriggerNotificationConfigurationProfile : IEntityTypeConfiguration<TriggerNotification>
    {
        public void Configure(EntityTypeBuilder<TriggerNotification> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(x => x.NotificationType)
                .HasColumnType("nvarchar(30)");
        }
    }
}
