export const CaseType = {
  Avr: 'AVR',
  ForcedReturn: 'Forced Return',
  VoluntaryReturnByOwnMeans: 'Voluntary Return By Own Means',
} as const;

export type CaseType = (typeof CaseType)[keyof typeof CaseType];

export const AvrCaseStage = {
  Counselling: 'Counselling',
  ApplicationProcessing: 'Application Processing',
  Evaluation: 'Evaluation',
  PreReturn: 'Pre-Return',
  ReturnImplementation: 'Return Implementation',
  Reintegration: 'Re-integration',
  Closed: 'Closed',
} as const;

export type AvrCaseStage = (typeof AvrCaseStage)[keyof typeof AvrCaseStage];

export const CaseStatus = {
  Pending: 'Pending',
  InProgress: 'In Progress',
  Completed: 'Completed',
  Cancelled: 'Cancelled',
} as const;

export type CaseStatus = (typeof CaseStatus)[keyof typeof CaseStatus];

export const CaseFlag = {
  NeedsAttention: 'Needs Attention',
  NoArc: 'No ARC',
  NoTravelDocument: 'No Travel Document',
  Minor: 'Minor',
} as const;

export type CaseFlag = (typeof CaseFlag)[keyof typeof CaseFlag];

export interface RecamasCase {
  caseId: string;
  tcnName: string;
  arc: string;
  type: CaseType;
  currentStage: AvrCaseStage;
  currentStatus: CaseStatus;
  needsAttention: boolean;
  caseFlags: CaseFlag[];
  createdDate: string;
  assignedOfficer: string | null;
}
