using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RECAMAS.Domain.Entities.TCNProfile;
using RECAMAS.Domain.Entities.Case;
using RECAMAS.Domain.Entities.Detention;
using RECAMAS.Domain.Entities.ReturnImplementation;
using RECAMAS.Domain.Entities.Admin;

namespace RECAMAS.Infrastructure.Database.Configurations;


public sealed class CounsellingChildConfiguration : IEntityTypeConfiguration<CounsellingChild>
{
    public void Configure(EntityTypeBuilder<CounsellingChild> builder)
    {
        builder.ToTable("counselling_children", schema: "cases");
        EntityConfiguration.ConfigureBase(builder);
        builder.Property(e => e.PlaceOfBirth).HasComment("Χώρα / πόλη γέννησης — free text");
    }
}
