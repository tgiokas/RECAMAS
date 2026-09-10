using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RECAMAS.Domain.Entities.TCNProfile;
using RECAMAS.Domain.Entities.Case;
using RECAMAS.Domain.Entities.Detention;
using RECAMAS.Domain.Entities.ReturnImplementation;
using RECAMAS.Domain.Entities.Admin;

namespace RECAMAS.Infrastructure.Database.Configurations;


public sealed class ImplementationTcnConfiguration : IEntityTypeConfiguration<ImplementationTcn>
{
    public void Configure(EntityTypeBuilder<ImplementationTcn> builder)
    {
        builder.ToTable("implementation_tcns", schema: "return_implementation");
        EntityConfiguration.ConfigureBase(builder);
        builder.Property(e => e.TravelAgency).HasComment("Free text");
        builder.Property(e => e.TicketCost).HasPrecision(18, 2);
        builder.Property(e => e.Airline).HasComment("Free text (airline name)");
        builder.Property(e => e.FlightNumber).HasComment("Free text (airline format)");
        builder.Property(e => e.DepartureAirportCode).HasComment("IATA code — string (standard)");
        builder.Property(e => e.DestinationCountryCode).HasComment("ISO 3166-1 alpha-2 — string");
        builder.Property(e => e.DestinationAirportCode).HasComment("IATA code — string");
        builder.Property(e => e.TicketNumber).HasComment("Free text (airline format)");
        builder.Property(e => e.EntryBanExpirationDate).HasComment("Calculated");
        builder.Property(e => e.UniqueEntryBanNumber).HasComment("Από Stoplist — free text (Police DB format)");
        builder.Property(e => e.MonetaryIncentiveApprovedAmount).HasPrecision(18, 2);
        builder.Property(e => e.MonetaryIncentiveAmountProvided).HasPrecision(18, 2);
        builder.Property(e => e.AssistanceDeadline).HasComment("Calculated: DepartureDate + 5 months");
    }
}
