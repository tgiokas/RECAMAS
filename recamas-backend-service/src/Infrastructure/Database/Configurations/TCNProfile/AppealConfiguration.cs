using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RECAMAS.Domain.Entities.TCNProfile;

namespace RECAMAS.Infrastructure.Database.Configurations;


public sealed class AppealConfiguration : IEntityTypeConfiguration<Appeal>
{
    public void Configure(EntityTypeBuilder<Appeal> builder)
    {
        builder.ToTable("appeals", schema: "tcn_profile");
        EntityConfiguration.ConfigureBase(builder);
        builder.Property(e => e.TypeOfAppeal).HasComment("AdministrativeCourt | Other");
        builder.Property(e => e.AppealNumber).HasComment("Μοναδικός αριθμός έφεσης — free text (αριθμός πρωτοκόλλου)");
    }
}
