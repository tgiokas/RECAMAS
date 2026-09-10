using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RECAMAS.Domain.Entities.TCNProfile;
using RECAMAS.Domain.Entities.Case;
using RECAMAS.Domain.Entities.Detention;
using RECAMAS.Domain.Entities.ReturnImplementation;
using RECAMAS.Domain.Entities.Admin;

namespace RECAMAS.Infrastructure.Database.Configurations;


public sealed class DetentionRoomConfiguration : IEntityTypeConfiguration<DetentionRoom>
{
    public void Configure(EntityTypeBuilder<DetentionRoom> builder)
    {
        builder.ToTable("detention_rooms", schema: "detention");
        EntityConfiguration.ConfigureBase(builder);
        builder.Property(e => e.Name).HasComment("Αναγνωριστικό δωματίου — free text");
    }
}
