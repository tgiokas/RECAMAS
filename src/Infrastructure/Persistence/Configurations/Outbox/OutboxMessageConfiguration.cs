using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RECAMAS.Domain.Entities.Outbox;

namespace RECAMAS.Infrastructure.Persistence.Configurations.Outbox;

public class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.ToTable("outbox_messages", schema: "outbox");

        builder.HasKey(e => e.Id);

        // The only query OutboxProcessor ever runs — matches its
        // ProcessedAt/AttemptCount/OccurredAt filter+order exactly.
        builder.HasIndex(e => new { e.ProcessedAt, e.AttemptCount, e.OccurredAt });

        builder.Property(e => e.EventType).HasMaxLength(200);
        builder.Property(e => e.Category).HasMaxLength(50);
        builder.Property(e => e.Key).HasMaxLength(200);
    }
}
