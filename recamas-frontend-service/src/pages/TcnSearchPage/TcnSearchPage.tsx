import { PageContainer } from '@/components/Containers/PageContainer/PageContainer';
import { Container } from '@/components/Containers/Container/Container';
import { useTcnSearch } from './hooks/useTcnSearch';
import { TcnSearchForm } from '@/features/tcnSearch/components/TcnSearchForm';
import { TcnSearchResults } from '@/features/tcnSearch/components/TcnSearchResults';

export function TcnSearchPage() {
  const {
    formRef,
    results,
    hasSearched,
    isSearching,
    handleSearch,
    handleClear,
    initialFormData,
    dateEditorOptions,
    nationalityEditorOptions,
  } = useTcnSearch();

  return (
    <PageContainer
      eyebrow="RECAMAS"
      title="TCN Search"
      intro="Search Alien Registration records by identity details. Results exclude photographs and are shown in the grid below."
    >
      <Container>
        <TcnSearchForm
          formRef={formRef}
          initialFormData={initialFormData}
          isSearching={isSearching}
          onSearch={handleSearch}
          onClear={handleClear}
          dateEditorOptions={dateEditorOptions}
          nationalityEditorOptions={nationalityEditorOptions}
        />
      </Container>

      <Container>
        <TcnSearchResults results={results} hasSearched={hasSearched} />
      </Container>
    </PageContainer>
  );
}
