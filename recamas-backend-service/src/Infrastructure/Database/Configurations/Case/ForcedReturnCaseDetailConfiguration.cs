using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RECAMAS.Domain.Entities.Case;

namespace RECAMAS.Infrastructure.Database.Configurations;

public class ForcedReturnCaseDetailConfiguration : IEntityTypeConfiguration<ForcedReturnCaseDetail>
{
    public void Configure(EntityTypeBuilder<ForcedReturnCaseDetail> builder)
    {
        builder.ToTable("forced_return_case_details", schema: "case");
        builder.HasKey(e => e.Id);
        builder.HasIndex(e => e.CaseId).IsUnique();
    }
}
