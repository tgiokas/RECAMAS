using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RECAMAS.Domain.Entities.Detention;

namespace RECAMAS.Infrastructure.Database.Configurations.Detention;

public class DetentionFacilityConfiguration : IEntityTypeConfiguration<DetentionFacility>
{
    public void Configure(EntityTypeBuilder<DetentionFacility> builder)
    {
        builder.ToTable("detention_facilities", schema: "detention");
        builder.HasKey(e => e.Id);
    }
}
