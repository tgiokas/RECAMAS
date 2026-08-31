namespace RECAMAS.Domain.Enums;

/// Which system last supplied a given record — appears throughout the
/// Implementation Specs as "Source | Enum | ARS, CASS, RECAMAS ...".
public enum RecordSource
{
    Ars = 1,
    Cass = 2,
    Recamas = 3,
}
