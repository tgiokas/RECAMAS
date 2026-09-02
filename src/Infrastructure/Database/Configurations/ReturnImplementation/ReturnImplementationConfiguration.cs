using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CaseEntity = RECAMAS.Domain.Entities.Case.Case;
using ReturnImplementationEntity = RECAMAS.Domain.Entities.ReturnImplementation.ReturnImplementation;

namespace RECAMAS.Infrastructure.Database.Configurations;

public class ReturnImplementationConfiguration : IEntityTypeConfiguration<ReturnImplementationEntity>
{
    public void Configure(EntityTypeBuilder<ReturnImplementationEntity> builder)
    {
        builder.ToTable("return_implementations", schema: "return_impl");
        builder.HasKey(e => e.Id);

        builder.HasIndex(e => e.CaseId);

        builder.HasOne<CaseEntity>()
            .WithMany()
            .HasForeignKey(e => e.CaseId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
