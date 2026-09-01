using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RECAMAS.Domain.Entities.TCNProfile;

namespace RECAMAS.Infrastructure.Database.Configurations.TCNProfile;

public class TCNIdentityDocumentConfiguration : IEntityTypeConfiguration<TCNIdentityDocument>
{
    public void Configure(EntityTypeBuilder<TCNIdentityDocument> builder)
    {
        builder.ToTable("tcn_identity_documents", schema: "tcn_profile");

        builder.HasKey(e => e.Id);

        builder.HasIndex(e => e.TCNProfileId);
        builder.HasIndex(e => e.DocumentNumber);
    }
}
