using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RECAMAS.Domain.Entities.Case;

namespace RECAMAS.Infrastructure.Database.Configurations;


public sealed class ByOwnReturnDecisionTcnConfiguration : IEntityTypeConfiguration<ByOwnReturnDecisionTcn>
{
    public void Configure(EntityTypeBuilder<ByOwnReturnDecisionTcn> builder)
    {
        builder.ToTable("by_own_return_decision_tcns", schema: "cases");
        EntityConfiguration.ConfigureBase(builder);
    }
}
