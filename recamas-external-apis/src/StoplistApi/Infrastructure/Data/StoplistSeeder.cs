using StoplistApi.Domain.Entities;
using Shared.Domain.Enums;

namespace StoplistApi.Infrastructure.Data;

public static class StoplistSeeder
{
    public static List<StoplistRecord> GetSeedData() => new()
    {
        // === 5 SHARED ARCs ===
        new() { Arc="ARC-001-SYR", FirstName="Ahmad", LastName="Al-Hassan",
            Nationality=Nationality.Syrian, PassportNo="P-SYR-001",
            DateOfBirth=new DateTime(1988,5,12),
            IsOnStoplist=false },

        new() { Arc="ARC-002-AFG", FirstName="Farida", LastName="Ahmadi",
            Nationality=Nationality.Afghan, PassportNo="P-AFG-002",
            DateOfBirth=new DateTime(1995,11,3),
            IsOnStoplist=false },

        new() { Arc="ARC-003-IRQ", FirstName="Omar", LastName="Khalil",
            Nationality=Nationality.Iraqi, PassportNo="P-IRQ-003",
            DateOfBirth=new DateTime(1982,7,22),
            IsOnStoplist=true,
            UniqueEntryBanNumber="BAN-CY-2024-003",
            StoplistEntryDate=new DateTime(2024,12,21) },

        new() { Arc="ARC-004-PAK", FirstName="Aisha", LastName="Malik",
            Nationality=Nationality.Pakistani, PassportNo="P-PAK-004",
            DateOfBirth=new DateTime(1990,2,18),
            IsOnStoplist=false },

        new() { Arc="ARC-005-NGA", FirstName="Emmanuel", LastName="Okafor",
            Nationality=Nationality.Nigerian, PassportNo="P-NGA-005",
            DateOfBirth=new DateTime(1985,9,30),
            IsOnStoplist=false },

        // === 15 STOPLIST-ONLY records ===
        new() { Arc="ARC-S01-ALB", FirstName="Fatos", LastName="Berisha",
            Nationality=Nationality.Albanian, PassportNo="P-ALB-S01",
            DateOfBirth=new DateTime(1983,4,11),
            IsOnStoplist=true,
            UniqueEntryBanNumber="BAN-CY-2024-101",
            StoplistEntryDate=new DateTime(2024,9,22) },

        new() { Arc="ARC-S02-GEO", FirstName="Irakli", LastName="Mchedlishvili",
            Nationality=Nationality.Georgian, PassportNo="P-GEO-S02",
            DateOfBirth=new DateTime(1990,7,3),
            IsOnStoplist=true,
            UniqueEntryBanNumber="BAN-CY-2023-102",
            StoplistEntryDate=new DateTime(2023,11,5) },

        new() { Arc="ARC-S03-MAR", FirstName="Samir", LastName="El-Ouafi",
            Nationality=Nationality.Moroccan, DateOfBirth=new DateTime(1978,9,18),
            IsOnStoplist=true,
            UniqueEntryBanNumber="BAN-CY-2025-103",
            StoplistEntryDate=new DateTime(2025,2,14) },

        new() { Arc="ARC-S04-DZA", FirstName="Mourad", LastName="Belounis",
            Nationality=Nationality.Algerian, PassportNo="P-DZA-S04",
            DateOfBirth=new DateTime(1981,12,27),
            IsOnStoplist=true,
            UniqueEntryBanNumber="BAN-CY-2025-104",
            StoplistEntryDate=new DateTime(2025,3,2) },

        new() { Arc="ARC-S05-TUN", FirstName="Walid", LastName="Cherif",
            Nationality=Nationality.Tunisian, PassportNo="P-TUN-S05",
            DateOfBirth=new DateTime(1986,5,14),
            IsOnStoplist=true,
            UniqueEntryBanNumber="BAN-CY-2024-105",
            StoplistEntryDate=new DateTime(2024,8,30) },

        new() { Arc="ARC-S06-EGY", FirstName="Tamer", LastName="Mansour",
            Nationality=Nationality.Egyptian, PassportNo="P-EGY-S06",
            DateOfBirth=new DateTime(1975,3,9),
            IsOnStoplist=false },

        new() { Arc="ARC-S07-SYR", FirstName="Basil", LastName="Nasser",
            Nationality=Nationality.Syrian, DateOfBirth=new DateTime(1988,10,22),
            IsOnStoplist=false },

        new() { Arc="ARC-S08-IRN", FirstName="Arash", LastName="Karimi",
            Nationality=Nationality.Iranian, PassportNo="P-IRN-S08",
            DateOfBirth=new DateTime(1979,1,31),
            IsOnStoplist=true,
            UniqueEntryBanNumber="BAN-CY-2023-108",
            StoplistEntryDate=new DateTime(2023,7,18) },

        new() { Arc="ARC-S09-PAK", FirstName="Imran", LastName="Khan",
            Nationality=Nationality.Pakistani, PassportNo="P-PAK-S09",
            DateOfBirth=new DateTime(1992,6,15),
            IsOnStoplist=false },

        new() { Arc="ARC-S10-NGA", FirstName="Blessing", LastName="Adeyemi",
            Nationality=Nationality.Nigerian, DateOfBirth=new DateTime(1995,8,4),
            IsOnStoplist=false },

        new() { Arc="ARC-S11-ETH", FirstName="Mulatu", LastName="Astatke",
            Nationality=Nationality.Ethiopian, DateOfBirth=new DateTime(1984,11,12),
            IsOnStoplist=true,
            UniqueEntryBanNumber="BAN-CY-2024-111",
            StoplistEntryDate=new DateTime(2024,5,19) },

        new() { Arc="ARC-S12-SDN", FirstName="Ahmed", LastName="Omar",
            Nationality=Nationality.Sudanese, DateOfBirth=new DateTime(1977,2,6),
            IsOnStoplist=true,
            UniqueEntryBanNumber="BAN-CY-2022-112",
            StoplistEntryDate=new DateTime(2022,12,10) },

        new() { Arc="ARC-S13-BGD", FirstName="Jahid", LastName="Uddin",
            Nationality=Nationality.Bangladeshi, DateOfBirth=new DateTime(1993,4,25),
            IsOnStoplist=false },

        new() { Arc="ARC-S14-CMR", FirstName="Herve", LastName="Ombga",
            Nationality=Nationality.Cameroonian, DateOfBirth=new DateTime(1987,9,7),
            IsOnStoplist=false },

        new() { Arc="ARC-S15-COD", FirstName="Patient", LastName="Mwamba",
            Nationality=Nationality.Congolese, DateOfBirth=new DateTime(1980,7,14),
            IsOnStoplist=true,
            UniqueEntryBanNumber="BAN-CY-2023-115",
            StoplistEntryDate=new DateTime(2023,4,3) }
    };
}
