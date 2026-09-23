import { AvrCaseStage } from '@/features/Cases/Details/casesTypes';
import type { RecamasCase } from '@/features/Cases/Details/casesTypes';

export interface CaseHistoryEntry {
  timestamp: string;
  user: string;
  role: string;
  action: string;
  comments: string;
}

export interface CaseCounsellingSummary {
  language: string;
  interpreterRequired: boolean;
  returnProgramme: string;
  legalStatus: string;
  vulnerabilityStatus: string;
  accompanyingMinors: number;
  financialLiabilities: string[];
  legalLiabilities: string[];
}

export interface CaseDetailFlag {
  label: string;
  tone: 'warning' | 'neutral' | 'danger';
}

export interface CaseSecurityChecks {
  criminalRecordFound: boolean;
  restrictiveActivitiesFound: boolean;
  findings: string;
}

export interface CaseTravelDocumentEntry {
  documentType: string;
  profileBadge: string;
  documentNumber: string;
  issuingCountry: string;
  issuingAuthority: string;
  expirationDate: string;
  canBeUsedForReturn: boolean;
  issuedForReturnCase: boolean;
  issuedForReturnCaseLabel: string;
  physicalLocationDelivered: boolean;
  physicalLocationLabel: string;
}

export interface CaseReturnDecision {
  decisionId: string;
  source: string;
  issuingAuthority: string;
  decisionDate: string;
  tcnReceiptDate: string;
  voluntaryReturnDeadline: string;
  entryBanDuration: string | null;
}

export interface CaseVulnerabilityAndNeeds {
  fitToFly: boolean;
  issues: string;
}

export interface CaseReturnDecisionApproval {
  decisionId: string;
  status: 'Approved' | 'Pending' | 'Rejected';
  documentName?: string;
  documentLanguage?: string;
  tcns: string;
  approvedBy?: string;
  decisionDateTime?: string;
  voluntaryDepartureDeadline?: string;
  approverNotes?: string;
  assessmentDecision: string;
}

export interface CaseRequestEntry {
  requestId: string;
  title: string;
  status: 'Closed' | 'Open' | 'Pending';
  requestType: string;
  requestedBy: string;
  recipientType: string;
  requestedTo: string;
  requestInitiationDateTime: string;
  requestAnswerDateTime?: string;
  requestNotes?: string;
  answerNotes?: string;
  attachmentName?: string;
  attachmentUrl?: string;
}

export interface CaseIssuanceRequestInfo {
  issuanceId: string;
  enabledStageBadge: string;
  tcn: string;
  requestDate: string;
  documentType: string;
  notes: string;
  requestedBy: string;
  requestingAuthority: string;
  issuingCountry: string;
  issuingAuthority: string;
  issueDate: string;
  status: 'Issued' | 'Pending' | 'Rejected';
}

export interface CaseIssuedDocumentInfo {
  documentType: string;
  documentNumber: string;
  issuingCountry: string;
  issuingAuthority: string;
  issueDate: string;
  expirationDate: string;
  attachmentName?: string;
  attachmentUrl?: string;
}

export interface CaseTravelDocumentIssuance {
  requestInfo: CaseIssuanceRequestInfo;
  issuedDocument: CaseIssuedDocumentInfo;
}

export interface CasePreReturnChecklistRow {
  tcn: string;
  travelDocumentExists: boolean;
  travelDocumentDelivered: boolean;
  travelDocumentReceived: boolean;
  fitToFly: boolean;
  preReturnActivitiesCompleted: boolean;
}

export interface CaseImplementationOverview {
  implementationId: string;
  badge: string;
  implementationType: string;
  initiationDateTime: string;
  plannedExecutionDate: string;
  destinationCountry: string;
  status: string;
}

export interface CaseImplementationChecklistRow {
  item: string;
  complete: boolean;
  note: string;
}

export interface CaseReturnImplementation {
  overview: CaseImplementationOverview;
  checklist: CaseImplementationChecklistRow[];
}

export interface CaseUploadedDocument {
  documentType: string;
  description: string;
  relatesTo: string;
  uploadedBy: string;
  uploadDateTime: string;
  fileName: string;
  fileUrl: string;
}

export interface CaseDetailsData extends RecamasCase {
  caseReference: string;
  priority: 'High' | 'Medium' | 'Low';
  statusLabel: string;
  caseTypeLabel: string;
  program: string;
  initiationDateTime: string;
  initiationOffice: string;
  implementationOffice: string;
  internationalFramework: string;
  returnReason: string;
  accompanyingFamilyMembersCount: number;
  tcnId: string;
  nationality: string;
  gender: string;
  mdFileNumber: string;
  residencyStatus: string;
  ipApplicationStatus: string;
  ipStatus: string;
  appealStatus: string;
  relationshipToMdFile: string;
  policeFileReference: string;
  policeRequestDate: string;
  travelDocumentNumber: string;
  travelDocumentExpiryDate: string;
  isTravelDocumentAttached: boolean;
  approvalStatus: string;
  counsellingSummary: CaseCounsellingSummary;
  flags: CaseDetailFlag[];
  securityChecks: CaseSecurityChecks;
  travelDocuments: CaseTravelDocumentEntry[];
  returnDecision: CaseReturnDecision;
  vulnerabilityAndNeeds: CaseVulnerabilityAndNeeds;
  returnDecisionApprovals: CaseReturnDecisionApproval[];
  requests: CaseRequestEntry[];
  travelDocumentIssuance: CaseTravelDocumentIssuance;
  preReturnChecklist: CasePreReturnChecklistRow[];
  returnImplementation: CaseReturnImplementation;
  uploadedDocuments: CaseUploadedDocument[];
  historyLogs: CaseHistoryEntry[];
}

export interface CaseDetailsProps {
  caseData: CaseDetailsData;
  steps?: readonly AvrCaseStage[];
}
