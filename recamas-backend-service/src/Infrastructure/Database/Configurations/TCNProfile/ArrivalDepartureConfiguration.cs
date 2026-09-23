using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RECAMAS.Domain.Entities.TCNProfile;

namespace RECAMAS.Infrastructure.Database.Configurations;


public sealed class ArrivalDepartureConfiguration : IEntityTypeConfiguration<ArrivalDeparture>
{
    public void Configure(EntityTypeBuilder<ArrivalDeparture> builder)
    {
        builder.ToTable("arrival_departures", schema: "tcn_profile");
        EntityConfiguration.ConfigureBase(builder);
        builder.Property(e => e.MovementType).HasComment("Arrival | Departure");
        builder.Property(e => e.AirportCode).HasComment("IATA airport code — string (standard code, πχ. \"LCA\")");
        builder.Property(e => e.PassportIssuingCountryCode).HasMaxLength(3);
        builder.Property(e => e.PassportNumber).HasMaxLength(50);
        builder.Property(e => e.VisaNumber).HasMaxLength(100);
        builder.Property(e => e.Source)
            .HasDefaultValue(RECAMAS.Domain.Enums.DataSourceType.PoliceDb)
            .HasSentinel((RECAMAS.Domain.Enums.DataSourceType)(-1));
        builder.HasIndex(e => new { e.Source, e.MovementType, e.ExternalRecordId })
            .IsUnique()
            .HasFilter("\"ExternalRecordId\" IS NOT NULL");
    }
}
