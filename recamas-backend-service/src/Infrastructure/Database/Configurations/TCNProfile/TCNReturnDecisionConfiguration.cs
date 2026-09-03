using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RECAMAS.Domain.Entities.TCNProfile;

namespace RECAMAS.Infrastructure.Database.Configurations;

public class TCNReturnDecisionConfiguration : IEntityTypeConfiguration<TCNReturnDecision>
{
    public void Configure(EntityTypeBuilder<TCNReturnDecision> builder)
    {
        builder.ToTable("tcn_return_decisions", schema: "tcn_profile");
        builder.HasKey(e => e.Id);
        builder.HasIndex(e => e.TCNProfileId);
    }
}
