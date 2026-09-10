using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RECAMAS.Domain.Common;

namespace RECAMAS.Infrastructure.Database.Configurations;

internal static class EntityConfiguration
{
    public static void ConfigureBase<TEntity>(EntityTypeBuilder<TEntity> builder)
        where TEntity : BaseEntity
    {
        builder.HasKey(e => e.Id);
        builder.HasIndex(e => e.PublicId).IsUnique();
        builder.Property(e => e.Id).HasComment("PK — bigint / identity");
        builder.Property(e => e.PublicId).HasComment("Public identifier — UUIDv4 (§12.5.16)");
        builder.Property(e => e.CreatedAt).HasComment("Χρόνος δημιουργίας εγγραφής");
        builder.Property(e => e.CreatedByUserId).HasComment("User που δημιούργησε");
        builder.Property(e => e.UpdatedAt).HasComment("Χρόνος τελευταίας τροποποίησης");
        builder.Property(e => e.UpdatedByUserId).HasComment("User που τροποποίησε τελευταίος");
        builder.Property(e => e.IsDeleted).HasComment("Soft delete — δεν διαγράφεται ποτέ hard (§4.3.2)");
    }
}
