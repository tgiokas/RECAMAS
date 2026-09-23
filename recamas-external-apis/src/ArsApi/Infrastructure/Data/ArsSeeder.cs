using ArsApi.Domain.Entities;
using Shared.Domain.Enums;

namespace ArsApi.Infrastructure.Data;

/// <summary>
/// Seeds 40 ARS records. First 5 ARCs are shared across all APIs
/// to simulate the same TCN appearing in multiple external systems.
/// ARC-021 to ARC-032 are near-duplicate records added on purpose, in groups
/// of 3, to exercise the "potential duplicate" detection categories: same
/// surname, same date of birth, same surname + country of origin, and same
/// travel document issuing country (== nationality in this mock).
/// ARC-033 to ARC-040 are grouped by ArsFolderNumber to simulate family/case
/// relationships (father/mother/child, siblings, uncle/nephew), with age
/// gaps kept realistic for the implied relationship.
/// </summary>
public static class ArsSeeder
{
    public static List<ArsRecord> GetSeedData() => new()
    {
        // === 5 SHARED TCNs (same ARC across ARS, CASS, Arrivals, Stoplist) ===
        new() {
            Arc = "ARC-001-SYR", FirstName = "Ahmad", LastName = "Al-Hassan",
            Nationality = Nationality.Syrian, Gender = Gender.Male,
            PassportNo = "P-SYR-001", PassportExpirationDate = new DateTime(2027, 3, 15),
            DateOfBirth = new DateTime(1988, 5, 12), PlaceOfBirth = "Damascus",
            Address = "14 Ledras Street, Nicosia", PhoneNo = "+357-99-111001",
            MdFileNo = "MD-2020-0001", RelationshipToFile = RelationshipToFile.PrimaryApplicant,
            PermitType = PermitType.TemporaryResidence, ResidencyIssueDate = new DateTime(2022, 1, 10),
            ResidenceCategory = ResidenceCategory.Other, PurposeOfResidence = "Asylum Seeker",
            ResidencyExpiryDate = new DateTime(2025, 1, 10), ResidencyStatus = ResidencyStatus.Active,
            ResidencyDocumentNumber = "RES-CY-00001",
            ReturnDecisionDate = new DateTime(2024, 6, 1), ReturnDecisionText = "Voluntary return ordered",
            VoluntaryReturnDeadlineDays = 30, EntryBanDurationMonths = 0
        },
        new() {
            Arc = "ARC-002-AFG", FirstName = "Farida", LastName = "Ahmadi",
            Nationality = Nationality.Afghan, Gender = Gender.Female,
            PassportNo = "P-AFG-002", PassportExpirationDate = new DateTime(2026, 8, 20),
            DateOfBirth = new DateTime(1995, 11, 3), PlaceOfBirth = "Kabul",
            Address = "5 Arch. Makarios Ave, Limassol", PhoneNo = "+357-99-111002",
            MdFileNo = "MD-2021-0002", RelationshipToFile = RelationshipToFile.PrimaryApplicant,
            PermitType = PermitType.AsylumSeeker, ResidencyIssueDate = new DateTime(2021, 5, 15),
            ResidenceCategory = ResidenceCategory.Other, PurposeOfResidence = "IP Applicant",
            ResidencyExpiryDate = new DateTime(2026, 5, 15), ResidencyStatus = ResidencyStatus.Active,
            ResidencyDocumentNumber = "RES-CY-00002",
            TypeOfPermitRequested = PermitType.SubsidiaryProtection,
            TypeOfApplication = "First Application",
            ApplicationSubmissionDate = new DateTime(2021, 5, 15),
            ApplicationStatus = ResidencyStatus.Pending
        },
        new() {
            Arc = "ARC-003-IRQ", FirstName = "Omar", LastName = "Khalil",
            Nationality = Nationality.Iraqi, Gender = Gender.Male,
            PassportNo = "P-IRQ-003", PassportExpirationDate = new DateTime(2025, 2, 10),
            DateOfBirth = new DateTime(1982, 7, 22), PlaceOfBirth = "Baghdad",
            Address = "22 Kennedy Street, Paphos", PhoneNo = "+357-99-111003",
            MdFileNo = "MD-2019-0003", RelationshipToFile = RelationshipToFile.PrimaryApplicant,
            PermitType = PermitType.TemporaryResidence, ResidencyIssueDate = new DateTime(2019, 9, 1),
            ResidenceCategory = ResidenceCategory.Other, PurposeOfResidence = "Protection",
            ResidencyExpiryDate = new DateTime(2024, 9, 1), ResidencyStatus = ResidencyStatus.Expired,
            ResidencyDocumentNumber = "RES-CY-00003",
            ReturnDecisionDate = new DateTime(2024, 11, 15), ReturnDecisionText = "Forced return ordered",
            VoluntaryReturnDeadlineDays = 0, EntryBanDurationMonths = 60
        },
        new() {
            Arc = "ARC-004-PAK", FirstName = "Aisha", LastName = "Malik",
            Nationality = Nationality.Pakistani, Gender = Gender.Female,
            PassportNo = "P-PAK-004", PassportExpirationDate = new DateTime(2028, 4, 5),
            DateOfBirth = new DateTime(1990, 2, 18), PlaceOfBirth = "Karachi",
            Address = "8 Athalassa Road, Nicosia", PhoneNo = "+357-99-111004",
            MdFileNo = "MD-2022-0004", RelationshipToFile = RelationshipToFile.PrimaryApplicant,
            PermitType = PermitType.TemporaryResidence, ResidencyIssueDate = new DateTime(2022, 3, 20),
            ResidenceCategory = ResidenceCategory.Other, PurposeOfResidence = "Protection",
            ResidencyExpiryDate = new DateTime(2025, 3, 20), ResidencyStatus = ResidencyStatus.Active,
            ResidencyDocumentNumber = "RES-CY-00004"
        },
        new() {
            Arc = "ARC-005-NGA", FirstName = "Emmanuel", LastName = "Okafor",
            Nationality = Nationality.Nigerian, Gender = Gender.Male,
            PassportNo = "P-NGA-005", PassportExpirationDate = new DateTime(2026, 12, 1),
            DateOfBirth = new DateTime(1985, 9, 30), PlaceOfBirth = "Lagos",
            Address = "3 Evagoras Street, Larnaca", PhoneNo = "+357-99-111005",
            MdFileNo = "MD-2020-0005", RelationshipToFile = RelationshipToFile.PrimaryApplicant,
            PermitType = PermitType.TemporaryResidence, ResidencyIssueDate = new DateTime(2020, 7, 8),
            ResidenceCategory = ResidenceCategory.Other, PurposeOfResidence = "Asylum",
            ResidencyExpiryDate = new DateTime(2025, 7, 8), ResidencyStatus = ResidencyStatus.Active,
            ResidencyDocumentNumber = "RES-CY-00005",
            ReturnDecisionDate = new DateTime(2025, 1, 10), ReturnDecisionText = "Voluntary return - AVR program",
            VoluntaryReturnDeadlineDays = 60, EntryBanDurationMonths = 0
        },

        // === 15 ARS-ONLY TCNs ===
        new() {
            Arc = "ARC-006-SOM", FirstName = "Hodan", LastName = "Ahmed",
            Nationality = Nationality.Somali, Gender = Gender.Female,
            PassportNo = "P-SOM-006", DateOfBirth = new DateTime(1993, 4, 14),
            PlaceOfBirth = "Mogadishu", Address = "12 Gladstonos, Limassol",
            MdFileNo = "MD-2021-0006", RelationshipToFile = RelationshipToFile.PrimaryApplicant,
            PermitType = PermitType.AsylumSeeker, ResidencyStatus = ResidencyStatus.Active,
            ResidencyIssueDate = new DateTime(2021, 2, 1), ResidencyExpiryDate = new DateTime(2026, 2, 1),
            ResidencyDocumentNumber = "RES-CY-00006"
        },
        new() {
            Arc = "ARC-007-ERI", FirstName = "Tesfaye", LastName = "Haile",
            Nationality = Nationality.Eritrean, Gender = Gender.Male,
            DateOfBirth = new DateTime(1987, 6, 25), PlaceOfBirth = "Asmara",
            MdFileNo = "MD-2018-0007", RelationshipToFile = RelationshipToFile.PrimaryApplicant,
            PermitType = PermitType.SubsidiaryProtection, ResidencyStatus = ResidencyStatus.Active,
            ResidencyIssueDate = new DateTime(2018, 11, 10), ResidencyExpiryDate = new DateTime(2027, 11, 10),
            ResidencyDocumentNumber = "RES-CY-00007",
            ReturnDecisionDate = new DateTime(2024, 3, 5),
            ReturnDecisionText = "Appeal pending - decision suspended",
            VoluntaryReturnDeadlineDays = 0, EntryBanDurationMonths = 0
        },
        new() {
            Arc = "ARC-008-BGD", FirstName = "Rahim", LastName = "Chowdhury",
            Nationality = Nationality.Bangladeshi, Gender = Gender.Male,
            PassportNo = "P-BGD-008", DateOfBirth = new DateTime(1991, 8, 11),
            MdFileNo = "MD-2023-0008", RelationshipToFile = RelationshipToFile.PrimaryApplicant,
            PermitType = PermitType.TemporaryResidence, ResidencyStatus = ResidencyStatus.Active
        },
        new() {
            Arc = "ARC-009-ETH", FirstName = "Tigist", LastName = "Bekele",
            Nationality = Nationality.Ethiopian, Gender = Gender.Female,
            DateOfBirth = new DateTime(1996, 1, 5), PlaceOfBirth = "Addis Ababa",
            MdFileNo = "MD-2022-0009", RelationshipToFile = RelationshipToFile.Spouse,
            PermitType = PermitType.AsylumSeeker, ResidencyStatus = ResidencyStatus.Pending
        },
        new() {
            Arc = "ARC-010-EGY", FirstName = "Mohamed", LastName = "Ibrahim",
            Nationality = Nationality.Egyptian, Gender = Gender.Male,
            PassportNo = "P-EGY-010", DateOfBirth = new DateTime(1979, 3, 27),
            MdFileNo = "MD-2017-0010", RelationshipToFile = RelationshipToFile.PrimaryApplicant,
            PermitType = PermitType.PermanentResidence, ResidencyStatus = ResidencyStatus.Active,
            ResidencyDocumentNumber = "RES-CY-00010"
        },
        new() {
            Arc = "ARC-011-ALB", FirstName = "Arben", LastName = "Gjoka",
            Nationality = Nationality.Albanian, Gender = Gender.Male,
            PassportNo = "P-ALB-011", DateOfBirth = new DateTime(1984, 12, 8),
            MdFileNo = "MD-2023-0011", RelationshipToFile = RelationshipToFile.PrimaryApplicant,
            ReturnDecisionDate = new DateTime(2024, 9, 20),
            ReturnDecisionText = "Return to Albania - no grounds for protection",
            VoluntaryReturnDeadlineDays = 15, EntryBanDurationMonths = 12
        },
        new() {
            Arc = "ARC-012-GEO", FirstName = "Nino", LastName = "Kvaratskhelia",
            Nationality = Nationality.Georgian, Gender = Gender.Female,
            PassportNo = "P-GEO-012", DateOfBirth = new DateTime(1998, 5, 19),
            MdFileNo = "MD-2024-0012", RelationshipToFile = RelationshipToFile.PrimaryApplicant,
            PermitType = PermitType.TemporaryResidence, ResidencyStatus = ResidencyStatus.Active
        },
        new() {
            Arc = "ARC-013-MAR", FirstName = "Fatima", LastName = "Benzekri",
            Nationality = Nationality.Moroccan, Gender = Gender.Female,
            DateOfBirth = new DateTime(1992, 7, 3), MdFileNo = "MD-2021-0013",
            RelationshipToFile = RelationshipToFile.PrimaryApplicant,
            PermitType = PermitType.AsylumSeeker, ResidencyStatus = ResidencyStatus.Active
        },
        new() {
            Arc = "ARC-014-DZA", FirstName = "Karim", LastName = "Boudiaf",
            Nationality = Nationality.Algerian, Gender = Gender.Male,
            PassportNo = "P-DZA-014", DateOfBirth = new DateTime(1983, 10, 14),
            MdFileNo = "MD-2020-0014", RelationshipToFile = RelationshipToFile.PrimaryApplicant,
            ReturnDecisionDate = new DateTime(2025, 2, 28),
            ReturnDecisionText = "Forced return", VoluntaryReturnDeadlineDays = 0,
            EntryBanDurationMonths = 24
        },
        new() {
            Arc = "ARC-015-TUN", FirstName = "Zainab", LastName = "Gharbi",
            Nationality = Nationality.Tunisian, Gender = Gender.Female,
            PassportNo = "P-TUN-015", DateOfBirth = new DateTime(2000, 3, 22),
            MdFileNo = "MD-2024-0015", RelationshipToFile = RelationshipToFile.PrimaryApplicant,
            PermitType = PermitType.TemporaryResidence, ResidencyStatus = ResidencyStatus.Pending
        },
        new() {
            Arc = "ARC-016-IRN", FirstName = "Dariush", LastName = "Ahmadpour",
            Nationality = Nationality.Iranian, Gender = Gender.Male,
            PassportNo = "P-IRN-016", DateOfBirth = new DateTime(1976, 9, 8),
            MdFileNo = "MD-2016-0016", RelationshipToFile = RelationshipToFile.PrimaryApplicant,
            PermitType = PermitType.SubsidiaryProtection, ResidencyStatus = ResidencyStatus.Active,
            ResidencyDocumentNumber = "RES-CY-00016"
        },
        new() {
            Arc = "ARC-017-SDN", FirstName = "Muna", LastName = "Abdalla",
            Nationality = Nationality.Sudanese, Gender = Gender.Female,
            DateOfBirth = new DateTime(1989, 11, 17), MdFileNo = "MD-2022-0017",
            RelationshipToFile = RelationshipToFile.PrimaryApplicant,
            PermitType = PermitType.AsylumSeeker, ResidencyStatus = ResidencyStatus.Active
        },
        new() {
            Arc = "ARC-018-COD", FirstName = "Jean-Pierre", LastName = "Mutombo",
            Nationality = Nationality.Congolese, Gender = Gender.Male,
            PassportNo = "P-COD-018", DateOfBirth = new DateTime(1980, 4, 2),
            MdFileNo = "MD-2019-0018", RelationshipToFile = RelationshipToFile.PrimaryApplicant,
            PermitType = PermitType.HumanitarianProtection, ResidencyStatus = ResidencyStatus.Active,
            ResidencyDocumentNumber = "RES-CY-00018"
        },
        new() {
            Arc = "ARC-019-CMR", FirstName = "Sandrine", LastName = "Mbarga",
            Nationality = Nationality.Cameroonian, Gender = Gender.Female,
            PassportNo = "P-CMR-019", DateOfBirth = new DateTime(1994, 6, 28),
            MdFileNo = "MD-2023-0019", RelationshipToFile = RelationshipToFile.PrimaryApplicant,
            PermitType = PermitType.AsylumSeeker, ResidencyStatus = ResidencyStatus.Active
        },
        new() {
            Arc = "ARC-020-GHA", FirstName = "Kwame", LastName = "Asante",
            Nationality = Nationality.Ghanaian, Gender = Gender.Male,
            PassportNo = "P-GHA-020", DateOfBirth = new DateTime(1988, 8, 15),
            MdFileNo = "MD-2021-0020", RelationshipToFile = RelationshipToFile.PrimaryApplicant,
            ReturnDecisionDate = new DateTime(2024, 12, 1),
            ReturnDecisionText = "Assisted voluntary return approved",
            VoluntaryReturnDeadlineDays = 90, EntryBanDurationMonths = 0
        },

        // === CATEGORY A.1: same LastName, different everything else ===
        new() {
            Arc = "ARC-021-IRQ", FirstName = "Layla", LastName = "Al-Hassan",
            Nationality = Nationality.Iraqi, Gender = Gender.Female,
            PassportNo = "P-IRQ-021", DateOfBirth = new DateTime(1975, 3, 10),
            PlaceOfBirth = "Baghdad", MdFileNo = "MD-2020-0021",
            RelationshipToFile = RelationshipToFile.PrimaryApplicant,
            PermitType = PermitType.TemporaryResidence, ResidencyStatus = ResidencyStatus.Active
        },
        new() {
            Arc = "ARC-022-BGD", FirstName = "Farhan", LastName = "Malik",
            Nationality = Nationality.Bangladeshi, Gender = Gender.Male,
            PassportNo = "P-BGD-022", DateOfBirth = new DateTime(1987, 6, 12),
            PlaceOfBirth = "Dhaka", MdFileNo = "MD-2021-0022",
            RelationshipToFile = RelationshipToFile.PrimaryApplicant,
            PermitType = PermitType.AsylumSeeker, ResidencyStatus = ResidencyStatus.Pending
        },
        new() {
            Arc = "ARC-023-SDN", FirstName = "Solomon", LastName = "Bekele",
            Nationality = Nationality.Sudanese, Gender = Gender.Male,
            DateOfBirth = new DateTime(1991, 9, 25), PlaceOfBirth = "Khartoum",
            MdFileNo = "MD-2022-0023", RelationshipToFile = RelationshipToFile.PrimaryApplicant,
            PermitType = PermitType.SubsidiaryProtection, ResidencyStatus = ResidencyStatus.Active
        },

        // === CATEGORY A.2: same DateOfBirth, different everything else ===
        new() {
            Arc = "ARC-024-GEO", FirstName = "Elena", LastName = "Kostova",
            Nationality = Nationality.Georgian, Gender = Gender.Female,
            PassportNo = "P-GEO-024", DateOfBirth = new DateTime(1988, 5, 12),
            PlaceOfBirth = "Tbilisi", MdFileNo = "MD-2020-0024",
            RelationshipToFile = RelationshipToFile.PrimaryApplicant,
            PermitType = PermitType.TemporaryResidence, ResidencyStatus = ResidencyStatus.Active
        },
        new() {
            Arc = "ARC-025-COD", FirstName = "Bruno", LastName = "Kanku",
            Nationality = Nationality.Congolese, Gender = Gender.Male,
            PassportNo = "P-COD-025", DateOfBirth = new DateTime(1995, 11, 3),
            PlaceOfBirth = "Kinshasa", MdFileNo = "MD-2021-0025",
            RelationshipToFile = RelationshipToFile.PrimaryApplicant,
            PermitType = PermitType.HumanitarianProtection, ResidencyStatus = ResidencyStatus.Active
        },
        new() {
            Arc = "ARC-026-GHA", FirstName = "Amara", LastName = "Owusu",
            Nationality = Nationality.Ghanaian, Gender = Gender.Female,
            PassportNo = "P-GHA-026", DateOfBirth = new DateTime(1982, 7, 22),
            PlaceOfBirth = "Accra", MdFileNo = "MD-2019-0026",
            RelationshipToFile = RelationshipToFile.PrimaryApplicant,
            PermitType = PermitType.AsylumSeeker, ResidencyStatus = ResidencyStatus.Pending
        },

        // === CATEGORY A.3: same LastName AND same country of origin, different everything else ===
        new() {
            Arc = "ARC-027-BGD", FirstName = "Nasrin", LastName = "Chowdhury",
            Nationality = Nationality.Bangladeshi, Gender = Gender.Female,
            PassportNo = "P-BGD-027", DateOfBirth = new DateTime(1994, 2, 14),
            PlaceOfBirth = "Dhaka", MdFileNo = "MD-2022-0027",
            RelationshipToFile = RelationshipToFile.PrimaryApplicant,
            PermitType = PermitType.TemporaryResidence, ResidencyStatus = ResidencyStatus.Active
        },
        new() {
            Arc = "ARC-028-ALB", FirstName = "Besnik", LastName = "Gjoka",
            Nationality = Nationality.Albanian, Gender = Gender.Male,
            PassportNo = "P-ALB-028", DateOfBirth = new DateTime(1979, 5, 30),
            PlaceOfBirth = "Tirana", MdFileNo = "MD-2018-0028",
            RelationshipToFile = RelationshipToFile.PrimaryApplicant,
            ReturnDecisionDate = new DateTime(2024, 10, 5),
            ReturnDecisionText = "Return to Albania - no grounds for protection",
            VoluntaryReturnDeadlineDays = 15, EntryBanDurationMonths = 12
        },
        new() {
            Arc = "ARC-029-MAR", FirstName = "Youssef", LastName = "Benzekri",
            Nationality = Nationality.Moroccan, Gender = Gender.Male,
            DateOfBirth = new DateTime(1990, 10, 10), PlaceOfBirth = "Casablanca",
            MdFileNo = "MD-2023-0029", RelationshipToFile = RelationshipToFile.PrimaryApplicant,
            PermitType = PermitType.AsylumSeeker, ResidencyStatus = ResidencyStatus.Active
        },

        // === CATEGORY A.4: same travel document issuing country (nationality), different everything else ===
        new() {
            Arc = "ARC-030-NGA", FirstName = "Chinedu", LastName = "Adeyemi",
            Nationality = Nationality.Nigerian, Gender = Gender.Male,
            PassportNo = "P-NGA-030", DateOfBirth = new DateTime(1999, 12, 1),
            PlaceOfBirth = "Abuja", MdFileNo = "MD-2023-0030",
            RelationshipToFile = RelationshipToFile.PrimaryApplicant,
            PermitType = PermitType.TemporaryResidence, ResidencyStatus = ResidencyStatus.Pending
        },
        new() {
            Arc = "ARC-031-TUN", FirstName = "Amina", LastName = "Trabelsi",
            Nationality = Nationality.Tunisian, Gender = Gender.Female,
            PassportNo = "P-TUN-031", DateOfBirth = new DateTime(1997, 8, 19),
            PlaceOfBirth = "Sfax", MdFileNo = "MD-2024-0031",
            RelationshipToFile = RelationshipToFile.PrimaryApplicant,
            PermitType = PermitType.TemporaryResidence, ResidencyStatus = ResidencyStatus.Active
        },
        new() {
            Arc = "ARC-032-IRN", FirstName = "Sara", LastName = "Rostami",
            Nationality = Nationality.Iranian, Gender = Gender.Female,
            PassportNo = "P-IRN-032", DateOfBirth = new DateTime(1985, 1, 27),
            PlaceOfBirth = "Tehran", MdFileNo = "MD-2020-0032",
            RelationshipToFile = RelationshipToFile.PrimaryApplicant,
            PermitType = PermitType.SubsidiaryProtection, ResidencyStatus = ResidencyStatus.Active
        },

        // === GROUP B.1 (ARS-FLD-1001): father, mother, child - Hosseini family ===
        new() {
            Arc = "ARC-033-IRN", FirstName = "Reza", LastName = "Hosseini",
            Nationality = Nationality.Iranian, Gender = Gender.Male,
            PassportNo = "P-IRN-033", DateOfBirth = new DateTime(1975, 4, 1),
            PlaceOfBirth = "Tehran", Address = "10 Stasinou Ave, Nicosia",
            MdFileNo = "MD-2023-0033", RelationshipToFile = RelationshipToFile.PrimaryApplicant,
            ArsFolderNumber = "ARS-FLD-1001",
            PermitType = PermitType.AsylumSeeker, ResidencyStatus = ResidencyStatus.Active
        },
        new() {
            Arc = "ARC-034-IRN", FirstName = "Shirin", LastName = "Hosseini",
            Nationality = Nationality.Iranian, Gender = Gender.Female,
            PassportNo = "P-IRN-034", DateOfBirth = new DateTime(1978, 9, 15),
            PlaceOfBirth = "Tehran", Address = "10 Stasinou Ave, Nicosia",
            MdFileNo = "MD-2023-0034", RelationshipToFile = RelationshipToFile.Spouse,
            ArsFolderNumber = "ARS-FLD-1001",
            PermitType = PermitType.AsylumSeeker, ResidencyStatus = ResidencyStatus.Active
        },
        new() {
            Arc = "ARC-035-IRN", FirstName = "Sina", LastName = "Hosseini",
            Nationality = Nationality.Iranian, Gender = Gender.Male,
            DateOfBirth = new DateTime(2010, 6, 20), PlaceOfBirth = "Nicosia",
            Address = "10 Stasinou Ave, Nicosia",
            MdFileNo = "MD-2023-0035", RelationshipToFile = RelationshipToFile.Dependent,
            ArsFolderNumber = "ARS-FLD-1001",
            PermitType = PermitType.AsylumSeeker, ResidencyStatus = ResidencyStatus.Active
        },

        // === GROUP B.2 (ARS-FLD-1002): siblings - Youssef family ===
        new() {
            Arc = "ARC-036-EGY", FirstName = "Karim", LastName = "Youssef",
            Nationality = Nationality.Egyptian, Gender = Gender.Male,
            PassportNo = "P-EGY-036", DateOfBirth = new DateTime(1990, 1, 10),
            PlaceOfBirth = "Cairo", MdFileNo = "MD-2022-0036",
            RelationshipToFile = RelationshipToFile.FamilyMember,
            ArsFolderNumber = "ARS-FLD-1002",
            PermitType = PermitType.TemporaryResidence, ResidencyStatus = ResidencyStatus.Active
        },
        new() {
            Arc = "ARC-037-EGY", FirstName = "Tarek", LastName = "Youssef",
            Nationality = Nationality.Egyptian, Gender = Gender.Male,
            PassportNo = "P-EGY-037", DateOfBirth = new DateTime(1993, 7, 22),
            PlaceOfBirth = "Cairo", MdFileNo = "MD-2022-0037",
            RelationshipToFile = RelationshipToFile.FamilyMember,
            ArsFolderNumber = "ARS-FLD-1002",
            PermitType = PermitType.TemporaryResidence, ResidencyStatus = ResidencyStatus.Active
        },
        new() {
            Arc = "ARC-038-EGY", FirstName = "Layla", LastName = "Youssef",
            Nationality = Nationality.Egyptian, Gender = Gender.Female,
            PassportNo = "P-EGY-038", DateOfBirth = new DateTime(1996, 3, 5),
            PlaceOfBirth = "Cairo", MdFileNo = "MD-2022-0038",
            RelationshipToFile = RelationshipToFile.FamilyMember,
            ArsFolderNumber = "ARS-FLD-1002",
            PermitType = PermitType.TemporaryResidence, ResidencyStatus = ResidencyStatus.Active
        },

        // === GROUP B.3 (ARS-FLD-1003): uncle & nephew - Suleiman family ===
        new() {
            Arc = "ARC-039-SDN", FirstName = "Ibrahim", LastName = "Suleiman",
            Nationality = Nationality.Sudanese, Gender = Gender.Male,
            DateOfBirth = new DateTime(1970, 2, 14), PlaceOfBirth = "Khartoum",
            MdFileNo = "MD-2021-0039", RelationshipToFile = RelationshipToFile.FamilyMember,
            ArsFolderNumber = "ARS-FLD-1003",
            PermitType = PermitType.AsylumSeeker, ResidencyStatus = ResidencyStatus.Active
        },
        new() {
            Arc = "ARC-040-SDN", FirstName = "Yusuf", LastName = "Suleiman",
            Nationality = Nationality.Sudanese, Gender = Gender.Male,
            DateOfBirth = new DateTime(1998, 11, 30), PlaceOfBirth = "Khartoum",
            MdFileNo = "MD-2021-0040", RelationshipToFile = RelationshipToFile.FamilyMember,
            ArsFolderNumber = "ARS-FLD-1003",
            PermitType = PermitType.AsylumSeeker, ResidencyStatus = ResidencyStatus.Active
        }
    };
}
