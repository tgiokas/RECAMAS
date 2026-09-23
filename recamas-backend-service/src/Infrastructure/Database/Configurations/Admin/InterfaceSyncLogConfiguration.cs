using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RECAMAS.Domain.Entities.Admin;

namespace RECAMAS.Infrastructure.Database.Configurations;


public sealed class InterfaceSyncLogConfiguration : IEntityTypeConfiguration<InterfaceSyncLog>
{
    public void Configure(EntityTypeBuilder<InterfaceSyncLog> builder)
    {
        builder.ToTable("interface_sync_logs", schema: "admin");
        EntityConfiguration.ConfigureBase(builder);
        builder.Property(e => e.Operation).HasMaxLength(100);
        builder.Property(e => e.CorrelationId).HasMaxLength(200);
        builder.Property(e => e.ErrorCode).HasMaxLength(200);
        builder.Property(e => e.ExternalReference).HasMaxLength(500);
        builder.Property(e => e.RequestPayloadHash).HasMaxLength(64);
        builder.Property(e => e.ResponsePayloadHash).HasMaxLength(64);
        builder.Property(e => e.ErrorMessage).HasComment("Free text (technical message)");
        builder.HasIndex(e => e.CorrelationId);
        builder.HasIndex(e => new { e.ExternalSystem, e.StartedAt });
        builder.HasIndex(e => new { e.Status, e.StartedAt });
    }
}
