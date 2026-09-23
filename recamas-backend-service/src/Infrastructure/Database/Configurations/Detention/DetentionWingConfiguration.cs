using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RECAMAS.Domain.Entities.Detention;

namespace RECAMAS.Infrastructure.Database.Configurations;


public sealed class DetentionWingConfiguration : IEntityTypeConfiguration<DetentionWing>
{
    public void Configure(EntityTypeBuilder<DetentionWing> builder)
    {
        builder.ToTable("detention_wings", schema: "detention");
        EntityConfiguration.ConfigureBase(builder);
        builder.Property(e => e.Name).HasComment("Όνομα πτέρυγας — free text");
    }
}
