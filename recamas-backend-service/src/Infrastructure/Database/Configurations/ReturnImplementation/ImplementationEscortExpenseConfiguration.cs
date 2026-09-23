using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RECAMAS.Domain.Entities.ReturnImplementation;

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
