namespace RECAMAS.Application.Errors;

/// All error codes as RECAMAS-XXX constants, grouped by module.
/// Never hardcode an error string in a service — add a constant here,
/// add the matching message to errors.json, then call
/// _errors.Fail&lt;T&gt;(ErrorCodes.Case.SomeNewCode) from the service.
///
/// Numbering convention: each module gets its own block of 100 so codes
/// never collide as modules grow (TCNProfile 001-099, Case 100-199, etc.)
/// Empty for this skeleton commit — filled in module by module.
public static class ErrorCodes
{
    public static class Common
    {
        // Unhandled exceptions are caught globally by ErrorHandlingMiddleware,
        // which returns this generic code — don't rely on it for normal control flow.
        public const string UnhandledException = "RECAMAS-000";
    }

    public static class TCNProfile
    {
        // e.g. public const string DuplicateProfileDetected = "RECAMAS-001";
    }

    public static class Case
    {
        // e.g. public const string InvalidStageTransition = "RECAMAS-101";
    }

    public static class Detention
    {
        // e.g. public const string FacilityAtCapacity = "RECAMAS-201";
    }

    public static class ReturnImplementation
    {
        // e.g. public const string FlightBookingConflict = "RECAMAS-301";
    }

    public static class Rules
    {
        // e.g. public const string InvalidConditionTree = "RECAMAS-401";
    }

    public static class Reports
    {
        // e.g. public const string ReportDefinitionNotFound = "RECAMAS-501";
    }
}
