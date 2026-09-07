using RECAMAS.Application.Dtos.Common;
using RECAMAS.Application.Dtos.TCNProfile;

namespace RECAMAS.Application.Interfaces;

public interface ITCNProfileService
{
    Task<Result<TCNProfileDto>> CreateAsync(TCNProfileCreateRequest request, CancellationToken ct = default);
}
