using Microsoft.EntityFrameworkCore;

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

    // DbSets are added module-by-module as entities are defined, e.g.:
    // public DbSet<TCNProfile> TCNProfiles => Set<TCNProfile>();
    // public DbSet<Case> Cases => Set<Case>();
    // public DbSet<Rule> Rules => Set<Rule>();
    // public DbSet<RuleVersion> RuleVersions => Set<RuleVersion>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Applies every IEntityTypeConfiguration<T> found in this assembly —
        // each module adds its own configuration class instead of editing this file.
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        // TODO once entities exist: loop all BaseEntity-derived types and apply
        //   modelBuilder.Entity(type).HasQueryFilter(e => !e.IsDeleted)
        // via reflection here, so soft delete is automatic for every entity
        // without repeating HasQueryFilter in every single configuration class.

        // TODO: configure RowVersion to map onto Postgres' native "xmin" system
        // column for optimistic concurrency, e.g.:
        //   modelBuilder.Entity<Case>().Property(e => e.RowVersion).IsRowVersion();
    }
}
