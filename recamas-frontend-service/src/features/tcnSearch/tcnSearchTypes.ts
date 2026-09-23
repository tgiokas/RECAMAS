export const Gender = {
  Male: 'Male',
  Female: 'Female',
  Unspecified: 'Unspecified',
} as const;

export type Gender = (typeof Gender)[keyof typeof Gender];

export const MdFileRelationship = {
  Principal: 'Principal',
  MainDependant: 'Main Dependant',
  Dependant: 'Dependant',
} as const;

export type MdFileRelationship = (typeof MdFileRelationship)[keyof typeof MdFileRelationship];

export const Nationality = {
  Afghanistan: 'Afghanistan',
  Bangladesh: 'Bangladesh',
  Egypt: 'Egypt',
  Lebanon: 'Lebanon',
  Nigeria: 'Nigeria',
  Pakistan: 'Pakistan',
  Somalia: 'Somalia',
  Syria: 'Syria',
  Ukraine: 'Ukraine',
} as const;

export type Nationality = (typeof Nationality)[keyof typeof Nationality];

export const TcnSourceSystem = {
  Ars: 'ARS',
  Cass: 'CASS',
  ArrivalsDepartures: 'Arrivals-Departures',
  Stoplist: 'Stoplist',
  Recamas: 'Recamas',
} as const;

export type TcnSourceSystem = (typeof TcnSourceSystem)[keyof typeof TcnSourceSystem];

export const PlaceOfBirth = {
  Beirut: 'Beirut',
  Cairo: 'Cairo',
  Damascus: 'Damascus',
  Dhaka: 'Dhaka',
  Kabul: 'Kabul',
  Karachi: 'Karachi',
  Kyiv: 'Kyiv',
  Lagos: 'Lagos',
  Mogadishu: 'Mogadishu',
} as const;

export type PlaceOfBirth = (typeof PlaceOfBirth)[keyof typeof PlaceOfBirth];

export interface TcnSearchCriteria {
  arc?: string;
  name?: string;
  surname?: string;
  nationality?: Nationality;
  passportNo?: string;
  dateOfBirth?: Date | string;
  mdFileNumber?: string;
}

export interface TcnSearchResult {
  arc: string;
  firstName: string;
  lastName: string;
  nationality: Nationality;
  gender: Gender;
  passportNo: string;
  passportExpirationDate: string;
  dateOfBirth: string;
  placeOfBirth: PlaceOfBirth;
  address: string;
  phoneNo: string;
  photograph: string;
  mdFileNo: string;
  relationshipToMdFile: MdFileRelationship;
  systems: TcnSourceSystem[];
}
