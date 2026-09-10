using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RECAMAS.Domain.Entities.TCNProfile;
using RECAMAS.Domain.Entities.Case;
using RECAMAS.Domain.Entities.Detention;
using RECAMAS.Domain.Entities.ReturnImplementation;
using RECAMAS.Domain.Entities.Admin;

namespace RECAMAS.Infrastructure.Database.Configurations;


public sealed class ReturnDecisionApprovalTcnConfiguration : IEntityTypeConfiguration<ReturnDecisionApprovalTcn>
{
    public void Configure(EntityTypeBuilder<ReturnDecisionApprovalTcn> builder)
    {
        builder.ToTable("return_decision_approval_tcns", schema: "cases");
        EntityConfiguration.ConfigureBase(builder);
    }
}
