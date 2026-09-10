using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RECAMAS.Domain.Entities.TCNProfile;
using RECAMAS.Domain.Entities.Case;
using RECAMAS.Domain.Entities.Detention;
using RECAMAS.Domain.Entities.ReturnImplementation;
using RECAMAS.Domain.Entities.Admin;

namespace RECAMAS.Infrastructure.Database.Configurations;


public sealed class TravelDocIssuanceApprovalItemConfiguration : IEntityTypeConfiguration<TravelDocIssuanceApprovalItem>
{
    public void Configure(EntityTypeBuilder<TravelDocIssuanceApprovalItem> builder)
    {
        builder.ToTable("travel_doc_issuance_approval_items", schema: "cases");
    }
}
