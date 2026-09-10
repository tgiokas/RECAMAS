using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RECAMAS.Domain.Entities.TCNProfile;
using RECAMAS.Domain.Entities.Case;
using RECAMAS.Domain.Entities.Detention;
using RECAMAS.Domain.Entities.ReturnImplementation;
using RECAMAS.Domain.Entities.Admin;

namespace RECAMAS.Infrastructure.Database.Configurations;


public sealed class IpApplicationConfiguration : IEntityTypeConfiguration<IpApplication>
{
    public void Configure(EntityTypeBuilder<IpApplication> builder)
    {
        builder.ToTable("ip_applications", schema: "tcn_profile");
        EntityConfiguration.ConfigureBase(builder);
    }
}
