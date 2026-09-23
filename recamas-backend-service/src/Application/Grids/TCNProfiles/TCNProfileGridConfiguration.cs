using RECAMAS.Application.Grids.Abstractions;
using RECAMAS.Application.Grids.Models;
using RECAMAS.Domain.Enums;

namespace RECAMAS.Application.Grids.TCNProfiles;

/// <summary>
/// Grid configuration for the TCN profiles listing screen. Defines the columns over
/// <see cref="TCNProfileGridRowDto"/> rows, including identifying details, demographics,
/// status and risk flags.
/// </summary>
public sealed class TCNProfileGridConfiguration : GridConfiguration<TCNProfileGridRowDto>
{
    private readonly IReadOnlyDictionary<string, GridField> _fields;
    private readonly IReadOnlyList<SortDescriptor> _defaultSort;
    private readonly int _maxPageSize;

    public TCNProfileGridConfiguration()
    {
        var builder = new GridConfigurationBuilder<TCNProfileGridRowDto>()
            .Field("id", typeof(long), searchable: false, sortable: true, filterable: true, sourceMember: "Id")
            .Field("recamasId", typeof(string), searchable: true, sortable: true, filterable: true)
            .Field("firstNameEl", typeof(string), searchable: true, sortable: true, filterable: true)
            .Field("lastNameEl", typeof(string), searchable: true, sortable: true, filterable: true)
            .Field("firstNameEn", typeof(string), searchable: true, sortable: true, filterable: true)
            .Field("lastNameEn", typeof(string), searchable: true, sortable: true, filterable: true)
            .Field("gender", typeof(GenderType), searchable: false, sortable: true, filterable: true)
            .Field("dateOfBirth", typeof(DateOnly?), searchable: false, sortable: true, filterable: true)
            .Field("status", typeof(TcnProfileStatus), searchable: false, sortable: true, filterable: true)
            .Field("primarySource", typeof(DataSourceType), searchable: false, sortable: true, filterable: true)
            .Field("arc", typeof(string), searchable: true, sortable: true, filterable: true)
            .Field("mdFileNo", typeof(string), searchable: true, sortable: true, filterable: true)
            .Field("cassFileNo", typeof(string), searchable: true, sortable: true, filterable: true)
            .Field("flagSecurityIssues", typeof(bool), searchable: false, sortable: true, filterable: true)
            .Field("flagMinor", typeof(bool), searchable: false, sortable: true, filterable: true)
            .Field("flagNoArc", typeof(bool), searchable: false, sortable: true, filterable: true)
            .Field("flagNoTravelDocument", typeof(bool), searchable: false, sortable: true, filterable: true)
            .Field("createdAt", typeof(DateTimeOffset), searchable: false, sortable: true, filterable: true)
            .DefaultSortBy("createdAt", desc: true)
            .MaxPageSize(1000);

        var built = builder.Build("tcnProfiles");
        _fields = built.Fields;
        _defaultSort = built.DefaultSort;
        _maxPageSize = built.MaxPageSize;
    }

    public override string Key => "tcnProfiles";
    public override IReadOnlyDictionary<string, GridField> Fields => _fields;
    public override IReadOnlyList<SortDescriptor> DefaultSort => _defaultSort;
    public override int MaxPageSize => _maxPageSize;
}
