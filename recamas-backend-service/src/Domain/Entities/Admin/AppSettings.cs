using RECAMAS.Domain.Common;
using RECAMAS.Domain.Enums;
using RECAMAS.Domain.Entities.TCNProfile;
using RECAMAS.Domain.Entities.Case;
using RECAMAS.Domain.Entities.Detention;
using RECAMAS.Domain.Entities.ReturnImplementation;
using RECAMAS.Domain.Entities.Admin;

namespace RECAMAS.Domain.Entities.Admin;

/// Ρυθμίσεις — ρυθμίσεις και παράμετροι συστήματος
public class AppSettings : BaseEntity
{
    public string SettingKey { get; set; } = null!; // The key of the setting or parameter
    public string SettingValue { get; set; } = null!; // The value of the setting — string
    public string? DisplayName { get; set; } // Το όνομα της παραμέτρου όπως αυτή θα εμφανίζεται στο UI
    public string? Description { get; set; } // Σύντομο κείμενο που να εξηγεί την παράμετρο ή την ρύθμιση
    public string? Data_Type { get; set; }  // Ο τύπος της τιμής SettingValue
    public string? Category { get; set; }  // Ο τύπος της τιμής SettingValue
    public bool IsActive { get; set; }
    public bool IsEncrypted { get; set; }
}
