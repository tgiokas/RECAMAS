using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RECAMAS.Domain.Entities.Case;

namespace RECAMAS.Infrastructure.Database.Configurations.Case;

public class VoluntaryReturnOwnMeansCaseDetailConfiguration : IEntityTypeConfiguration<VoluntaryReturnOwnMeansCaseDetail>
{
    public void Configure(EntityTypeBuilder<VoluntaryReturnOwnMeansCaseDetail> builder)
    {
        builder.ToTable("voluntary_return_own_means_case_details", schema: "case");
        builder.HasKey(e => e.Id);
        builder.HasIndex(e => e.CaseId).IsUnique();
    }
}
