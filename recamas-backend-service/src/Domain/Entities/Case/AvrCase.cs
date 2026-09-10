using RECAMAS.Domain.Common;
using RECAMAS.Domain.Enums;
using RECAMAS.Domain.Entities.TCNProfile;
using RECAMAS.Domain.Entities.Case;
using RECAMAS.Domain.Entities.Detention;
using RECAMAS.Domain.Entities.ReturnImplementation;
using RECAMAS.Domain.Entities.Admin;

namespace RECAMAS.Domain.Entities.Case;

/// Assisted Voluntary Return case (§4.4)
public class AvrCase : ReturnCase
{
    public AvrProgram Program { get; set; }             // AVRCyprus | EURP

    public long? CounsellingSessionId { get; set; }
    public CounsellingSession? CounsellingSession { get; set; }
    public PreReturnChecklist? PreReturnChecklist { get; set; }
    public ICollection<TravelDocumentIssuance> TravelDocumentIssuances { get; set; } = [];
}
