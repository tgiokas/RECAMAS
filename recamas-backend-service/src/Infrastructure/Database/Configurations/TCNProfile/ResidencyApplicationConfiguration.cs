using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RECAMAS.Domain.Entities.TCNProfile;
using RECAMAS.Domain.Entities.Case;
using RECAMAS.Domain.Entities.Detention;
using RECAMAS.Domain.Entities.ReturnImplementation;
using RECAMAS.Domain.Entities.Admin;

namespace RECAMAS.Infrastructure.Database.Configurations;


public sealed class ResidencyApplicationConfiguration : IEntityTypeConfiguration<ResidencyApplication>
{
    public void Configure(EntityTypeBuilder<ResidencyApplication> builder)
    {
        builder.ToTable("residency_applications", schema: "tcn_profile");
        EntityConfiguration.ConfigureBase(builder);
        builder.Property(e => e.TypeOfPermitRequested).HasComment("Τύπος αιτούμενης άδειας");
        builder.Property(e => e.TypeOfApplication).HasComment("Initial | Renewal | Replacement");
        builder.Property(e => e.PurposeRnd).HasComment("RND code — free text, ARS-specific");
        builder.Property(e => e.Status).HasComment("Pending | Approved | Rejected");
    }
}
