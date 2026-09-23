using Shared.Domain.Enums;

namespace ArrivalsApi.Application.DTOs;

/// <summary>Search request — Table 164</summary>
public record ArrivalsSearchRequest(
    string? Arc, string? Name, string? Surname,
    Nationality? Nationality, string? PassportNo, DateTime? DateOfBirth);

/// <summary>Create request — all fields</summary>
public record ArrivalsCreateRequest(
    string Arc, string FirstName, string LastName,
    Nationality Nationality, string? PassportNo, DateTime DateOfBirth,
    MovementType MovementType, DateTime MovementDate, Airport Airport);

/// <summary>Response — Table 165</summary>
public record ArrivalMovementResponse(
    string Arc, string FirstName, string LastName,
    MovementType MovementType, DateTime Date, Airport Airport);
