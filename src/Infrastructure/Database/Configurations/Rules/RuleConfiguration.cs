using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RECAMAS.Domain.Entities.Rules;

namespace RECAMAS.Infrastructure.Database.Configurations;

public class RuleConfiguration : IEntityTypeConfiguration<Rule>
{
    public void Configure(EntityTypeBuilder<Rule> builder)
    {
        builder.ToTable("rules", schema: "rules");
        builder.HasKey(e => e.Id);

        builder.HasMany(e => e.Versions)
            .WithOne()
            .HasForeignKey(e => e.RuleId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
