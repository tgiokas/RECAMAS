namespace RECAMAS.Application.Errors;

/// All error codes as RECAMAS-XXX constants, grouped by module.
/// Never hardcode an error string in a service — add a constant here,
/// add the matching message to errors.json
public static class ErrorCodes
{
    public static class Common
    {
        // Unhandled exceptions are caught globally by ErrorHandlingMiddleware,
        // which returns this generic code
        public const string UnhandledException = "RECAMAS-000";
    }

    public static class TCNProfile
    {
        public const string DuplicateProfileDetected = "RECAMAS-001";
    }

    public static class Case
    {
        public const string InvalidStageTransition = "RECAMAS-101";
    }

    public static class Detention
    {
        public const string FacilityAtCapacity = "RECAMAS-201";
    }

    public static class ReturnImplementation
    {
        public const string FlightBookingConflict = "RECAMAS-301";
    }

    public static class Rules
    {
        public const string InvalidConditionTree = "RECAMAS-401";
    }

    public static class Reports
    {
        public const string ReportDefinitionNotFound = "RECAMAS-501";
    }
}
