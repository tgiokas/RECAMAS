import { useNavigate } from 'react-router-dom';

import { Container } from '@/components/Containers/Container/Container';
import { PageContainer } from '@/components/Containers/PageContainer/PageContainer';
import { DxDataGrid } from '@/components/DxDataGrid/DxDataGrid';
import { casesMock } from './casesMockData';
import styles from './components/CasesGrid.module.css';
import { getCaseGridProps, renderCaseGridChildren } from './config/casesGridConfig';
import type { RecamasCase } from './Details/casesTypes';

export function CaseManagementPage() {
  const cases = [...casesMock];
  const navigate = useNavigate();

  const handleManageCase = (caseId: string) => {
    navigate(`/cases/${caseId}`);
  };

  const gridProps = getCaseGridProps(cases);

  return (
    <PageContainer eyebrow="RECAMAS" title="Case Management">
      <Container>
        <div className={styles.gridWrapper}>
          <DxDataGrid<RecamasCase> {...gridProps}>
            {renderCaseGridChildren(handleManageCase)}
          </DxDataGrid>
        </div>
      </Container>
    </PageContainer>
  );
}
