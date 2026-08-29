using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using RECAMAS.Domain.Common;

namespace RECAMAS.Infrastructure.Persistence;

/// <summary>
/// Single PostgreSQL 18 instance, one schema per module:
///   tcn_profile | case | detention | return_impl | reports | rules
///
/// Each module's entity configurations (IEntityTypeConfiguration&lt;T&gt;) go in
/// a matching subfolder here, e.g. Persistence/Configurations/Case/CaseConfiguration.cs,
/// and call builder.ToTable("cases", schema: "case"). Keeping every module's
/// migrations scoped to its own schema is what keeps EF migrations from
/// constantly conflicting across developers working in different modules.
///
/// Soft delete: a global query filter is applied per-entity below so
/// IsDeleted=true rows are invisible by default — repositories don't need
/// to remember to filter them out manually. Use IgnoreQueryFilters() explicitly
/// on the rare admin/audit query that needs to see deleted rows.
/// </summary>
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Domain.Entities.TCNProfile.TCNProfile> TCNProfiles => Set<Domain.Entities.TCNProfile.TCNProfile>();
    public DbSet<Domain.Entities.TCNProfile.TCNNationality> TCNNationalities => Set<Domain.Entities.TCNProfile.TCNNationality>();
    public DbSet<Domain.Entities.TCNProfile.TCNIdentityDocument> TCNIdentityDocuments => Set<Domain.Entities.TCNProfile.TCNIdentityDocument>();
    // Further DbSets are added module-by-module as entities are defined, e.g.:
    // public DbSet<Case> Cases => Set<Case>();
    // public DbSet<Rule> Rules => Set<Rule>();
    // public DbSet<RuleVersion> RuleVersions => Set<RuleVersion>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // pg_trgm backs the GIN fuzzy-search indexes used for TCN name/ARC search
        // (architecture diagram: "+ GIN (TCN name/ARC fuzzy search)"). Trusted
        // extension since Postgres 13 — installable by the database owner, no
        // superuser required.
        modelBuilder.HasPostgresExtension("pg_trgm");

        // Applies every IEntityTypeConfiguration<T> found in this assembly —
        // each module adds its own configuration class instead of editing this file.
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        // Soft delete (IsDeleted) and optimistic concurrency (RowVersion -> Postgres'
        // native "xmin" system column) apply the same way to every entity, so they're
        // wired once here via reflection instead of being repeated per configuration.
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (!typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
            {
                continue;
            }

            var parameter = Expression.Parameter(entityType.ClrType, "e");
            var isDeletedProperty = Expression.Call(
                typeof(EF), nameof(EF.Property), [typeof(bool)],
                parameter, Expression.Constant(nameof(BaseEntity.IsDeleted)));
            var notDeleted = Expression.Lambda(Expression.Not(isDeletedProperty), parameter);
            modelBuilder.Entity(entityType.ClrType).HasQueryFilter(notDeleted);

            modelBuilder.Entity(entityType.ClrType)
                .Property(nameof(BaseEntity.RowVersion))
                .HasColumnName("xmin")
                .HasColumnType("xid")
                .ValueGeneratedOnAddOrUpdate()
                .IsRowVersion();
        }
    }
}
