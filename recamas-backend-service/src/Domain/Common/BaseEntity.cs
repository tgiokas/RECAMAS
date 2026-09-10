namespace RECAMAS.Domain.Common;

/// Κοινά audit / metadata πεδία — κληρονομούνται από όλες τις κλάσεις
public abstract class BaseEntity
{
    public long Id { get; set; }                        // PK — bigint / identity
    public Guid PublicId { get; set; }                  // Public identifier — UUIDv4 (§12.5.16)
    public DateTimeOffset CreatedAt { get; set; }       // Χρόνος δημιουργίας εγγραφής
    public long CreatedByUserId { get; set; }           // User που δημιούργησε
    public DateTimeOffset? UpdatedAt { get; set; }      // Χρόνος τελευταίας τροποποίησης
    public long? UpdatedByUserId { get; set; }          // User που τροποποίησε τελευταίος
    public bool IsDeleted { get; set; }                 // Soft delete — δεν διαγράφεται ποτέ hard (§4.3.2)
}
