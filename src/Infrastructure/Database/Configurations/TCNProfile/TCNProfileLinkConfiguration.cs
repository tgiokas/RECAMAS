using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RECAMAS.Domain.Entities.TCNProfile;
using TCNProfileEntity = RECAMAS.Domain.Entities.TCNProfile.TCNProfile;

namespace RECAMAS.Infrastructure.Database.Configurations.TCNProfile;

public class TCNProfileLinkConfiguration : IEntityTypeConfiguration<TCNProfileLink>
{
    public void Configure(EntityTypeBuilder<TCNProfileLink> builder)
    {
        builder.ToTable("tcn_profile_links", schema: "tcn_profile");
        builder.HasKey(e => e.Id);

        builder.HasIndex(e => new { e.TCNProfileId, e.LinkedProfileId }).IsUnique();

        builder.ToTable(t => t.HasCheckConstraint(
            "CK_tcn_profile_links_not_self",
            "\"TCNProfileId\" <> \"LinkedProfileId\""));

        // The owning side (TCNProfileId) is configured in TCNProfileConfiguration
        // with Cascade. This second FK, to the same table, must be Restrict —
        // Postgres/EF Core rejects two cascade paths into the same table.
        builder.HasOne<TCNProfileEntity>()
            .WithMany()
            .HasForeignKey(e => e.LinkedProfileId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
