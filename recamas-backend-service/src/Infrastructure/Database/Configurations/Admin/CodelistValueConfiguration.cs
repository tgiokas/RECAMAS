using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RECAMAS.Domain.Entities.TCNProfile;
using RECAMAS.Domain.Entities.Case;
using RECAMAS.Domain.Entities.Detention;
using RECAMAS.Domain.Entities.ReturnImplementation;
using RECAMAS.Domain.Entities.Admin;

namespace RECAMAS.Infrastructure.Database.Configurations;


public sealed class CodelistValueConfiguration : IEntityTypeConfiguration<CodelistValue>
{
    public void Configure(EntityTypeBuilder<CodelistValue> builder)
    {
        builder.ToTable("codelist_values", schema: "admin");
        EntityConfiguration.ConfigureBase(builder);
        builder.Property(e => e.Code).HasComment("Μοναδικός κωδικός τιμής — string");
    }
}
