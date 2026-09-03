using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TCNProfileEntity = RECAMAS.Domain.Entities.TCNProfile.TCNProfile;

namespace RECAMAS.Infrastructure.Database.Configurations;

public class TCNProfileConfiguration : IEntityTypeConfiguration<TCNProfileEntity>
{
    public void Configure(EntityTypeBuilder<TCNProfileEntity> builder)
    {
        builder.ToTable("tcn_profiles", schema: "tcn_profile");

        builder.HasKey(e => e.Id);

        builder.HasIndex(e => e.PublicId).IsUnique();

        // Partial unique index: DisplayCode is null until the Application layer
        // assigns one on creation (see TCNProfile.DisplayCode remarks).
        builder.HasIndex(e => e.DisplayCode)
            .IsUnique()
            .HasFilter("\"DisplayCode\" IS NOT NULL");

        builder.HasIndex(e => e.Arc);

        // Fuzzy search on ARC and name fields per architecture diagram
        // ("+ GIN (TCN name/ARC fuzzy search)") — requires pg_trgm, enabled in
        // ApplicationDbContext.OnModelCreating.
        builder.HasIndex(e => new { e.Arc, e.FirstNameEn, e.LastNameEn })
            .HasMethod("gin")
            .HasOperators("gin_trgm_ops", "gin_trgm_ops", "gin_trgm_ops");

        builder.HasMany(e => e.Nationalities)
            .WithOne()
            .HasForeignKey(e => e.TCNProfileId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(e => e.IdentityDocuments)
            .WithOne()
            .HasForeignKey(e => e.TCNProfileId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(e => e.ResidencyStatuses)
            .WithOne()
            .HasForeignKey(e => e.TCNProfileId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(e => e.ResidencyApplications)
            .WithOne()
            .HasForeignKey(e => e.TCNProfileId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(e => e.InternationalProtectionStatuses)
            .WithOne()
            .HasForeignKey(e => e.TCNProfileId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(e => e.InternationalProtectionApplications)
            .WithOne()
            .HasForeignKey(e => e.TCNProfileId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(e => e.Appeals)
            .WithOne()
            .HasForeignKey(e => e.TCNProfileId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(e => e.ReturnDecisions)
            .WithOne()
            .HasForeignKey(e => e.TCNProfileId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(e => e.StoplistEntries)
            .WithOne()
            .HasForeignKey(e => e.TCNProfileId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(e => e.ArrivalsDepartures)
            .WithOne()
            .HasForeignKey(e => e.TCNProfileId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(e => e.SecurityFindings)
            .WithOne()
            .HasForeignKey(e => e.TCNProfileId)
            .OnDelete(DeleteBehavior.Cascade);

        // Only the owning side (TCNProfileId) cascades — see TCNProfileLinkConfiguration
        // for why the LinkedProfileId side is Restrict instead.
        builder.HasMany(e => e.Links)
            .WithOne()
            .HasForeignKey(e => e.TCNProfileId)
            .OnDelete(DeleteBehavior.Cascade);

        // Age is computed from DateOfBirth, never persisted.
        builder.Ignore(e => e.Age);
    }
}
