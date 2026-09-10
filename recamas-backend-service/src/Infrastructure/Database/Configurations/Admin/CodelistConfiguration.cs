using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RECAMAS.Domain.Entities.TCNProfile;
using RECAMAS.Domain.Entities.Case;
using RECAMAS.Domain.Entities.Detention;
using RECAMAS.Domain.Entities.ReturnImplementation;
using RECAMAS.Domain.Entities.Admin;

namespace RECAMAS.Infrastructure.Database.Configurations;


public sealed class CodelistConfiguration : IEntityTypeConfiguration<Codelist>
{
    public void Configure(EntityTypeBuilder<Codelist> builder)
    {
        builder.ToTable("codelists", schema: "admin");
        EntityConfiguration.ConfigureBase(builder);
        builder.Property(e => e.ListCode).HasComment("Μοναδικός κωδικός — string (πχ. \"COUNTRY\")");
        builder.Property(e => e.DisplayName).HasComment("String");
        builder.HasIndex(e => e.ListCode).IsUnique();
    }
}
