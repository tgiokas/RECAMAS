using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RECAMAS.Domain.Entities.TCNProfile;
using RECAMAS.Domain.Entities.Case;
using RECAMAS.Domain.Entities.Detention;
using RECAMAS.Domain.Entities.ReturnImplementation;
using RECAMAS.Domain.Entities.Admin;

namespace RECAMAS.Infrastructure.Database.Configurations;


public sealed class TcnNationalityConfiguration : IEntityTypeConfiguration<TcnNationality>
{
    public void Configure(EntityTypeBuilder<TcnNationality> builder)
    {
        builder.ToTable("tcn_nationalities", schema: "tcn_profile");
        EntityConfiguration.ConfigureBase(builder);
        builder.Property(e => e.TcnProfileId).HasComment("FK → TcnProfile");
        builder.Property(e => e.CountryCode).HasComment("ISO 3166-1 alpha-2 — string γιατί είναι standard κωδικός (πχ. \"CY\", \"GR\")");
        builder.Property(e => e.IsPrimary).HasComment("Κύρια εθνικότητα για reporting");
        builder.Property(e => e.IdentificationStatus).HasComment("Confirmed | Claimed | Unknown");
    }
}
