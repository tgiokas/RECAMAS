using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RECAMAS.Domain.Entities.TCNProfile;
using RECAMAS.Domain.Entities.Case;
using RECAMAS.Domain.Entities.Detention;
using RECAMAS.Domain.Entities.ReturnImplementation;
using RECAMAS.Domain.Entities.Admin;

namespace RECAMAS.Infrastructure.Database.Configurations;


public sealed class ImplementationOtherExpenseConfiguration : IEntityTypeConfiguration<ImplementationOtherExpense>
{
    public void Configure(EntityTypeBuilder<ImplementationOtherExpense> builder)
    {
        builder.ToTable("implementation_other_expenses", schema: "return_implementation");
        EntityConfiguration.ConfigureBase(builder);
        builder.Property(e => e.ApprovedExpenseAmount).HasComment("Από Case").HasPrecision(18, 2);
        builder.Property(e => e.ActualExpenseAmount).HasComment("Εισάγεται κατά implementation").HasPrecision(18, 2);
    }
}
