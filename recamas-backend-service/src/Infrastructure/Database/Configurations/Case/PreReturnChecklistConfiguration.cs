using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RECAMAS.Domain.Entities.TCNProfile;
using RECAMAS.Domain.Entities.Case;
using RECAMAS.Domain.Entities.Detention;
using RECAMAS.Domain.Entities.ReturnImplementation;
using RECAMAS.Domain.Entities.Admin;

namespace RECAMAS.Infrastructure.Database.Configurations;


public sealed class PreReturnChecklistConfiguration : IEntityTypeConfiguration<PreReturnChecklist>
{
    public void Configure(EntityTypeBuilder<PreReturnChecklist> builder)
    {
        builder.ToTable("pre_return_checklists", schema: "cases");
        EntityConfiguration.ConfigureBase(builder);
        builder.HasOne(e => e.Case)
            .WithOne()
            .HasForeignKey<PreReturnChecklist>(e => e.CaseId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.Property(e => e.CaseId).HasComment("1:1 με ReturnCase");
    }
}
