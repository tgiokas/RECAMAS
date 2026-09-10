using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RECAMAS.Domain.Entities.TCNProfile;
using RECAMAS.Domain.Entities.Case;
using RECAMAS.Domain.Entities.Detention;
using RECAMAS.Domain.Entities.ReturnImplementation;
using RECAMAS.Domain.Entities.Admin;

namespace RECAMAS.Infrastructure.Database.Configurations;


public sealed class ReturnImplementationConfiguration : IEntityTypeConfiguration<ReturnImplementation>
{
    public void Configure(EntityTypeBuilder<ReturnImplementation> builder)
    {
        builder.ToTable("return_implementations", schema: "return_implementation");
        EntityConfiguration.ConfigureBase(builder);
        builder.Property(e => e.ImplementationId).HasComment("System-generated — string");
        builder.Property(e => e.ReturnCountryCode).HasComment("ISO 3166-1 alpha-2 — string (standard country code)");
        builder.Property(e => e.TransitCountryCodes).HasComment("JSON array of ISO codes — string[] serialized").HasColumnType("jsonb");
        builder.Property(e => e.OperationOwner).HasComment("FRONTEX | CyprusAuthorities | Other");
        builder.Property(e => e.FlightNumber).HasComment("Free text (airline format)");
        builder.Property(e => e.CancellationReason).HasComment("Free text");
        builder.HasIndex(e => e.ImplementationId).IsUnique();
    }
}
