import { CheckBox } from 'devextreme-react/check-box';
import { Column } from 'devextreme-react/data-grid';

import { Badge } from '@/components/Badge/Badge';
import { Container } from '@/components/Containers/Container/Container';
import { DisplayForm } from '@/components/DisplayForm/DisplayForm';
import { DxDataGrid } from '@/components/DxDataGrid/DxDataGrid';
import type { TabConfig } from '@/components/Tabs/Tabs';
import {
  approvalStatusColor,
  buildApprovalDisplayFields,
  buildRequestDisplayFields,
  getCounsellingQuestionnaireSections,
  getFreeformStatusColor,
  getImplementationOverviewDisplayForm,
  getIssuanceRequestDisplayForm,
  getIssuedDocumentDisplayForm,
  getOverviewDisplayForm,
  getReturnDecisionDisplayForm,
  getSecurityChecksDisplayForm,
  getTravelDocumentEntryDisplayForm,
  getUploadedDocumentsColumns,
  getVulnerabilityAndNeedsDisplayForm,
  implementationChecklistColumns,
  issuanceStatusColor,
  preReturnChecklistColumns,
  requestStatusColor,
} from '@/features/Cases/Details/caseDetailsMockData';
import type {
  CaseDetailsData,
  CaseIssuanceRequestInfo,
} from '@/features/Cases/Details/caseDetailsTypes';
import i18n from '@/localization/i18n';
import { CaseInfoSection } from './CaseInfoSection';
import styles from './CaseDetailsTabs.module.css';

const tabTitle = (key: string) => i18n.t(`cases.Details.tabs.${key}.tabTitle`);

const renderCheckboxField = (checked: boolean, label: string) => (
  <CheckBox value={checked} readOnly text={label} />
);

const renderIssuanceStatus = (status: CaseIssuanceRequestInfo['status']) => (
  <Badge text={status} dotColor={issuanceStatusColor[status]} />
);

const renderFreeformStatus = (status: string) => (
  <Badge text={status} dotColor={getFreeformStatusColor(status)} />
);

const renderAttachmentLink = (name: string, url: string) => (
  <a href={url} target="_blank" rel="noreferrer" className={styles.attachmentLink}>
    {name}
  </a>
);

export function getCaseDetailsTabs(caseData: CaseDetailsData): TabConfig[] {
  return [
    {
      key: 'councelling',
      title: tabTitle('councelling'),
      render: () => (
        <>
          {getCounsellingQuestionnaireSections(caseData).map((section) => (
            <Container key={section.title} title={section.title} expandable={true}>
              <CaseInfoSection rows={section.rows} />
            </Container>
          ))}
        </>
      ),
    },
    {
      key: 'caseDetails',
      title: tabTitle('caseDetails'),
      render: () => (
        <>
          <Container>
            <h4>Overview</h4>
            <DisplayForm colCount={5} {...getOverviewDisplayForm(caseData)} />
          </Container>

          <Container>
            <h4>Security Checks</h4>
            <DisplayForm
              colCount={3}
              {...getSecurityChecksDisplayForm(caseData, renderCheckboxField)}
            />
          </Container>

          <Container>
            <div className={styles.headerRow}>
              <div className={styles.titleGroup}>
                <h4 className={styles.h4Reset}>Travel Documents</h4>
                <Badge text="Each document = separate entry" variant="outline" />
              </div>
              <span className={styles.label}>
                {caseData.travelDocuments.length} ENTR
                {caseData.travelDocuments.length === 1 ? 'Y' : 'IES'}
              </span>
            </div>

            <div className={styles.stack}>
              {caseData.travelDocuments.map((doc, index) => (
                <Container key={`${doc.documentType}-${index}`}>
                  <div className={styles.headerRow}>
                    <div className={styles.titleGroup}>
                      <strong>{doc.documentType}</strong>
                      <Badge text={doc.profileBadge} />
                    </div>
                    <CheckBox
                      value={doc.canBeUsedForReturn}
                      readOnly
                      text="Can be used for Return"
                    />
                  </div>

                  <div className={styles.mt12}>
                    <DisplayForm
                      colCount={4}
                      {...getTravelDocumentEntryDisplayForm(doc, renderCheckboxField)}
                    />
                  </div>
                </Container>
              ))}
            </div>
          </Container>

          <Container>
            <div className={styles.headerRowTight}>
              <h4 className={styles.h4Reset}>Return Decision</h4>
              <Badge text="Case" />
            </div>

            <DisplayForm colCount={4} {...getReturnDecisionDisplayForm(caseData)} />
          </Container>

          <Container>
            <div className={styles.headerRowTight}>
              <h4 className={styles.h4Reset}>Vulnerability and Needs</h4>
              <Badge text="Case" />
            </div>

            <DisplayForm
              colCount={1}
              {...getVulnerabilityAndNeedsDisplayForm(caseData, renderCheckboxField)}
            />
          </Container>
        </>
      ),
    },
    {
      key: 'approvalItems',
      title: tabTitle('approvalItems'),
      render: () => (
        <Container>
          <h4>Return Decisions</h4>

          <div className={styles.stack}>
            {caseData.returnDecisionApprovals.map((doc) => {
              const { formData, fields } = buildApprovalDisplayFields(doc);
              return (
                <Container key={doc.decisionId}>
                  <div className={styles.headerRowStart}>
                    <strong>{doc.decisionId}</strong>
                    <Badge text={doc.status} dotColor={approvalStatusColor[doc.status]} />
                  </div>

                  <div className={styles.mt8}>
                    <DisplayForm colCount={5} formData={formData} fields={fields} />
                  </div>
                </Container>
              );
            })}
          </div>
        </Container>
      ),
    },
    {
      key: 'requests',
      title: tabTitle('requests'),
      render: () => (
        <Container>
          <h4 className={styles.h4Reset}>Requests</h4>

          <div className={styles.stack}>
            {caseData.requests.map((doc) => {
              const { formData, fields } = buildRequestDisplayFields(doc, renderAttachmentLink);
              return (
                <Container key={doc.requestId}>
                  <div className={styles.headerRowStart}>
                    <strong>
                      {doc.requestId} — {doc.title}
                    </strong>
                    <Badge text={doc.status} dotColor={requestStatusColor[doc.status]} />
                  </div>

                  <div className={styles.mt8}>
                    <DisplayForm colCount={5} formData={formData} fields={fields} />
                  </div>
                </Container>
              );
            })}
          </div>
        </Container>
      ),
    },
    {
      key: 'travelDocuments',
      title: tabTitle('travelDocuments'),
      render: () => {
        const { requestInfo, issuedDocument } = caseData.travelDocumentIssuance;
        return (
          <Container>
            <Container>
              <div className={`${styles.titleGroup} ${styles.mb12}`}>
                <h4 className={styles.h4Reset}>Issuance Request Information</h4>
                <Badge text={requestInfo.enabledStageBadge} variant="outline" />
              </div>

              <DisplayForm
                colCount={5}
                {...getIssuanceRequestDisplayForm(requestInfo, renderIssuanceStatus)}
              />
            </Container>

            <div className={styles.mt16}>
              <Container>
                <h4 className={`${styles.h4Reset} ${styles.mb12}`}>Issued Document Information</h4>

                <DisplayForm
                  colCount={5}
                  {...getIssuedDocumentDisplayForm(issuedDocument, renderAttachmentLink)}
                />
              </Container>
            </div>
          </Container>
        );
      },
    },
    {
      key: 'preReturn',
      title: tabTitle('preReturn'),
      render: () => {
        const checklist = caseData.preReturnChecklist;
        const completeCount = checklist.filter(
          (row) =>
            row.travelDocumentExists &&
            row.travelDocumentDelivered &&
            row.travelDocumentReceived &&
            row.fitToFly &&
            row.preReturnActivitiesCompleted,
        ).length;

        return (
          <Container>
            <div className={`${styles.headerRow} ${styles.mb12}`}>
              <div className={styles.titleGroup}>
                <h4 className={styles.h4Reset}>Pre-Return Checklist</h4>
                <Badge text="Per TCN included in the case" variant="outline" />
              </div>
              <span className={styles.label}>
                {completeCount} OF {checklist.length} COMPLETE
              </span>
            </div>

            <DxDataGrid dataSource={checklist}>
              {preReturnChecklistColumns.map((col) => (
                <Column key={col.dataField} {...col} />
              ))}
            </DxDataGrid>
          </Container>
        );
      },
    },
    {
      key: 'returnImplementation',
      title: tabTitle('returnImplementation'),
      render: () => {
        const { overview, checklist } = caseData.returnImplementation;
        return (
          <>
            <Container>
              <div className={styles.headerRowTight}>
                <h4>Implementation Overview</h4>
                <Badge text={overview.badge} variant="maroon" />
              </div>

              <DisplayForm
                colCount={5}
                {...getImplementationOverviewDisplayForm(overview, renderFreeformStatus)}
              />
            </Container>

            <div className={styles.mt16}>
              <Container>
                <div className={`${styles.titleGroup} ${styles.mb12}`}>
                  <h4>Implementation Checklist</h4>
                  <Badge text="Per TCN included in the case" variant="outline" />
                </div>

                <DxDataGrid dataSource={checklist}>
                  {implementationChecklistColumns.map((col) => (
                    <Column key={col.dataField} {...col} />
                  ))}
                </DxDataGrid>
              </Container>
            </div>
          </>
        );
      },
    },
    {
      key: 'caseDocuments',
      title: tabTitle('caseDocuments'),
      render: () => (
        <Container>
          <div className={`${styles.headerRow} ${styles.mb12}`}>
            <div className={styles.titleGroup}>
              <h4 className={styles.h4Reset}>Uploaded Documents</h4>
              <Badge text="Case · TCN(s)" variant="outline" />
            </div>
            <span className={styles.label}>{caseData.uploadedDocuments.length} FILES</span>
          </div>

          <DxDataGrid dataSource={caseData.uploadedDocuments}>
            {getUploadedDocumentsColumns(renderAttachmentLink).map((col) => (
              <Column key={col.dataField} {...col} />
            ))}
          </DxDataGrid>
        </Container>
      ),
    },
  ];
}
