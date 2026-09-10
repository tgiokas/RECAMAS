using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RECAMAS.Domain.Entities.TCNProfile;
using RECAMAS.Domain.Entities.Case;
using RECAMAS.Domain.Entities.Detention;
using RECAMAS.Domain.Entities.ReturnImplementation;
using RECAMAS.Domain.Entities.Admin;

namespace RECAMAS.Infrastructure.Database.Configurations;


public sealed class AdditionalApproverConfiguration : IEntityTypeConfiguration<AdditionalApprover>
{
    public void Configure(EntityTypeBuilder<AdditionalApprover> builder)
    {
        builder.ToTable("additional_approvers", schema: "cases");
        EntityConfiguration.ConfigureBase(builder);
        builder.Property(e => e.OrderInChain).HasComment("Σειρά στο chain");
    }
}
