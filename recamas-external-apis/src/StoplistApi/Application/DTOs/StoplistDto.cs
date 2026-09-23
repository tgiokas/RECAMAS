using Shared.Domain.Enums;

namespace StoplistApi.Application.DTOs;

/// <summary>Search request — Table 167</summary>
public record StoplistSearchRequest(
    string? Arc, string? Name, string? Surname,
    Nationality? Nationality, string? PassportNo, DateTime? DateOfBirth);

/// <summary>Create request</summary>
public record StoplistCreateRequest(
    string Arc, string FirstName, string LastName,
    Nationality Nationality, string? PassportNo, DateTime DateOfBirth,
    bool IsOnStoplist, string? UniqueEntryBanNumber, DateTime? StoplistEntryDate);

/// <summary>Response — Table 168</summary>
public record StoplistSearchResponse(
    string Arc, string FirstName, string LastName,
    bool IsOnStoplist, string? UniqueEntryBanNumber, DateTime? StoplistEntryDate);
