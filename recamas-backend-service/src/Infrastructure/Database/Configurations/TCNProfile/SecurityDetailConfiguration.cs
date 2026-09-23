using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RECAMAS.Domain.Entities.TCNProfile;

namespace RECAMAS.Infrastructure.Database.Configurations;


public sealed class SecurityDetailConfiguration : IEntityTypeConfiguration<SecurityDetail>
{
    public void Configure(EntityTypeBuilder<SecurityDetail> builder)
    {
        builder.ToTable("security_details", schema: "tcn_profile");
        EntityConfiguration.ConfigureBase(builder);
    }
}
