using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RECAMAS.Domain.Entities.TCNProfile;
using RECAMAS.Domain.Entities.Case;
using RECAMAS.Domain.Entities.Detention;
using RECAMAS.Domain.Entities.ReturnImplementation;
using RECAMAS.Domain.Entities.Admin;

namespace RECAMAS.Infrastructure.Database.Configurations;


public sealed class PostArrivalAssistanceConfiguration : IEntityTypeConfiguration<PostArrivalAssistance>
{
    public void Configure(EntityTypeBuilder<PostArrivalAssistance> builder)
    {
        builder.ToTable("post_arrival_assistances", schema: "return_implementation");
        EntityConfiguration.ConfigureBase(builder);
    }
}
