using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RECAMAS.Domain.Entities.Detention;

namespace RECAMAS.Infrastructure.Database.Configurations;


public sealed class TemporaryCheckoutConfiguration : IEntityTypeConfiguration<TemporaryCheckout>
{
    public void Configure(EntityTypeBuilder<TemporaryCheckout> builder)
    {
        builder.ToTable("temporary_checkouts", schema: "detention");
        EntityConfiguration.ConfigureBase(builder);
        builder.Property(e => e.CheckoutReason).HasComment("Medical | DoctorVisit | Embassy | Other");
        builder.Property(e => e.Notes).HasComment("Free text");
    }
}
