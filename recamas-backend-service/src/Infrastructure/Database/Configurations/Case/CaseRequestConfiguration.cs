using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RECAMAS.Domain.Entities.TCNProfile;
using RECAMAS.Domain.Entities.Case;
using RECAMAS.Domain.Entities.Detention;
using RECAMAS.Domain.Entities.ReturnImplementation;
using RECAMAS.Domain.Entities.Admin;

namespace RECAMAS.Infrastructure.Database.Configurations;


public sealed class CaseRequestConfiguration : IEntityTypeConfiguration<CaseRequest>
{
    public void Configure(EntityTypeBuilder<CaseRequest> builder)
    {
        builder.ToTable("case_requests", schema: "cases");
        EntityConfiguration.ConfigureBase(builder);
        builder.Property(e => e.RequestId).HasComment("System-generated human-readable ID — string");
        builder.Property(e => e.RequestType).HasComment("ExpediteProcess | DocumentRequest | InformationRequest");
        builder.Property(e => e.RecipientType).HasComment("Role | User");
        builder.Property(e => e.Status).HasComment("Requested | Answered | Closed");
        builder.HasIndex(e => e.RequestId).IsUnique();
    }
}
