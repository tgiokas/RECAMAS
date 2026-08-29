namespace RECAMAS.Domain.Enums;

/// <summary>
/// Which system last supplied a given record — appears throughout the
/// Implementation Study as "Source | Enum | ARS, CASS, RECAMAS ...".
/// </summary>
public enum RecordSource
{
    Ars = 1,
    Cass = 2,
    Recamas = 3,
}
