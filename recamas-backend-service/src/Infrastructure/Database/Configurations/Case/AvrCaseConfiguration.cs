using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RECAMAS.Domain.Entities.Case;

namespace RECAMAS.Infrastructure.Database.Configurations;


public sealed class AvrCaseConfiguration : IEntityTypeConfiguration<AvrCase>
{
    public void Configure(EntityTypeBuilder<AvrCase> builder)
    {
        builder.ToTable("avr_cases", schema: "cases");
        builder.HasOne(e => e.CounsellingSession)
            .WithOne(e => e.AvrCase)
            .HasForeignKey<CounsellingSession>(e => e.AvrCaseId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.Ignore(e => e.PreReturnChecklist);
        builder.Ignore(e => e.TravelDocumentIssuances);
        builder.Property(e => e.Program).HasComment("AVRCyprus | EURP");
    }
}
