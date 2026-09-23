using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RECAMAS.Domain.Entities.Case;

namespace RECAMAS.Infrastructure.Database.Configurations;


public sealed class ByOwnCaseConfiguration : IEntityTypeConfiguration<ByOwnCase>
{
    public void Configure(EntityTypeBuilder<ByOwnCase> builder)
    {
        builder.ToTable("by_own_cases", schema: "cases");
    }
}
