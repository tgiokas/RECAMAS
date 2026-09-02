using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RECAMAS.Domain.Entities.Detention;

namespace RECAMAS.Infrastructure.Database.Configurations;

public class DetentionReassessmentConfiguration : IEntityTypeConfiguration<DetentionReassessment>
{
    public void Configure(EntityTypeBuilder<DetentionReassessment> builder)
    {
        builder.ToTable("detention_reassessments", schema: "detention");
        builder.HasKey(e => e.Id);

        builder.HasOne<DetentionOrder>()
            .WithMany()
            .HasForeignKey(e => e.DetentionOrderId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
