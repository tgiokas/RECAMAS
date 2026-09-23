using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RECAMAS.Domain.Entities.TCNProfile;

namespace RECAMAS.Infrastructure.Database.Configurations;


public sealed class IpApplicationConfiguration : IEntityTypeConfiguration<IpApplication>
{
    public void Configure(EntityTypeBuilder<IpApplication> builder)
    {
        builder.ToTable("ip_applications", schema: "tcn_profile");
        EntityConfiguration.ConfigureBase(builder);
    }
}
