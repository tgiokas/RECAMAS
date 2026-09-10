using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RECAMAS.Domain.Entities.TCNProfile;
using RECAMAS.Domain.Entities.Case;
using RECAMAS.Domain.Entities.Detention;
using RECAMAS.Domain.Entities.ReturnImplementation;
using RECAMAS.Domain.Entities.Admin;

namespace RECAMAS.Infrastructure.Database.Configurations;


public sealed class CaseRequestItemConfiguration : IEntityTypeConfiguration<CaseRequestItem>
{
    public void Configure(EntityTypeBuilder<CaseRequestItem> builder)
    {
        builder.ToTable("case_request_items", schema: "cases");
        EntityConfiguration.ConfigureBase(builder);
        builder.Property(e => e.IsReply).HasComment("False = initial, True = reply");
        builder.Property(e => e.Notes).HasComment("Κείμενο — free text");
    }
}
