import type { Nationality } from '@/features/tcnSearch/tcnSearchTypes';

export const RecamasProfileStatus = {
  Citizen: 'Citizen',
  PermanentResident: 'Permanent Resident',
  TemporaryResident: 'Temporary Resident',
  Visitor: 'Visitor / tourist',
  WorkVisa: 'Work visa / work permit holder',
  StudentVisa: 'Student visa holder',
  Dependent: 'Dependent / family visa holder',
  Refugee: 'Refugee / asylum seeker',
  Irregular: 'Irregular / undocumented',
  Humanitarian: 'Humanitarian / special protection',
  PendingImmigration: 'Pending immigration application',
  Detained: 'Detained / removal proceedings',
  Diplomatic: 'Diplomatic / official status',
} as const;

export type RecamasProfileStatus = (typeof RecamasProfileStatus)[keyof typeof RecamasProfileStatus];

export interface RecamasProfile {
  profileId: string;
  arc: string;
  firstName: string;
  lastName: string;
  nationality: Nationality;
  dateOfBirth: string;
  passportNo: string;
  mdFileNo: string;
  status: RecamasProfileStatus;
}
