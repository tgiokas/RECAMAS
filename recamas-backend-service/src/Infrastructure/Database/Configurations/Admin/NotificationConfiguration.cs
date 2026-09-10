using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RECAMAS.Domain.Entities.TCNProfile;
using RECAMAS.Domain.Entities.Case;
using RECAMAS.Domain.Entities.Detention;
using RECAMAS.Domain.Entities.ReturnImplementation;
using RECAMAS.Domain.Entities.Admin;

namespace RECAMAS.Infrastructure.Database.Configurations;


public sealed class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.ToTable("notifications", schema: "admin");
        EntityConfiguration.ConfigureBase(builder);
        builder.Property(e => e.Channel).HasComment("InApp | Email | Both");
        builder.Property(e => e.Title).HasComment("Τίτλος — free text (template-generated)");
        builder.Property(e => e.Body).HasComment("Σώμα — free text");
        builder.Property(e => e.DeepLinkEntityType).HasComment("Case | Implementation | Profile");
    }
}
