using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RECAMAS.Domain.Entities.TCNProfile;
using RECAMAS.Domain.Entities.Case;
using RECAMAS.Domain.Entities.Detention;
using RECAMAS.Domain.Entities.ReturnImplementation;
using RECAMAS.Domain.Entities.Admin;

namespace RECAMAS.Infrastructure.Database.Configurations;


public sealed class InterfaceSyncLogConfiguration : IEntityTypeConfiguration<InterfaceSyncLog>
{
    public void Configure(EntityTypeBuilder<InterfaceSyncLog> builder)
    {
        builder.ToTable("interface_sync_logs", schema: "admin");
        EntityConfiguration.ConfigureBase(builder);
        builder.Property(e => e.ErrorMessage).HasComment("Free text (technical message)");
    }
}
