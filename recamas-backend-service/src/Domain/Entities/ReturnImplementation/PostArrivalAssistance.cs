using RECAMAS.Domain.Common;
using RECAMAS.Domain.Enums;
using RECAMAS.Domain.Entities.TCNProfile;
using RECAMAS.Domain.Entities.Case;
using RECAMAS.Domain.Entities.Detention;
using RECAMAS.Domain.Entities.ReturnImplementation;
using RECAMAS.Domain.Entities.Admin;

namespace RECAMAS.Domain.Entities.ReturnImplementation;

/// Post-arrival assistance (§6.3.1.4.2)
public class PostArrivalAssistance : BaseEntity
{
    public long ImplementationTcnId { get; set; }

    public bool Requested { get; set; }
    public DateOnly? RequestDate { get; set; }
    public bool Provided { get; set; }
    public PostArrivalAssistanceType AssistanceType { get; set; }
    public string? Notes { get; set; }
    public string? AttachmentsPath { get; set; }

    public ImplementationTcn ImplementationTcn { get; set; } = null!;
}
