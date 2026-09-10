using System.Linq.Expressions;
using Cbs.Audit.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using RECAMAS.Application.Interfaces;
using RECAMAS.Domain.Common;
using RECAMAS.Domain.Entities.TCNProfile;
using RECAMAS.Domain.Entities.Case;
using RECAMAS.Domain.Entities.Detention;
using RECAMAS.Domain.Entities.ReturnImplementation;
using RECAMAS.Domain.Entities.Admin;

namespace RECAMAS.Infrastructure.Database;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<TcnProfile> TcnProfiles => Set<TcnProfile>();
    public DbSet<TcnNationality> TcnNationalities => Set<TcnNationality>();
    public DbSet<IdentityDocument> IdentityDocuments => Set<IdentityDocument>();
    public DbSet<ResidencyStatus> ResidencyStatuses => Set<ResidencyStatus>();
    public DbSet<ResidencyApplication> ResidencyApplications => Set<ResidencyApplication>();
    public DbSet<IpStatus> IpStatuses => Set<IpStatus>();
    public DbSet<IpApplication> IpApplications => Set<IpApplication>();
    public DbSet<Appeal> Appeals => Set<Appeal>();
    public DbSet<ReturnDecision> ReturnDecisions => Set<ReturnDecision>();
    public DbSet<StoplistEntry> StoplistEntries => Set<StoplistEntry>();
    public DbSet<ArrivalDeparture> ArrivalDepartures => Set<ArrivalDeparture>();
    public DbSet<SecurityDetail> SecurityDetails => Set<SecurityDetail>();
    public DbSet<SecurityFinding> SecurityFindings => Set<SecurityFinding>();
    public DbSet<LinkedProfile> LinkedProfiles => Set<LinkedProfile>();
    public DbSet<ReturnCase> ReturnCases => Set<ReturnCase>();
    public DbSet<AvrCase> AvrCases => Set<AvrCase>();
    public DbSet<ForcedReturnCase> ForcedReturnCases => Set<ForcedReturnCase>();
    public DbSet<ByOwnCase> ByOwnCases => Set<ByOwnCase>();
    public DbSet<CaseTcn> CaseTcns => Set<CaseTcn>();
    public DbSet<CaseTravelDocument> CaseTravelDocuments => Set<CaseTravelDocument>();
    public DbSet<CaseReturnDecision> CaseReturnDecisions => Set<CaseReturnDecision>();
    public DbSet<VulnerabilityIssue> VulnerabilityIssues => Set<VulnerabilityIssue>();
    public DbSet<CounsellingSession> CounsellingSessions => Set<CounsellingSession>();
    public DbSet<CounsellingChild> CounsellingChildren => Set<CounsellingChild>();
    public DbSet<CaseAssignment> CaseAssignments => Set<CaseAssignment>();
    public DbSet<CaseRequest> CaseRequests => Set<CaseRequest>();
    public DbSet<CaseRequestItem> CaseRequestItems => Set<CaseRequestItem>();
    public DbSet<CaseHistoryEntry> CaseHistoryEntries => Set<CaseHistoryEntry>();
    public DbSet<ApprovalItem> ApprovalItems => Set<ApprovalItem>();
    public DbSet<ReturnDecisionApprovalItem> ReturnDecisionApprovalItems => Set<ReturnDecisionApprovalItem>();
    public DbSet<ReturnDecisionApprovalTcn> ReturnDecisionApprovalTcns => Set<ReturnDecisionApprovalTcn>();
    public DbSet<TravelDocIssuanceApprovalItem> TravelDocIssuanceApprovalItems => Set<TravelDocIssuanceApprovalItem>();
    public DbSet<TravelDocIssuanceApprovalTcn> TravelDocIssuanceApprovalTcns => Set<TravelDocIssuanceApprovalTcn>();
    public DbSet<EntryBanApprovalItem> EntryBanApprovalItems => Set<EntryBanApprovalItem>();
    public DbSet<EscortApprovalItem> EscortApprovalItems => Set<EscortApprovalItem>();
    public DbSet<EscortExpense> EscortExpenses => Set<EscortExpense>();
    public DbSet<OtherExpenseApprovalItem> OtherExpenseApprovalItems => Set<OtherExpenseApprovalItem>();
    public DbSet<MonetaryIncentiveApprovalItem> MonetaryIncentiveApprovalItems => Set<MonetaryIncentiveApprovalItem>();
    public DbSet<MonetaryIncentiveApprovalTcn> MonetaryIncentiveApprovalTcns => Set<MonetaryIncentiveApprovalTcn>();
    public DbSet<FrcOrderApprovalItem> FrcOrderApprovalItems => Set<FrcOrderApprovalItem>();
    public DbSet<AlternativeMeasureApprovalItem> AlternativeMeasureApprovalItems => Set<AlternativeMeasureApprovalItem>();
    public DbSet<AdditionalApprover> AdditionalApprovers => Set<AdditionalApprover>();
    public DbSet<AdjustmentNote> AdjustmentNotes => Set<AdjustmentNote>();
    public DbSet<TravelDocumentIssuance> TravelDocumentIssuances => Set<TravelDocumentIssuance>();
    public DbSet<ByOwnReturnDecisionIssuance> ByOwnReturnDecisionIssuances => Set<ByOwnReturnDecisionIssuance>();
    public DbSet<ByOwnReturnDecisionTcn> ByOwnReturnDecisionTcns => Set<ByOwnReturnDecisionTcn>();
    public DbSet<PreReturnChecklist> PreReturnChecklists => Set<PreReturnChecklist>();
    public DbSet<CaseDocument> CaseDocuments => Set<CaseDocument>();
    public DbSet<DetentionCenter> DetentionCenters => Set<DetentionCenter>();
    public DbSet<DetentionWing> DetentionWings => Set<DetentionWing>();
    public DbSet<DetentionRoom> DetentionRooms => Set<DetentionRoom>();
    public DbSet<DetentionRecord> DetentionRecords => Set<DetentionRecord>();
    public DbSet<TemporaryCheckout> TemporaryCheckouts => Set<TemporaryCheckout>();
    public DbSet<DetentionReassessment> DetentionReassessments => Set<DetentionReassessment>();
    public DbSet<DetentionCounsellingSession> DetentionCounsellingSessions => Set<DetentionCounsellingSession>();
    public DbSet<DetentionUpdateIssue> DetentionUpdateIssues => Set<DetentionUpdateIssue>();
    public DbSet<ReturnImplementation> ReturnImplementations => Set<ReturnImplementation>();
    public DbSet<ImplementationTcn> ImplementationTcns => Set<ImplementationTcn>();
    public DbSet<PostArrivalAssistance> PostArrivalAssistances => Set<PostArrivalAssistance>();
    public DbSet<ImplementationEscortTeamMember> ImplementationEscortTeamMembers => Set<ImplementationEscortTeamMember>();
    public DbSet<ImplementationEscortExpense> ImplementationEscortExpenses => Set<ImplementationEscortExpense>();
    public DbSet<ImplementationOtherExpense> ImplementationOtherExpenses => Set<ImplementationOtherExpense>();
    public DbSet<ImplementationDocument> ImplementationDocuments => Set<ImplementationDocument>();
    public DbSet<AppSettings> AppSettings => Set<AppSettings>();
    public DbSet<Codelist> Codelists => Set<Codelist>();
    public DbSet<CodelistValue> CodelistValues => Set<CodelistValue>();
    public DbSet<DocumentTemplate> DocumentTemplates => Set<DocumentTemplate>();
    public DbSet<BusinessRule> BusinessRules => Set<BusinessRule>();
    public DbSet<Notification> Notifications => Set<Notification>();
    public DbSet<InterfaceSyncLog> InterfaceSyncLogs => Set<InterfaceSyncLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasPostgresExtension("pg_trgm");
        modelBuilder.HasPostgresExtension("vector");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        modelBuilder.ApplyAuditOutbox("audit_outbox", payloadColumnType: "jsonb");

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (!typeof(BaseEntity).IsAssignableFrom(entityType.ClrType) || entityType.BaseType is not null)
                continue;

            var parameter = Expression.Parameter(entityType.ClrType, "e");
            var property = Expression.Call(typeof(EF), nameof(EF.Property), [typeof(bool)], parameter, Expression.Constant(nameof(BaseEntity.IsDeleted)));
            modelBuilder.Entity(entityType.ClrType).HasQueryFilter(Expression.Lambda(Expression.Not(property), parameter));
        }
    }
}
