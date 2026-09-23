using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RECAMAS.Domain.Entities.Case;

namespace RECAMAS.Infrastructure.Database.Configurations;


public sealed class EscortExpenseConfiguration : IEntityTypeConfiguration<EscortExpense>
{
    public void Configure(EntityTypeBuilder<EscortExpense> builder)
    {
        builder.ToTable("escort_expenses", schema: "cases");
        EntityConfiguration.ConfigureBase(builder);
        builder.Property(e => e.ExpenseType).HasComment("Transport | Accommodation | MedicalEquipment | Other");
        builder.Property(e => e.ExpenseAmount).HasPrecision(18, 2);
    }
}
