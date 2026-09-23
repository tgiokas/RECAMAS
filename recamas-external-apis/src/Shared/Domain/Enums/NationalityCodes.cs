namespace Shared.Domain.Enums;

public static class NationalityCodes
{
    private static readonly IReadOnlyDictionary<Nationality, string> Codes = new Dictionary<Nationality, string>
    {
        [Nationality.Syrian] = "SYR", [Nationality.Afghan] = "AFG", [Nationality.Iraqi] = "IRQ",
        [Nationality.Pakistani] = "PAK", [Nationality.Nigerian] = "NGA", [Nationality.Somali] = "SOM",
        [Nationality.Eritrean] = "ERI", [Nationality.Bangladeshi] = "BGD", [Nationality.Ethiopian] = "ETH",
        [Nationality.Egyptian] = "EGY", [Nationality.Albanian] = "ALB", [Nationality.Georgian] = "GEO",
        [Nationality.Moroccan] = "MAR", [Nationality.Algerian] = "DZA", [Nationality.Tunisian] = "TUN",
        [Nationality.Iranian] = "IRN", [Nationality.Sudanese] = "SDN", [Nationality.Congolese] = "COD",
        [Nationality.Cameroonian] = "CMR", [Nationality.Ghanaian] = "GHA"
    };

    public static string ToAlpha3(this Nationality nationality) => Codes[nationality];

    public static bool TryParseAlpha3(string? value, out Nationality nationality)
    {
        var match = Codes.FirstOrDefault(x => string.Equals(x.Value, value?.Trim(), StringComparison.OrdinalIgnoreCase));
        nationality = match.Key;
        return match.Value is not null;
    }
}
