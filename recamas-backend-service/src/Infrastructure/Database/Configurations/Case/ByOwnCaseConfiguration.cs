using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RECAMAS.Domain.Entities.TCNProfile;
using RECAMAS.Domain.Entities.Case;
using RECAMAS.Domain.Entities.Detention;
using RECAMAS.Domain.Entities.ReturnImplementation;
using RECAMAS.Domain.Entities.Admin;

namespace RECAMAS.Infrastructure.Database.Configurations;


public sealed class ByOwnCaseConfiguration : IEntityTypeConfiguration<ByOwnCase>
{
    public void Configure(EntityTypeBuilder<ByOwnCase> builder)
    {
        builder.ToTable("by_own_cases", schema: "cases");
    }
}
