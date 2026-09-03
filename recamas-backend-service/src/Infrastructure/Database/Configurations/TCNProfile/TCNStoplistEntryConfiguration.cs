using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RECAMAS.Domain.Entities.TCNProfile;

namespace RECAMAS.Infrastructure.Database.Configurations;

public class TCNStoplistEntryConfiguration : IEntityTypeConfiguration<TCNStoplistEntry>
{
    public void Configure(EntityTypeBuilder<TCNStoplistEntry> builder)
    {
        builder.ToTable("tcn_stoplist_entries", schema: "tcn_profile");
        builder.HasKey(e => e.Id);
        builder.HasIndex(e => e.TCNProfileId);
        builder.HasIndex(e => e.UniqueEntryBanNumber);
    }
}
