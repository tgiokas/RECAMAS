import { useParams } from 'react-router-dom';

import { PageContainer } from '@/components/Containers/PageContainer/PageContainer';
import CaseDetailsView, { mockCaseDetailData } from '../components/CaseDetailsView';

export function CaseDetailsPage() {
  const { caseId } = useParams();

  const caseData = {
    ...mockCaseDetailData,
    caseId: caseId ?? mockCaseDetailData.caseId,
    caseReference: caseId ?? mockCaseDetailData.caseReference,
  };

  return (
    <PageContainer eyebrow="RECAMAS" title="Case Details">
      <CaseDetailsView caseData={caseData} />
    </PageContainer>
  );
}
