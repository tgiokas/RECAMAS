import type { DisplayBadgeConfig, DisplayGroupConfig } from '@/components/DisplayForm/DisplayForm';

export const headerGroups: DisplayGroupConfig[] = [
  {
    colCount: 4,
    fields: [
      { label: 'ARC Number', value: 'ARC-0412887' },
      { label: 'MD File Number', value: 'MD-2019-33871' },
      { label: 'CASS File Number', value: 'CS-2019-10042' },
      { label: 'First Name (EL/EN)', value: 'Nadia / Νάντια' },
      { label: 'Middle Name (EL/EN)', value: '—' },
      { label: 'Last Name (EL/EN)', value: 'Rahimi / Ραχίμι' },
      { label: 'Passport / Travel Doc No', value: 'P8839201' },
      { label: 'Date of Birth', value: '12/03/1994' },
      { label: 'Place of Birth', value: 'Kabul, Afghanistan' },
      { label: 'Nationality', value: 'Afghanistan' },
      { label: 'Gender', value: 'Female' },
    ],
  },
];

export const headerBottomBadges: DisplayBadgeConfig[] = [
  { text: 'AVR Application Pending', variant: 'success', showDot: true },
  { text: 'Security Issues', variant: 'default' },
  { text: 'Minor', variant: 'default' },
  { text: 'No ARC', variant: 'default' },
  { text: 'No Travel Document', variant: 'default' },
];

export const personalInfoGroups: DisplayGroupConfig[] = [
  {
    group: {
      title: 'Identity & contact — from the Aliens Registry System',
      badge: { text: 'ARS', variant: 'primary' },
    },
    colCount: 4,
    fields: [
      {
        dataField: 'firstName',
        label: 'First Name (EL/EN)',
        value: 'Nadia / Ναντια',
        editorType: 'dxTextBox',
        editorOptions: { placeholder: 'Enter first name' },
        visible: true,
      },
      {
        dataField: 'middleName',
        label: 'Middle Name (EL/EN)',
        value: null,
        editorType: 'dxTextBox',
        visible: true,
      },
      {
        dataField: 'lastName',
        label: 'Last Name (EL/EN)',
        value: 'Rahimi / Ραχιμι',
        editorType: 'dxTextBox',
        visible: true,
      },
      {
        dataField: 'gender',
        label: 'Gender',
        value: 'Female',
        editorType: 'dxSelectBox',
        editorOptions: { items: ['Female', 'Male', 'Other'] },
        visible: true,
      },
      {
        dataField: 'dateOfBirth',
        label: 'Date of Birth',
        value: '12/03/1994',
        editorType: 'dxDateBox',
        editorOptions: { displayFormat: 'dd/MM/yyyy', type: 'date' },
        visible: true,
      },
      {
        dataField: 'placeOfBirth',
        label: 'Place of Birth',
        value: 'Kabul, Afghanistan',
        editorType: 'dxTextBox',
        visible: true,
      },
      {
        dataField: 'nationality',
        label: 'Nationality',
        value: 'Afghanistan',
        editorType: 'dxTextBox',
        visible: true,
      },
      {
        dataField: 'arc',
        label: 'ARC',
        value: '0412887',
        editorType: 'dxTextBox',
        readOnly: true,
        visible: true,
      },
      {
        dataField: 'mdFileNo',
        label: 'MD File No.',
        value: 'MD-2019-33871',
        editorType: 'dxTextBox',
        disabled: true,
        visible: true,
      },
      {
        dataField: 'relationshipToMdFile',
        label: 'Relationship to MD File',
        value: 'Principal',
        editorType: 'dxTextBox',
        visible: true,
      },
      {
        dataField: 'mdAddress',
        label: 'MD Address',
        value: '12 Ledra St., Nicosia',
        editorType: 'dxTextBox',
        visible: true,
      },
      {
        dataField: 'mdPhone',
        label: 'MD Phone',
        value: '+357 99 112233',
        editorType: 'dxTextBox',
        editorOptions: { mode: 'tel' },
        visible: true,
      },
    ],
  },
  {
    group: {
      title: 'Asylum-side records',
      badge: { text: 'CASS', variant: 'info' },
    },
    colCount: 4,
    fields: [
      {
        dataField: 'cassFileNo',
        label: 'CASS File No.',
        value: 'CS-2019-10042',
        editorType: 'dxTextBox',
        disabled: true,
        visible: true,
      },
      {
        dataField: 'cassAddress',
        label: 'CASS Address',
        value: '12 Ledra St., Nicosia',
        editorType: 'dxTextBox',
        visible: true,
      },
      {
        dataField: 'cassPhone',
        label: 'CASS Phone',
        value: '+357 99 112233',
        editorType: 'dxTextBox',
        editorOptions: { mode: 'tel' },
        visible: true,
      },
      {
        dataField: 'eurodacNumber',
        label: 'EURODAC Number',
        value: null,
        editorType: 'dxTextBox',
        visible: true,
      },
    ],
  },
  {
    group: {
      title: 'System-generated / computed',
      badge: { text: 'RECAMAS', variant: 'success' },
    },
    colCount: 4,
    fields: [
      {
        dataField: 'recamasId',
        label: 'RECAMAS ID',
        value: 'TCN-00412',
        editorType: 'dxTextBox',
        readOnly: true,
        visible: true,
      },
      {
        dataField: 'age',
        label: 'Age',
        value: '32 — calculated from DOB',
        editorType: 'dxTextBox',
        readOnly: true,
        visible: true,
      },
      {
        dataField: 'fingerprints',
        label: 'Fingerprints',
        value: null,
        editorType: 'dxTextBox',
        disabled: true,
        visible: true,
      },
    ],
  },
  {
    group: {
      title: 'Identity & travel documents',
    },
    colCount: 4,
    fields: [
      { dataField: 'documentType', label: 'Document Type', value: 'National Passport' },
      { dataField: 'documentNumber', label: 'Document Number', value: 'P8839201' },
      { dataField: 'issuingCountry', label: 'Issuing Country', value: 'Afghanistan' },
      { dataField: 'issueDate', label: 'Issue Date', value: '10/05/2021', editorType: 'dxDateBox' },
      {
        dataField: 'expiryDate',
        label: 'Expiry Date',
        value: '10/05/2026',
        editorType: 'dxDateBox',
      },
      { dataField: 'physicalLocation', label: 'Physical Location', value: 'Held by TCN' },
      { dataField: 'isTravelDocument', label: 'Is Travel Document', value: 'True' },
      { dataField: 'attachment', label: 'Attachment', value: 'passport_scan.pdf' },
    ],
  },
  {
    group: {
      title: 'Active & historical return cases',
    },
    colCount: 4,
    fields: [
      { dataField: 'caseId', label: 'Case ID', value: 'CAS-2024-0012' },
      { dataField: 'caseType', label: 'Type', value: 'AVR' },
      { dataField: 'caseStage', label: 'Stage', value: 'Counselling' },
      { dataField: 'caseStatus', label: 'Status', value: 'Application Processing' },
      { dataField: 'assignedOfficer', label: 'Assigned Officer', value: 'N. Georgiou' },
      { dataField: 'actions', label: 'Actions', value: 'View Case' },
    ],
  },
];

export const residencyTabGroups: DisplayGroupConfig[] = [
  {
    group: { title: 'Residency status & applications', badge: { text: 'ARS', variant: 'primary' } },
    colCount: 4,
    fields: [
      {
        dataField: 'permitType',
        label: 'Permit / Application Type',
        value: 'Temporary Resident (Visitor)',
        editorType: 'dxSelectBox',
      },
      {
        dataField: 'applicationNumber',
        label: 'Permit / Application No',
        value: 'TR-99201',
      },
      {
        dataField: 'issueDate',
        label: 'Issue / Filing Date',
        value: '15/01/2020',
        editorType: 'dxDateBox',
      },
      {
        dataField: 'expirationDate',
        label: 'Expiration Date',
        value: '15/01/2021',
        editorType: 'dxDateBox',
      },
      {
        dataField: 'applicationStatus',
        label: 'Status',
        value: 'Expired',
        editorType: 'dxSelectBox',
      },
      { dataField: 'details', label: 'Details', value: 'View ARS Record' },
    ],
  },
  {
    group: { title: 'International protection', badge: { text: 'CASS', variant: 'info' } },
    colCount: 4,
    fields: [
      {
        dataField: 'asylumApplicationNumber',
        label: 'Asylum App No',
        value: 'CS-2019-10042',
      },
      {
        dataField: 'applicationDate',
        label: 'Application Date',
        value: '20/02/2019',
        editorType: 'dxDateBox',
      },
      { dataField: 'eurodacNumber', label: 'EURODAC No / Date', value: 'CY20190220123' },
      { dataField: 'asylumStatus', label: 'Status', value: 'Rejected (Final)' },
      {
        dataField: 'outcome',
        label: 'Asylum Decision',
        value: 'Negative Decision',
      },
      {
        dataField: 'decisionDate',
        label: 'Decision Date',
        value: '10/11/2022',
        editorType: 'dxDateBox',
      },
    ],
  },
  {
    group: { title: 'Appeals', badge: { text: 'ΔΔΔΠ / IPAC', variant: 'warning' } },
    colCount: 4,
    fields: [
      { dataField: 'appealReference', label: 'Appeal Reference', value: 'AP-2022-4091' },
      {
        dataField: 'filingDate',
        label: 'Filing Date',
        value: '01/12/2022',
        editorType: 'dxDateBox',
      },
      {
        dataField: 'appealStatus',
        label: 'Appeal Status',
        value: 'Dismissed',
        editorType: 'dxSelectBox',
      },
      {
        dataField: 'finalOutcome',
        label: 'Final Judgment Date',
        value: '15/05/2023',
        editorType: 'dxDateBox',
      },
    ],
  },
  {
    group: { title: 'Return decisions', badge: { text: 'Return', variant: 'danger' } },
    colCount: 4,
    fields: [
      {
        dataField: 'returnReference',
        label: 'Return Decision Ref Number',
        value: 'RET-2023-00912',
      },
      {
        dataField: 'issuingAuthority',
        label: 'Issuing Authority',
        value: 'Civil Registry and Migration Department',
      },
      {
        dataField: 'voluntaryDepartureDeadline',
        label: 'Voluntary Departure Deadline',
        value: '15/06/2023 (Expired)',
      },
      { dataField: 'entryBanDuration', label: 'Entry Ban Duration', value: '3 Years' },
    ],
  },
];

export const securityTabGroups: DisplayGroupConfig[] = [
  {
    group: { title: 'Stoplist record', badge: { text: 'Security', variant: 'default' } },
    colCount: 4,
    fields: [
      { dataField: 'activeStoplist', label: 'Is In Stoplist', value: 'YES ⚠️ Active' },
      {
        dataField: 'stoplistReason',
        label: 'Stoplist Reason',
        value: 'Overstayed / Entry Ban Issued',
      },
      {
        dataField: 'stoplistInsertionDate',
        label: 'Insertion Date',
        value: '16/06/2023',
        editorType: 'dxDateBox',
      },
      {
        dataField: 'stoplistExpirationDate',
        label: 'Expiry Date',
        value: '16/06/2026',
        editorType: 'dxDateBox',
      },
      {
        dataField: 'insertedBy',
        label: 'Inserted By',
        value: 'Aliens & Immigration Unit (YAM)',
      },
    ],
  },
];

export type ProfileCase = {
  id: string;
  type: string;
  stage: string;
  status: string;
  assignedOfficer: string;
};

export type LinkedProfile = {
  id: string;
  name: string;
  relationship: string;
  arc: string;
  nationality: string;
};

export const profileCases: ProfileCase[] = [
  {
    id: 'CAS-2024-0012',
    type: 'AVR',
    stage: 'Counselling',
    status: 'Application Processing',
    assignedOfficer: 'N. Georgiou',
  },
  {
    id: 'CAS-2023-0881',
    type: 'Forced Return',
    stage: 'Decision Issued',
    status: 'Closed',
    assignedOfficer: 'M. Christou',
  },
  {
    id: 'CAS-2025-0144',
    type: 'Dublin Transfer',
    stage: 'Identification',
    status: 'Open',
    assignedOfficer: 'A. Ioannou',
  },
];

export const linkedProfiles: LinkedProfile[] = [
  {
    id: 'TCN-00410',
    name: 'Ahmad Rahimi',
    relationship: 'Spouse',
    arc: 'ARC-0412884',
    nationality: 'Afghanistan',
  },
  {
    id: 'TCN-00413',
    name: 'Omar Rahimi',
    relationship: 'Child',
    arc: 'ARC-0412889',
    nationality: 'Afghanistan',
  },
  {
    id: 'TCN-00414',
    name: 'Layla Rahimi',
    relationship: 'Child',
    arc: '—',
    nationality: 'Afghanistan',
  },
];
