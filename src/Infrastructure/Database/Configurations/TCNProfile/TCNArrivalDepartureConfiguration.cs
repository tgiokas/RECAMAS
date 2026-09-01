using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RECAMAS.Domain.Entities.TCNProfile;

namespace RECAMAS.Infrastructure.Database.Configurations.TCNProfile;

public class TCNArrivalDepartureConfiguration : IEntityTypeConfiguration<TCNArrivalDeparture>
{
    public void Configure(EntityTypeBuilder<TCNArrivalDeparture> builder)
    {
        builder.ToTable("tcn_arrivals_departures", schema: "tcn_profile");
        builder.HasKey(e => e.Id);
        builder.HasIndex(e => e.TCNProfileId);
    }
}
