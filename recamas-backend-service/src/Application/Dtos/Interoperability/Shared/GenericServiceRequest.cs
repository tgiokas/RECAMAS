using System.Text.Json;

namespace RECAMAS.Application.Dtos.Interoperability.Shared;

public sealed record GenericServiceRequest(string ServiceName, string Operation, JsonElement RequestData);
