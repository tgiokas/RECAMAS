import { Column } from 'devextreme-react/data-grid';
import { Item, Stepper } from 'devextreme-react/stepper';

import { AvatarDisplayForm } from '@/components/AvatarDisplayForm/AvatarDisplayForm';
import { Badge } from '@/components/Badge/Badge';
import Button from '@/components/Button/Button';
import { Container } from '@/components/Containers/Container/Container';
import { PageContainer } from '@/components/Containers/PageContainer/PageContainer';
import { DxDataGrid } from '@/components/DxDataGrid/DxDataGrid';
import { Tabs } from '@/components/Tabs/Tabs';
import {
  defaultCaseStageSteps,
  getCaseOverviewDisplayForm,
  getCaseOverviewSubtitle,
  historyLogColumns,
  mockCaseDetailData,
  toneToBadgeVariant,
} from '@/features/Cases/Details/caseDetailsMockData';
import type { CaseDetailsProps } from '@/features/Cases/Details/caseDetailsTypes';
import { getCaseDetailsTabs } from './CaseDetailsTabs';
import styles from './CaseDetailsView.module.css';

export {
  defaultCaseStageSteps,
  mockCaseDetailData,
} from '@/features/Cases/Details/caseDetailsMockData';
export type {
  CaseCounsellingSummary,
  CaseDetailFlag,
  CaseDetailsData,
  CaseDetailsProps,
  CaseHistoryEntry,
  CaseImplementationChecklistRow,
  CaseImplementationOverview,
  CaseIssuanceRequestInfo,
  CaseIssuedDocumentInfo,
  CasePreReturnChecklistRow,
  CaseRequestEntry,
  CaseReturnDecision,
  CaseReturnDecisionApproval,
  CaseReturnImplementation,
  CaseSecurityChecks,
  CaseTravelDocumentEntry,
  CaseTravelDocumentIssuance,
  CaseUploadedDocument,
  CaseVulnerabilityAndNeeds,
} from '@/features/Cases/Details/caseDetailsTypes';

export default function CaseDetailsView({
  caseData = mockCaseDetailData,
  steps = defaultCaseStageSteps,
}: CaseDetailsProps) {
  const activeStageIndex = steps.indexOf(caseData.currentStage);
  const stepperCurrentStep = activeStageIndex >= 0 ? activeStageIndex : 0;
  const casesTabs = getCaseDetailsTabs(caseData);
  return (
    <PageContainer title="">
      <Container>
        <div className={styles.header}>
          <div>
            <div className={styles.title}>{caseData.caseReference}</div>
            <div className={styles.subtitle}>{getCaseOverviewSubtitle(caseData)}</div>
          </div>

          <Button text="Submit stage" color="primary" stylingMode="contained" />
        </div>

        <div className={styles.overview}>
          <AvatarDisplayForm
            avatar={caseData.type}
            colCount={4}
            {...getCaseOverviewDisplayForm(caseData)}
          />
        </div>

        <div className={styles.flags}>
          {caseData.flags.map((flag) => (
            <Badge key={flag.label} text={flag.label} variant={toneToBadgeVariant[flag.tone]} />
          ))}
        </div>
      </Container>

      <Container title="Case Progression">
        <Stepper
          id="case-progress-stepper"
          defaultSelectedIndex={stepperCurrentStep}
          selectedIndex={stepperCurrentStep}
          orientation="horizontal"
        >
          {steps.map((stage) => (
            <Item key={stage} label={stage} />
          ))}
        </Stepper>
      </Container>

      <Container>
        <Tabs tabs={casesTabs} />
      </Container>

      <Container expandable title="Audit Trail / History Log">
        <DxDataGrid dataSource={caseData.historyLogs}>
          {historyLogColumns.map((col) => (
            <Column key={col.dataField} {...col} />
          ))}
        </DxDataGrid>
      </Container>
    </PageContainer>
  );
}
