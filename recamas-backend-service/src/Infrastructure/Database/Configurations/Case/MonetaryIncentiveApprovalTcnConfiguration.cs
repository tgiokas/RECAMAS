using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RECAMAS.Domain.Entities.Case;

namespace RECAMAS.Infrastructure.Database.Configurations;


public sealed class MonetaryIncentiveApprovalTcnConfiguration : IEntityTypeConfiguration<MonetaryIncentiveApprovalTcn>
{
    public void Configure(EntityTypeBuilder<MonetaryIncentiveApprovalTcn> builder)
    {
        builder.ToTable("monetary_incentive_approval_tcns", schema: "cases");
        EntityConfiguration.ConfigureBase(builder);
    }
}
