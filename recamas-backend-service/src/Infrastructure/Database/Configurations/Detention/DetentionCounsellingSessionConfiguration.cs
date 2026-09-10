using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RECAMAS.Domain.Entities.TCNProfile;
using RECAMAS.Domain.Entities.Case;
using RECAMAS.Domain.Entities.Detention;
using RECAMAS.Domain.Entities.ReturnImplementation;
using RECAMAS.Domain.Entities.Admin;

namespace RECAMAS.Infrastructure.Database.Configurations;


public sealed class DetentionCounsellingSessionConfiguration : IEntityTypeConfiguration<DetentionCounsellingSession>
{
    public void Configure(EntityTypeBuilder<DetentionCounsellingSession> builder)
    {
        builder.ToTable("detention_counselling_sessions", schema: "detention");
        EntityConfiguration.ConfigureBase(builder);
        builder.Property(e => e.SessionId).HasComment("System-generated — string");
        builder.Property(e => e.CounsellorType).HasComment("AIUOfficer | FRONTEX | Other");
        builder.Property(e => e.Status).HasComment("InProgress | Completed");
        builder.HasIndex(e => e.SessionId).IsUnique();
    }
}
