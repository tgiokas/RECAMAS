import { PageContainer } from '@/components/Containers/PageContainer/PageContainer';
import { Container } from '@/components/Containers/Container/Container';
import { RecamasProfilesGrid } from '@/features/recamasProfiles/components/RecamasProfilesGrid';

export function RecamasProfilesPage() {
  return (
    <PageContainer
      eyebrow="RECAMAS"
      title="Recamas Profiles"
      intro="Review existing Recamas profiles. Add or update a profile from TCN Search, or open the TCN profile record to view details."
    >
      <Container>
        <RecamasProfilesGrid />
      </Container>
    </PageContainer>
  );
}
