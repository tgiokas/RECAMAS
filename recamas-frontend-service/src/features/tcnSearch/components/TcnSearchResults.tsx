import type { TcnSearchResult } from '@/features/tcnSearch';
import styles from '@/features/tcnSearch/TcnSearchPage.module.css';
import { Table } from '@/components/Table/Table';
import type { GridAction } from '@/components/types/tableRowActionButton';
import type { IColumnProps } from '@/components/Table/Table';

type TcnSearchResultsProps = {
  results: TcnSearchResult[];
  hasSearched: boolean;
};

export function TcnSearchResults({ results, hasSearched }: TcnSearchResultsProps) {
  const columns: IColumnProps<TcnSearchResult>[] = [
    { caption: 'ARC', dataField: 'arc' },
    { caption: 'First Name', dataField: 'firstName' },
    { caption: 'Last Name', dataField: 'lastName' },
    { caption: 'Nationality', dataField: 'nationality' },
    { caption: 'Passport No.', dataField: 'passportNo' },
    {
      caption: 'Date of Birth',
      dataField: 'dateOfBirth',
      render: (value) => (value ? new Date(value as string).toLocaleDateString('en-GB') : ''),
    },
    {
      caption: 'System',
      dataField: 'systems',
      render: (value) => (Array.isArray(value) ? value.join(', ') : ''),
    },
  ];

  const rowActions: GridAction<TcnSearchResult>[] = [
    {
      label: 'Add',
      icon: 'plus',
      onClick: (row) => {
        console.log('View TCN record', row);
      },
    },
  ];

  return (
    <div className={styles.results}>
      <h2 className={styles.resultsTitle}>Search results</h2>

      {!hasSearched ? (
        <p className={styles.resultsMeta}>Run a search to display TCN records.</p>
      ) : results.length === 0 ? (
        <p className={styles.resultsMeta}>No TCN records match the search criteria.</p>
      ) : (
        <Table dataSource={results} columns={columns} rowActions={rowActions} />
      )}
    </div>
  );
}
