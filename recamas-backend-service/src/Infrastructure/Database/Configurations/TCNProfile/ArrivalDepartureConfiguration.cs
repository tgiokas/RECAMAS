using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RECAMAS.Domain.Entities.TCNProfile;
using RECAMAS.Domain.Entities.Case;
using RECAMAS.Domain.Entities.Detention;
using RECAMAS.Domain.Entities.ReturnImplementation;
using RECAMAS.Domain.Entities.Admin;

namespace RECAMAS.Infrastructure.Database.Configurations;


public sealed class ArrivalDepartureConfiguration : IEntityTypeConfiguration<ArrivalDeparture>
{
    public void Configure(EntityTypeBuilder<ArrivalDeparture> builder)
    {
        builder.ToTable("arrival_departures", schema: "tcn_profile");
        EntityConfiguration.ConfigureBase(builder);
        builder.Property(e => e.MovementType).HasComment("Arrival | Departure");
        builder.Property(e => e.AirportCode).HasComment("IATA airport code — string (standard code, πχ. \"LCA\")");
    }
}
