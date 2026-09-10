using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RECAMAS.Domain.Entities.TCNProfile;
using RECAMAS.Domain.Entities.Case;
using RECAMAS.Domain.Entities.Detention;
using RECAMAS.Domain.Entities.ReturnImplementation;
using RECAMAS.Domain.Entities.Admin;

namespace RECAMAS.Infrastructure.Database.Configurations;


public sealed class LinkedProfileConfiguration : IEntityTypeConfiguration<LinkedProfile>
{
    public void Configure(EntityTypeBuilder<LinkedProfile> builder)
    {
        builder.ToTable("linked_profiles", schema: "tcn_profile");
        EntityConfiguration.ConfigureBase(builder);
        builder.HasOne(e => e.FromTcnProfile)
            .WithMany(e => e.LinkedProfilesFrom)
            .HasForeignKey(e => e.FromTcnProfileId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(e => e.ToTcnProfile)
            .WithMany(e => e.LinkedProfilesTo)
            .HasForeignKey(e => e.ToTcnProfileId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.Property(e => e.Relationship).HasComment("Spouse | Child | Parent | Sibling | DuplicateMerged | Other");
        builder.Property(e => e.Notes).HasComment("Σημειώσεις — free text");
    }
}
