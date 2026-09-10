using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RECAMAS.Domain.Entities.TCNProfile;
using RECAMAS.Domain.Entities.Case;
using RECAMAS.Domain.Entities.Detention;
using RECAMAS.Domain.Entities.ReturnImplementation;
using RECAMAS.Domain.Entities.Admin;

namespace RECAMAS.Infrastructure.Database.Configurations;


public sealed class DetentionRecordConfiguration : IEntityTypeConfiguration<DetentionRecord>
{
    public void Configure(EntityTypeBuilder<DetentionRecord> builder)
    {
        builder.ToTable("detention_records", schema: "detention");
        EntityConfiguration.ConfigureBase(builder);
        builder.Property(e => e.Bed).HasComment("[ΠΡΟΣΤΕΘΗΚΕ] — έλειπε, βλ. case_detention_period.bed PDF ref: §5.2.2 \"Detention Centers\" — \"the above metrics can also be applied to more detailed levels such as 'Wing' and 'Beds'\"");
        builder.Property(e => e.CumulativeDetentionDays).HasComment("Calculated");
        builder.Property(e => e.MaximumDetentionPeriodDays).HasComment("Από admin parameter");
    }
}
