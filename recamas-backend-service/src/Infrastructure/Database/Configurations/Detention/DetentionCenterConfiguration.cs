using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RECAMAS.Domain.Entities.TCNProfile;
using RECAMAS.Domain.Entities.Case;
using RECAMAS.Domain.Entities.Detention;
using RECAMAS.Domain.Entities.ReturnImplementation;
using RECAMAS.Domain.Entities.Admin;

namespace RECAMAS.Infrastructure.Database.Configurations;


public sealed class DetentionCenterConfiguration : IEntityTypeConfiguration<DetentionCenter>
{
    public void Configure(EntityTypeBuilder<DetentionCenter> builder)
    {
        builder.ToTable("detention_centers", schema: "detention");
        EntityConfiguration.ConfigureBase(builder);
        builder.Property(e => e.Name).HasComment("Επωνυμία — free text");
        builder.Property(e => e.Location).HasComment("Τοποθεσία — free text");
    }
}
