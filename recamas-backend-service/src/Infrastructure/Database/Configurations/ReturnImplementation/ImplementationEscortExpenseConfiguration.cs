using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RECAMAS.Domain.Entities.TCNProfile;
using RECAMAS.Domain.Entities.Case;
using RECAMAS.Domain.Entities.Detention;
using RECAMAS.Domain.Entities.ReturnImplementation;
using RECAMAS.Domain.Entities.Admin;

namespace RECAMAS.Infrastructure.Database.Configurations;


public sealed class ImplementationEscortExpenseConfiguration : IEntityTypeConfiguration<ImplementationEscortExpense>
{
    public void Configure(EntityTypeBuilder<ImplementationEscortExpense> builder)
    {
        builder.ToTable("implementation_escort_expenses", schema: "return_implementation");
        EntityConfiguration.ConfigureBase(builder);
        builder.Property(e => e.ExpenseAmount).HasPrecision(18, 2);
    }
}
