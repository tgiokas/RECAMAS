using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RECAMAS.Domain.Entities.TCNProfile;
using RECAMAS.Domain.Entities.Case;
using RECAMAS.Domain.Entities.Detention;
using RECAMAS.Domain.Entities.ReturnImplementation;
using RECAMAS.Domain.Entities.Admin;

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
