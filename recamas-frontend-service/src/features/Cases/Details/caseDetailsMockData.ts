import type { BadgeVariant } from '@/components/Badge/Badge';
import type { DisplayFieldConfig } from '@/components/DisplayForm/DisplayForm';
import { AvrCaseStage, CaseStatus } from '@/features/Cases/Details/casesTypes';
import type { CaseInfoRow } from '@/features/Cases/components/CaseInfoSection';
import type { ReactNode } from 'react';
import type {
  CaseDetailFlag,
  CaseDetailsData,
  CaseHistoryEntry,
  CaseImplementationChecklistRow,
  CaseImplementationOverview,
  CaseIssuanceRequestInfo,
  CaseIssuedDocumentInfo,
  CasePreReturnChecklistRow,
  CaseRequestEntry,
  CaseReturnDecisionApproval,
  CaseTravelDocumentEntry,
  CaseUploadedDocument,
} from './caseDetailsTypes';

export const toneToBadgeVariant: Record<CaseDetailFlag['tone'], BadgeVariant> = {
  warning: 'orange',
  danger: 'red',
  neutral: 'gray',
};

export const approvalStatusColor: Record<CaseReturnDecisionApproval['status'], string> = {
  Approved: '#22C55E',
  Pending: '#F59E0B',
  Rejected: '#EF4444',
};

export const requestStatusColor: Record<CaseRequestEntry['status'], string> = {
  Closed: '#22C55E',
  Open: '#F59E0B',
  Pending: '#F59E0B',
};

export const issuanceStatusColor: Record<CaseIssuanceRequestInfo['status'], string> = {
  Issued: '#22C55E',
  Pending: '#F59E0B',
  Rejected: '#EF4444',
};

export const getFreeformStatusColor = (status: string): string =>
  status.toLowerCase().includes('pending') ? '#F59E0B' : '#22C55E';

export interface CaseDisplayFormConfig {
  formData: Record<string, unknown>;
  fields: DisplayFieldConfig[];
}

export interface CaseGridColumnConfig<TRow = Record<string, unknown>> {
  dataField: string;
  caption: string;
  dataType?: 'string' | 'number' | 'date' | 'datetime' | 'boolean' | 'object';
  width?: number;
  cellRender?: (e: { data: TRow }) => ReactNode;
}

export const defaultCaseStageSteps: readonly AvrCaseStage[] = [
  AvrCaseStage.Counselling,
  AvrCaseStage.ApplicationProcessing,
  AvrCaseStage.Evaluation,
  AvrCaseStage.PreReturn,
  AvrCaseStage.ReturnImplementation,
  AvrCaseStage.Reintegration,
  AvrCaseStage.Closed,
];

export const mockCaseDetailData: CaseDetailsData = {
  caseId: 'CAS-2024-00192',
  caseReference: 'CAS-2024-00192',
  type: 'AVR',
  createdDate: '2026-09-15',
  priority: 'High',
  caseTypeLabel: 'Assisted Voluntary Return',
  program: 'AVR Cyprus',
  initiationDateTime: '18/06/2026 10:05',
  initiationOffice: 'MD — Limassol',
  implementationOffice: 'A&IU — Larnaca',
  internationalFramework: 'EU readmission agreement/arrangements',
  returnReason: 'Asylum Rejection',
  accompanyingFamilyMembersCount: 2,
  assignedOfficer: 'Petros Georgiou',
  tcnName: 'Ahmad Hassan',
  tcnId: 'TCN-00412',
  arc: '10024581',
  mdFileNumber: 'MD-2024-00118',
  nationality: 'Syria',
  gender: 'Male',
  residencyStatus: 'Expired',
  ipApplicationStatus: 'Rejected (final)',
  ipStatus: 'None granted',
  appealStatus: 'Dismissed',
  relationshipToMdFile: 'Principal',
  currentStage: AvrCaseStage.PreReturn,
  currentStatus: CaseStatus.Pending,
  statusLabel: 'Pending Security Check',
  needsAttention: true,
  caseFlags: ['Needs Attention'],
  counsellingSummary: {
    language: 'Arabic',
    interpreterRequired: true,
    returnProgramme: 'EURP (European Return Program)',
    legalStatus: 'Asylum Seeker (Rejected)',
    vulnerabilityStatus: 'Single Parent',
    accompanyingMinors: 1,
    financialLiabilities: ['No Pending Fines'],
    legalLiabilities: ['No Pending Criminal Cases'],
  },
  policeFileReference: 'POL-NC-2026-88192',
  policeRequestDate: '2026-09-15',
  travelDocumentNumber: 'Passport N00481233',
  travelDocumentExpiryDate: '2026-12-10',
  isTravelDocumentAttached: true,
  approvalStatus: 'Pending Final Evaluation',
  flags: [
    { label: ' Needs Attention', tone: 'warning' },
    { label: 'No Travel Document', tone: 'danger' },
  ],
  securityChecks: {
    criminalRecordFound: false,
    restrictiveActivitiesFound: false,
    findings: 'none recorded',
  },
  travelDocuments: [
    {
      documentType: 'Passport',
      profileBadge: 'TCN Profile',
      documentNumber: 'K0384511',
      issuingCountry: 'Afghanistan',
      issuingAuthority: 'Ministry of Interior — Kabul',
      expirationDate: '09/2027',
      canBeUsedForReturn: true,
      issuedForReturnCase: false,
      issuedForReturnCaseLabel: 'No — pre-existing document',
      physicalLocationDelivered: true,
      physicalLocationLabel: 'With TCN — Delivered',
    },
  ],
  returnDecision: {
    decisionId: 'RD-2026-0304',
    source: 'RECAMAS — issued through Case',
    issuingAuthority: 'MD — issued through Case',
    decisionDate: '02/05/2026',
    tcnReceiptDate: '06/05/2026',
    voluntaryReturnDeadline: '31/08/2026',
    entryBanDuration: null,
  },
  vulnerabilityAndNeeds: {
    fitToFly: true,
    issues: 'none recorded',
  },
  returnDecisionApprovals: [
    {
      decisionId: 'RD-2026-0304',
      status: 'Approved',
      documentName: 'Return Decision — Rahimi Nadia',
      documentLanguage: 'Greek / English',
      tcns: 'RAHIMI Nadia',
      approvedBy: 'G. Constantinou (MD Director)',
      decisionDateTime: '02/05/2026 11:20',
      voluntaryDepartureDeadline: '31/08/2026',
      approverNotes: 'Complete file, no objections.',
      assessmentDecision: 'Approved',
    },
    {
      decisionId: 'RD-2026-0305',
      status: 'Approved',
      tcns: 'RAHIMI Omar',
      assessmentDecision: 'Approved',
    },
    {
      decisionId: 'RD-2026-0306',
      status: 'Approved',
      tcns: 'RAHIMI Sara',
      assessmentDecision: 'Approved',
    },
  ],
  requests: [
    {
      requestId: 'REQ-2026-0211',
      title: 'Document request',
      status: 'Closed',
      requestType: 'Document',
      requestedBy: 'L. Papadopoulou (VR Officer)',
      recipientType: 'Role — Police Liaison',
      requestedTo: 'M. Antoniou',
      requestInitiationDateTime: '03/07/2026 09:12',
      requestAnswerDateTime: '03/07/2026 16:45',
      requestNotes: 'Please confirm Stoplist result for all 3 TCNs on the case before Evaluation.',
      answerNotes: 'Confirmed — no hits for any of the 3 TCNs.',
      attachmentName: 'stoplist_check_030726.pdf',
      attachmentUrl: '#',
    },
  ],
  travelDocumentIssuance: {
    requestInfo: {
      issuanceId: 'TDI-2026-0043',
      enabledStageBadge: 'Enabled from Pre-Return stage',
      tcn: 'RAHIMI Sara',
      requestDate: '10/07/2026',
      documentType: 'Laissez-Passer',
      notes: 'Minor holds only a Country Issued ID — passer required for return travel.',
      requestedBy: 'L. Papadopoulou',
      requestingAuthority: 'Migration Department',
      issuingCountry: 'Afghanistan',
      issuingAuthority: 'Embassy of Afghanistan — Athens',
      issueDate: '01/08/2026',
      status: 'Issued',
    },
    issuedDocument: {
      documentType: 'Laissez-Passer',
      documentNumber: 'LP-AFG-2026-3391',
      issuingCountry: 'Afghanistan',
      issuingAuthority: 'Embassy of Afghanistan — Athens',
      issueDate: '01/08/2026',
      expirationDate: '01/11/2026',
      attachmentName: 'laissez_passer_sara.pdf',
      attachmentUrl: '#',
    },
  },
  preReturnChecklist: [
    {
      tcn: 'RAHIMI Nadia',
      travelDocumentExists: true,
      travelDocumentDelivered: true,
      travelDocumentReceived: true,
      fitToFly: true,
      preReturnActivitiesCompleted: true,
    },
    {
      tcn: 'RAHIMI Omar',
      travelDocumentExists: true,
      travelDocumentDelivered: true,
      travelDocumentReceived: true,
      fitToFly: true,
      preReturnActivitiesCompleted: true,
    },
    {
      tcn: 'RAHIMI Sara',
      travelDocumentExists: true,
      travelDocumentDelivered: true,
      travelDocumentReceived: true,
      fitToFly: true,
      preReturnActivitiesCompleted: true,
    },
  ],
  returnImplementation: {
    overview: {
      implementationId: 'IMP-2026-0091',
      badge: 'Implementation',
      implementationType: 'Single — AVR',
      initiationDateTime: '15/07/2026 10:00',
      plannedExecutionDate: '05/09/2026',
      destinationCountry: 'Afghanistan (KBL via IST)',
      status: 'Pending Sign-Off',
    },
    checklist: [
      {
        item: 'Ticket',
        complete: true,
        note: 'TK 787 · 05/09/2026 — all 3 TCNs booked',
      },
      {
        item: 'Sign-Off',
        complete: false,
        note: 'Agreement & Final Confirmation not yet submitted',
      },
      {
        item: 'Departure Activities Completed',
        complete: false,
        note: 'Awaiting Sign-Off',
      },
      {
        item: 'Departure',
        complete: false,
        note: 'Awaiting confirmation from Arrivals/Departures',
      },
      {
        item: 'Implementation Completed',
        complete: false,
        note: 'Manual close after departure confirmed',
      },
    ],
  },
  uploadedDocuments: [
    {
      documentType: 'Counselling Report',
      description: 'Signed scan',
      relatesTo: 'Case',
      uploadedBy: 'L. Papadopoulou',
      uploadDateTime: '22/06/2026 14:30',
      fileName: 'counselling_signed.pdf',
      fileUrl: '#',
    },
    {
      documentType: 'Other',
      description: 'Marriage certificate',
      relatesTo: 'Case',
      uploadedBy: 'L. Papadopoulou',
      uploadDateTime: '22/06/2026 14:35',
      fileName: 'marriage_cert.pdf',
      fileUrl: '#',
    },
    {
      documentType: 'Asylum Rejection Decision',
      description: 'Final decision, CASS',
      relatesTo: 'TCN — Nadia',
      uploadedBy: 'System (CASS)',
      uploadDateTime: '03/07/2026 09:00',
      fileName: 'asylum_rejection_nadia.pdf',
      fileUrl: '#',
    },
    {
      documentType: 'Other',
      description: 'Copy of passport',
      relatesTo: 'TCN — Nadia',
      uploadedBy: 'L. Papadopoulou',
      uploadDateTime: '22/06/2026 14:20',
      fileName: 'passport_nadia.pdf',
      fileUrl: '#',
    },
    {
      documentType: 'Other',
      description: 'Copy of passport',
      relatesTo: 'TCN — Omar',
      uploadedBy: 'L. Papadopoulou',
      uploadDateTime: '22/06/2026 14:22',
      fileName: 'passport_omar.pdf',
      fileUrl: '#',
    },
    {
      documentType: 'Other',
      description: 'Birth certificate',
      relatesTo: 'TCN — Sara',
      uploadedBy: 'L. Papadopoulou',
      uploadDateTime: '01/07/2026 11:05',
      fileName: 'birth_cert_sara.pdf',
      fileUrl: '#',
    },
  ],
  historyLogs: [
    {
      timestamp: '2026-09-15 09:10',
      user: 'Maria Daskalaki',
      role: 'Case Officer',
      action: 'Security check requested',
      comments: 'Police clearance submitted for verification.',
    },
    {
      timestamp: '2026-09-12 14:25',
      user: 'Petros Georgiou',
      role: 'Case Manager',
      action: 'Travel document reviewed',
      comments: 'Passport listed and expiration date confirmed.',
    },
    {
      timestamp: '2026-09-09 11:40',
      user: 'Nicolas Arvanitis',
      role: 'Counsellor',
      action: 'Interview updated',
      comments: 'Language support requirement and family profile updated.',
    },
    {
      timestamp: '2026-09-05 16:05',
      user: 'System',
      role: 'Automation',
      action: 'Case created',
      comments: 'Application received and assigned to the active caseload.',
    },
  ],
};

export interface CaseInfoSectionData {
  title: string;
  rows: CaseInfoRow[];
}

export const getCounsellingWelcomeLanguageSection = (
  caseData: CaseDetailsData,
): CaseInfoSectionData => ({
  title: 'Section A — Welcome & Language',
  rows: [
    {
      code: 'A1',
      label: 'Please confirm your full name.',
      value: `${caseData.tcnName} — confirmed`,
    },
    {
      code: 'A2',
      label: 'Country of origin / return country.',
      value: `${caseData.nationality} → ${caseData.nationality}`,
    },
    {
      code: 'A3',
      label: 'Preferred language for this session.',
      value: caseData.counsellingSummary.language,
    },
    {
      code: 'A4',
      label: 'Do you understand and speak English?',
      value: 'Some',
    },
    {
      code: 'A5',
      label: 'Is an interpreter required?',
      value: caseData.counsellingSummary.interpreterRequired ? 'Yes' : 'No',
    },
    {
      code: 'A6',
      label: 'Interpreter name / language / mode.',
      value: 'K. Ahmadi — Dari — in person',
    },
  ],
});

export const getCounsellingStatusSection = (caseData: CaseDetailsData): CaseInfoSectionData => ({
  title: 'Section B — Status & current situation',
  rows: [
    {
      code: 'B1',
      label: 'Current status in Cyprus.',
      value: caseData.counsellingSummary.legalStatus,
    },
    {
      code: 'B2',
      label: 'What led you to consider returning now?',
      value:
        'Asylum claim & appeal rejected (final); residence permit expired; wishes to return with husband and daughter.',
    },
    {
      code: 'B3',
      label: 'How did you enter Cyprus?',
      value: 'Regular crossing',
    },
    {
      code: 'B4',
      label: 'How did you hear about the programme?',
      value: 'FRONTEX counsellor',
    },
  ],
});

export const getCounsellingTravelDocumentsSection = (
  caseData: CaseDetailsData,
): CaseInfoSectionData => ({
  title: 'Section C — Travel documents',
  rows: [
    {
      code: 'C1',
      label: 'Do you hold a valid travel document?',
      value: caseData.isTravelDocumentAttached ? 'Yes' : 'No',
    },
    {
      code: 'C2',
      label: 'Document type and number.',
      value: caseData.travelDocumentNumber,
    },
    {
      code: 'C3',
      label: 'Document expiry date.',
      value: caseData.travelDocumentExpiryDate,
    },
    {
      code: 'C4',
      label: 'Where is the original document now?',
      value: 'With applicant',
    },
    {
      code: 'C5',
      label: 'Was the loss/theft reported to police?',
      value: 'not applicable',
      muted: true,
    },
    {
      code: 'C6',
      label: 'Do you have a copy of the document?',
      value: 'Yes — uploaded',
    },
  ],
});

export const getCounsellingLegalFinancialSection = (): CaseInfoSectionData => ({
  title: 'Section D — Legal & financial record',
  rows: [
    {
      code: 'D1',
      label: 'Do you have any pending fines, debts to the State, or criminal cases?',
      value: 'No',
    },
  ],
});

const getCounsellingStatusBranchRows = (legalStatus: string): CaseInfoRow[] => {
  const normalized = legalStatus.toLowerCase();

  if (normalized.includes('asylum')) {
    return [
      {
        code: 'EA1',
        label: 'Do you have your rejection decision?',
        value: 'Yes — uploaded',
      },
      {
        code: 'EA2',
        label: 'Is your asylum appeal still open, or is it closed?',
        value: 'Closed',
      },
    ];
  }

  return [
    {
      code: 'FO1',
      label: 'Please describe your current legal situation in Cyprus.',
      value: 'Rejected asylum seeker awaiting voluntary return.',
    },
  ];
};

export const getCounsellingStatusSpecificSection = (
  caseData: CaseDetailsData,
): CaseInfoSectionData => ({
  title: 'Section E — Status-specific questions',
  rows: getCounsellingStatusBranchRows(caseData.counsellingSummary.legalStatus),
});

export const getCounsellingFamilySection = (): CaseInfoSectionData => ({
  title: 'Section F — Family & accompanying persons',
  rows: [
    {
      code: 'F0',
      label: 'Travelling alone or with family?',
      value: 'With spouse & children',
    },
    {
      code: 'F1',
      label: 'Are you married?',
      value: 'Yes — marriage certificate uploaded',
    },
    {
      code: 'F3',
      label: 'Does each accompanying child have a valid travel document?',
      value: 'Sara: No — Laissez-Passer requested (see Travel Document Issuance)',
    },
    {
      code: 'F4',
      label: 'Where was each child born?',
      value: 'Sara — Nicosia, Cyprus',
    },
    {
      code: 'F5',
      label: "Do you have the child's birth certificate / family book?",
      value: 'Yes — uploaded',
    },
    {
      code: 'F7',
      label: 'Do you have family members in Cyprus?',
      value: 'No further family',
    },
    {
      code: 'F8',
      label: 'Has any family member previously returned via the programme?',
      value: 'No',
    },
  ],
});

export const getCounsellingVulnerabilitySection = (
  caseData: CaseDetailsData,
): CaseInfoSectionData => ({
  title: 'Section G — Vulnerability & needs',
  rows: [
    {
      code: 'G1',
      label: 'Any circumstances we should be aware of?',
      value: caseData.counsellingSummary.vulnerabilityStatus,
    },
    {
      code: 'G2',
      label: 'Do you need accommodation?',
      value: 'No',
    },
    {
      code: 'G3',
      label: 'Any medical condition, or pregnancy?',
      value: 'No',
    },
    {
      code: 'G4',
      label: 'Need assistance at airport / transit?',
      value: 'not applicable',
      muted: true,
    },
  ],
});

export const getCounsellingProgrammeChoiceSection = (
  caseData: CaseDetailsData,
): CaseInfoSectionData => ({
  title: 'Section H — Programme choice & logistics',
  rows: [
    {
      code: 'H1',
      label: 'Officer introduced both return programmes.',
      value: 'Acknowledged',
    },
    {
      code: 'H2',
      label: 'Which programme would you prefer?',
      value: caseData.counsellingSummary.returnProgramme.includes('AVR')
        ? 'AVR Cyprus'
        : caseData.counsellingSummary.returnProgramme,
    },
    {
      code: 'H3',
      label: 'Do you wish to proceed with an AVR?',
      value: 'Yes',
    },
    {
      code: 'H4',
      label: 'Best contact telephone number(s).',
      value: '+357 99 112233',
    },
    {
      code: 'H5',
      label: 'Current address in Cyprus.',
      value: '12 Ledra St., Nicosia',
    },
    {
      code: 'H6',
      label: 'Preferred airport / destination.',
      value: `Larnaca → ${caseData.nationality}`,
    },
  ],
});

export const getOverviewDisplayForm = (caseData: CaseDetailsData): CaseDisplayFormConfig => ({
  formData: {
    tcnId: caseData.tcnId,
    tcnName: caseData.tcnName,
    arc: caseData.arc,
    gender: caseData.gender,
    nationality: caseData.nationality,
    residencyStatus: caseData.residencyStatus,
    ipApplicationStatus: caseData.ipApplicationStatus,
    ipStatus: caseData.ipStatus,
    appealStatus: caseData.appealStatus,
    relationshipToMdFile: caseData.relationshipToMdFile,
  },
  fields: [
    { dataField: 'tcnId', label: 'Recamas ID' },
    { dataField: 'tcnName', label: 'Name/Middle/Surname' },
    { dataField: 'arc', label: 'ARC' },
    { dataField: 'gender', label: 'Gender' },
    { dataField: 'nationality', label: 'Nationality' },
    { dataField: 'residencyStatus', label: 'Residency Status' },
    { dataField: 'ipApplicationStatus', label: 'IP Application Status' },
    { dataField: 'ipStatus', label: 'IP Status' },
    { dataField: 'appealStatus', label: 'Appeal Status' },
    { dataField: 'relationshipToMdFile', label: 'Relationship to MD File' },
  ],
});

export const getReturnDecisionDisplayForm = (caseData: CaseDetailsData): CaseDisplayFormConfig => ({
  formData: {
    decisionId: caseData.returnDecision.decisionId,
    source: caseData.returnDecision.source,
    issuingAuthority: caseData.returnDecision.issuingAuthority,
    decisionDate: caseData.returnDecision.decisionDate,
    tcnReceiptDate: caseData.returnDecision.tcnReceiptDate,
    voluntaryReturnDeadline: caseData.returnDecision.voluntaryReturnDeadline,
    entryBanDuration: caseData.returnDecision.entryBanDuration ?? 'none',
  },
  fields: [
    { dataField: 'decisionId', label: 'Decision ID' },
    { dataField: 'source', label: 'Source' },
    { dataField: 'issuingAuthority', label: 'Issuing Authority' },
    { dataField: 'decisionDate', label: 'Decision Date' },
    { dataField: 'tcnReceiptDate', label: 'TCN Receipt Date' },
    { dataField: 'voluntaryReturnDeadline', label: 'Voluntary Return Deadline' },
    { dataField: 'entryBanDuration', label: 'Entry Ban Duration' },
  ],
});

export const getIssuanceRequestDisplayForm = (
  requestInfo: CaseIssuanceRequestInfo,
  renderStatus: (status: CaseIssuanceRequestInfo['status']) => ReactNode,
): CaseDisplayFormConfig => ({
  formData: {
    tcn: requestInfo.tcn,
    issuanceId: requestInfo.issuanceId,
    requestDate: requestInfo.requestDate,
    documentType: requestInfo.documentType,
    notes: requestInfo.notes,
    requestedBy: requestInfo.requestedBy,
    requestingAuthority: requestInfo.requestingAuthority,
    issuingCountry: requestInfo.issuingCountry,
    issuingAuthority: requestInfo.issuingAuthority,
    issueDate: requestInfo.issueDate,
    status: requestInfo.status,
  },
  fields: [
    { dataField: 'tcn', label: 'TCN' },
    { dataField: 'issuanceId', label: 'Issuance ID' },
    { dataField: 'requestDate', label: 'Request Date' },
    { dataField: 'documentType', label: 'Document Type' },
    { dataField: 'notes', label: 'Notes' },
    { dataField: 'requestedBy', label: 'Requested By' },
    { dataField: 'requestingAuthority', label: 'Requesting Authority' },
    { dataField: 'issuingCountry', label: 'Issuing Country' },
    { dataField: 'issuingAuthority', label: 'Issuing Authority' },
    { dataField: 'issueDate', label: 'Issue Date' },
    {
      dataField: 'status',
      label: 'Status',
      render: (value) => renderStatus(value as CaseIssuanceRequestInfo['status']),
    },
  ],
});

export const getIssuedDocumentDisplayForm = (
  issuedDocument: CaseIssuedDocumentInfo,
  renderAttachment: (name: string, url: string) => ReactNode,
): CaseDisplayFormConfig => {
  const hasAttachment = Boolean(issuedDocument.attachmentName && issuedDocument.attachmentUrl);

  return {
    formData: {
      documentType: issuedDocument.documentType,
      documentNumber: issuedDocument.documentNumber,
      issuingCountry: issuedDocument.issuingCountry,
      issuingAuthority: issuedDocument.issuingAuthority,
      issueDate: issuedDocument.issueDate,
      expirationDate: issuedDocument.expirationDate,
      ...(hasAttachment
        ? {
            attachment: {
              name: issuedDocument.attachmentName,
              url: issuedDocument.attachmentUrl,
            },
          }
        : {}),
    },
    fields: [
      { dataField: 'documentType', label: 'Document Type' },
      { dataField: 'documentNumber', label: 'Document Number' },
      { dataField: 'issuingCountry', label: 'Issuing Country' },
      { dataField: 'issuingAuthority', label: 'Issuing Authority' },
      { dataField: 'issueDate', label: 'Issue Date' },
      { dataField: 'expirationDate', label: 'Expiration Date' },
      ...(hasAttachment
        ? [
            {
              dataField: 'attachment',
              label: 'Attachment',
              render: (value: unknown) => {
                const attachment = value as { name: string; url: string };
                return renderAttachment(attachment.name, attachment.url);
              },
            },
          ]
        : []),
    ],
  };
};

export const getCaseOverviewDisplayForm = (caseData: CaseDetailsData): CaseDisplayFormConfig => ({
  formData: {
    caseId: caseData.caseReference,
    caseTypeLabel: caseData.caseTypeLabel,
    program: caseData.program,
    currentStage: caseData.currentStage,
    statusLabel: caseData.statusLabel,
    assignedOfficer: caseData.assignedOfficer,
    initiationDateTime: caseData.initiationDateTime,
    initiationOffice: caseData.initiationOffice,
    implementationOffice: caseData.implementationOffice,
    internationalFramework: caseData.internationalFramework,
    returnReason: caseData.returnReason,
    nationality: caseData.nationality,
  },
  fields: [
    { dataField: 'caseId', label: 'Case ID' },
    { dataField: 'caseTypeLabel', label: 'Case Type' },
    { dataField: 'program', label: 'Program' },
    { dataField: 'currentStage', label: 'Stage' },
    { dataField: 'statusLabel', label: 'Status' },
    { dataField: 'assignedOfficer', label: 'Assigned To' },
    { dataField: 'initiationDateTime', label: 'Initiation Date/Time' },
    { dataField: 'initiationOffice', label: 'Initiation Office' },
    { dataField: 'implementationOffice', label: 'Implementation Office' },
    { dataField: 'internationalFramework', label: 'International Framework' },
    { dataField: 'returnReason', label: 'Return Reason' },
    { dataField: 'nationality', label: 'Return Country' },
  ],
});

export const getCaseOverviewSubtitle = (caseData: CaseDetailsData): string =>
  `${caseData.caseTypeLabel} · Program: ${caseData.program} · Principal TCN: ${caseData.tcnName} (+${caseData.accompanyingFamilyMembersCount} family members)`;

export const getSecurityChecksDisplayForm = (
  caseData: CaseDetailsData,
  renderCheckboxField: (checked: boolean, label: string) => ReactNode,
): CaseDisplayFormConfig => {
  const { criminalRecordFound, restrictiveActivitiesFound, findings } = caseData.securityChecks;

  return {
    formData: {
      criminalRecordFound: {
        checked: !criminalRecordFound,
        label: criminalRecordFound ? 'Criminal Record Found' : 'No Criminal Record Found',
      },
      restrictiveActivitiesFound: {
        checked: !restrictiveActivitiesFound,
        label: restrictiveActivitiesFound
          ? 'Restrictive Activities Found'
          : 'No Restrictive Activities Found',
      },
      findings,
    },
    fields: [
      {
        dataField: 'criminalRecordFound',
        label: 'Criminal Record',
        render: (value) => {
          const checkboxValue = value as { checked: boolean; label: string };
          return renderCheckboxField(checkboxValue.checked, checkboxValue.label);
        },
      },
      {
        dataField: 'restrictiveActivitiesFound',
        label: 'Restrictive Activities',
        render: (value) => {
          const checkboxValue = value as { checked: boolean; label: string };
          return renderCheckboxField(checkboxValue.checked, checkboxValue.label);
        },
      },
      { dataField: 'findings', label: 'Findings' },
    ],
  };
};

export const getVulnerabilityAndNeedsDisplayForm = (
  caseData: CaseDetailsData,
  renderCheckboxField: (checked: boolean, label: string) => ReactNode,
): CaseDisplayFormConfig => ({
  formData: {
    fitToFly: { checked: caseData.vulnerabilityAndNeeds.fitToFly, label: 'Confirmed' },
    issues: caseData.vulnerabilityAndNeeds.issues,
  },
  fields: [
    {
      dataField: 'fitToFly',
      label: 'Fit-to-Fly',
      render: (value) => {
        const checkboxValue = value as { checked: boolean; label: string };
        return renderCheckboxField(checkboxValue.checked, checkboxValue.label);
      },
    },
    { dataField: 'issues', label: 'Issues' },
  ],
});

export const getTravelDocumentEntryDisplayForm = (
  doc: CaseTravelDocumentEntry,
  renderCheckboxField: (checked: boolean, label: string) => ReactNode,
): CaseDisplayFormConfig => ({
  formData: {
    documentNumber: doc.documentNumber,
    issuingCountry: doc.issuingCountry,
    issuingAuthority: doc.issuingAuthority,
    expirationDate: doc.expirationDate,
    issuedForReturnCase: {
      checked: doc.issuedForReturnCase,
      label: doc.issuedForReturnCaseLabel,
    },
    physicalLocationDelivered: {
      checked: doc.physicalLocationDelivered,
      label: doc.physicalLocationLabel,
    },
  },
  fields: [
    { dataField: 'documentNumber', label: 'Document Number' },
    { dataField: 'issuingCountry', label: 'Issuing Country' },
    { dataField: 'issuingAuthority', label: 'Issuing Authority' },
    { dataField: 'expirationDate', label: 'Expiration Date' },
    {
      dataField: 'issuedForReturnCase',
      label: 'Issued for Return Case',
      render: (value) => {
        const checkboxValue = value as { checked: boolean; label: string };
        return renderCheckboxField(checkboxValue.checked, checkboxValue.label);
      },
    },
    {
      dataField: 'physicalLocationDelivered',
      label: 'Physical Location / Delivered',
      render: (value) => {
        const checkboxValue = value as { checked: boolean; label: string };
        return renderCheckboxField(checkboxValue.checked, checkboxValue.label);
      },
    },
  ],
});

export const getImplementationOverviewDisplayForm = (
  overview: CaseImplementationOverview,
  renderStatus: (status: string) => ReactNode,
): CaseDisplayFormConfig => ({
  formData: {
    implementationId: overview.implementationId,
    implementationType: overview.implementationType,
    initiationDateTime: overview.initiationDateTime,
    plannedExecutionDate: overview.plannedExecutionDate,
    destinationCountry: overview.destinationCountry,
    status: overview.status,
  },
  fields: [
    { dataField: 'implementationId', label: 'Implementation ID' },
    { dataField: 'implementationType', label: 'Implementation Type' },
    { dataField: 'initiationDateTime', label: 'Initiation Date/Time' },
    { dataField: 'plannedExecutionDate', label: 'Planned Execution Date' },
    { dataField: 'destinationCountry', label: 'Destination Country' },
    {
      dataField: 'status',
      label: 'Status',
      render: (value) => renderStatus(value as string),
    },
  ],
});

export const preReturnChecklistColumns: CaseGridColumnConfig<CasePreReturnChecklistRow>[] = [
  { dataField: 'tcn', caption: 'TCN' },
  { dataField: 'travelDocumentExists', caption: 'Travel Document Exists', dataType: 'boolean' },
  {
    dataField: 'travelDocumentDelivered',
    caption: 'Travel Document Delivered',
    dataType: 'boolean',
  },
  {
    dataField: 'travelDocumentReceived',
    caption: 'Travel Document Received',
    dataType: 'boolean',
  },
  { dataField: 'fitToFly', caption: 'Fit-to-Fly', dataType: 'boolean' },
  {
    dataField: 'preReturnActivitiesCompleted',
    caption: 'Pre-Return Activities Completed',
    dataType: 'boolean',
  },
];

export const implementationChecklistColumns: CaseGridColumnConfig<CaseImplementationChecklistRow>[] =
  [
    { dataField: 'item', caption: 'Item' },
    { dataField: 'complete', caption: 'Status', dataType: 'boolean' },
    { dataField: 'note', caption: 'Note' },
  ];

export const historyLogColumns: CaseGridColumnConfig<CaseHistoryEntry>[] = [
  { dataField: 'timestamp', caption: 'Timestamp', width: 150 },
  { dataField: 'user', caption: 'User', width: 180 },
  { dataField: 'role', caption: 'Role', width: 150 },
  { dataField: 'action', caption: 'Action / Event', width: 220 },
  { dataField: 'comments', caption: 'Comments' },
];

export const getUploadedDocumentsColumns = (
  renderAttachment: (name: string, url: string) => ReactNode,
): CaseGridColumnConfig<CaseUploadedDocument>[] => [
  { dataField: 'documentType', caption: 'Document Type' },
  { dataField: 'description', caption: 'Description' },
  { dataField: 'relatesTo', caption: 'Relates To' },
  { dataField: 'uploadedBy', caption: 'Uploaded By' },
  { dataField: 'uploadDateTime', caption: 'Upload Date/Time' },
  {
    dataField: 'fileName',
    caption: 'File',
    cellRender: ({ data }) => renderAttachment(data.fileName, data.fileUrl),
  },
];

const approvalFieldLabels: Record<
  Exclude<keyof CaseReturnDecisionApproval, 'decisionId' | 'status'>,
  string
> = {
  documentName: 'Document Name',
  documentLanguage: 'Document Language',
  tcns: 'TCNs',
  approvedBy: 'Approved By',
  decisionDateTime: 'Decision Date/Time',
  voluntaryDepartureDeadline: 'Voluntary Departure Deadline',
  approverNotes: 'Approver Notes',
  assessmentDecision: 'Assessment Decision',
};

export const buildApprovalDisplayFields = (
  doc: CaseReturnDecisionApproval,
): CaseDisplayFormConfig => {
  const formData: Record<string, unknown> = {};
  const fields: DisplayFieldConfig[] = [];

  (Object.keys(approvalFieldLabels) as Array<keyof typeof approvalFieldLabels>).forEach((key) => {
    const value = doc[key];
    if (value === undefined || value === null || value === '') return;
    formData[key] = value;
    fields.push({ dataField: key, label: approvalFieldLabels[key] });
  });

  return { formData, fields };
};

const requestFieldLabels: Record<
  Exclude<
    keyof CaseRequestEntry,
    'requestId' | 'title' | 'status' | 'attachmentName' | 'attachmentUrl'
  >,
  string
> = {
  requestType: 'Request Type',
  requestedBy: 'Requested By',
  recipientType: 'Recipient Type',
  requestedTo: 'Requested To',
  requestInitiationDateTime: 'Request Initiation Date/Time',
  requestAnswerDateTime: 'Request Answer Date/Time',
  requestNotes: 'Request Notes',
  answerNotes: 'Answer Notes',
};

export const buildRequestDisplayFields = (
  doc: CaseRequestEntry,
  renderAttachment: (name: string, url: string) => ReactNode,
): CaseDisplayFormConfig => {
  const formData: Record<string, unknown> = {};
  const fields: DisplayFieldConfig[] = [];

  (Object.keys(requestFieldLabels) as Array<keyof typeof requestFieldLabels>).forEach((key) => {
    const value = doc[key];
    if (value === undefined || value === null || value === '') return;
    formData[key] = value;
    fields.push({ dataField: key, label: requestFieldLabels[key] });
  });

  if (doc.attachmentName && doc.attachmentUrl) {
    formData.attachments = { name: doc.attachmentName, url: doc.attachmentUrl };
    fields.push({
      dataField: 'attachments',
      label: 'Attachments',
      render: (value) => {
        const attachment = value as { name: string; url: string };
        return renderAttachment(attachment.name, attachment.url);
      },
    });
  }

  return { formData, fields };
};

export const getCounsellingQuestionnaireSections = (
  caseData: CaseDetailsData,
): CaseInfoSectionData[] => [
  getCounsellingWelcomeLanguageSection(caseData),
  getCounsellingStatusSection(caseData),
  getCounsellingTravelDocumentsSection(caseData),
  getCounsellingLegalFinancialSection(),
  getCounsellingStatusSpecificSection(caseData),
  getCounsellingFamilySection(),
  getCounsellingVulnerabilitySection(caseData),
  getCounsellingProgrammeChoiceSection(caseData),
];
