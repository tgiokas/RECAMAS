import { useNavigate } from 'react-router-dom';
import { Table } from '@/components/Table/Table';
import type { IColumnProps } from '@/components/Table/Table';
import type { GridAction } from '@/components/types/tableRowActionButton';
import Button from '@/components/Button/Button';
import styles from '@/features/recamasProfiles/RecamasProfilesPage.module.css';
import { recamasProfilesMock } from '@/features/recamasProfiles/config/recamasProfilesMock';
import type { RecamasProfile } from '@/features/recamasProfiles/recamasProfileTypes';

const columns: IColumnProps<RecamasProfile>[] = [
  { caption: 'Profile ID', dataField: 'profileId' },
  { caption: 'ARC', dataField: 'arc' },
  { caption: 'First Name', dataField: 'firstName' },
  { caption: 'Last Name', dataField: 'lastName' },
  { caption: 'Nationality', dataField: 'nationality' },
  {
    caption: 'Date of Birth',
    dataField: 'dateOfBirth',
    render: (value) => (value ? new Date(value as string).toLocaleDateString('en-GB') : ''),
  },
  { caption: 'Passport No.', dataField: 'passportNo' },
  { caption: 'MD File No.', dataField: 'mdFileNo' },
  { caption: 'Status', dataField: 'status' },
];

export function RecamasProfilesGrid() {
  const navigate = useNavigate();
  const profiles = [...recamasProfilesMock];

  const rowActions: GridAction<RecamasProfile>[] = [
    {
      label: 'View',
      icon: 'info',
      onClick: (row) => {
        void navigate('/tcn-profiles', { state: { profile: row } });
      },
    },
    {
      label: 'Update',
      icon: 'edit',
      onClick: (row) => {
        void navigate('/tcn-search', { state: { profile: row } });
      },
    },
  ];

  return (
    <div>
      <div className={styles.toolbar}>
        <h2 className={styles.title}>Profiles</h2>
      </div>
      <Table
        dataSource={profiles}
        columns={columns}
        rowActions={rowActions}
        toolbarLeft={
          <Button
            text="Add new profile"
            onClick={() => {
              void navigate('/tcn-search');
            }}
            stylingMode="outlined"
            icon='plus'
          />
        }
      />
    </div>
  );
}
