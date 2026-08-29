using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RECAMAS.Domain.Entities.TCNProfile;

namespace RECAMAS.Infrastructure.Persistence.Configurations.TCNProfile;

public class TCNNationalityConfiguration : IEntityTypeConfiguration<TCNNationality>
{
    public void Configure(EntityTypeBuilder<TCNNationality> builder)
    {
        builder.ToTable("tcn_nationalities", schema: "tcn_profile");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.NationalityCode).HasMaxLength(10);

        builder.HasIndex(e => e.TCNProfileId);
    }
}
