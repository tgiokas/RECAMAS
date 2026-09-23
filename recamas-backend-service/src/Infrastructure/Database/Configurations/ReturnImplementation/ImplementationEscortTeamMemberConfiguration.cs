using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RECAMAS.Domain.Entities.ReturnImplementation;

namespace RECAMAS.Infrastructure.Database.Configurations;


public sealed class ImplementationEscortTeamMemberConfiguration : IEntityTypeConfiguration<ImplementationEscortTeamMember>
{
    public void Configure(EntityTypeBuilder<ImplementationEscortTeamMember> builder)
    {
        builder.ToTable("implementation_escort_team_members", schema: "return_implementation");
        EntityConfiguration.ConfigureBase(builder);
        builder.Property(e => e.EscortUserId).HasComment("Αναγνωριστικό χρήστη από το εξωτερικό identity system");
        builder.Property(e => e.ExternalEscortName).HasComment("Free text (αν εξωτερικός)");
    }
}
