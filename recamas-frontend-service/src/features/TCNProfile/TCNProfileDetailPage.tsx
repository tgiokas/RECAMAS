import { AvatarDisplayForm } from '@/components/AvatarDisplayForm/AvatarDisplayForm';
import { PageContainer } from '@/components/Containers/PageContainer/PageContainer';
import type { TabConfig } from '@/components/Tabs/Tabs';
import { Tabs } from '@/components/Tabs/Tabs';
import { headerBottomBadges, headerGroups } from './dummyData';
import CasesTab from './tabs/CasesTab';
import OverviewTab from './tabs/OverviewTab';
import ResidencyTab from './tabs/ResidencyTab';
import SecurityTab from './tabs/SecurityTab';

const profileTabs: TabConfig[] = [
  { key: 'overview', title: 'Overview', render: () => <OverviewTab /> },
  {
    key: 'residency',
    title: 'Residency, IP & Decisions',
    render: () => <ResidencyTab />,
  },
  {
    key: 'security',
    title: 'Security & Arrival History',
    render: () => <SecurityTab />,
  },
  {
    key: 'cases',
    title: 'Cases & Linked Profiles',
    render: () => <CasesTab />,
  },
];

export function TCNProfileDetailPage() {
  return (
    <>
      <div style={{ position: 'relative' }}>
        <span
          style={{
            position: 'absolute',
            top: 0,
            right: 0,
            display: 'inline-flex',
            alignItems: 'center',
            gap: 5,
            padding: '3px 10px',
            borderRadius: 999,
            background: '#eaf6ee',
            color: 'var(--govcy-success, #00703c)',
          }}
        >
          <i className="dx-icon dx-icon-isnotblank" style={{ fontSize: 12 }}></i>
          <span>Active</span>
        </span>
        <PageContainer
          title="RAHIMI Nadia"
          eyebrow="CIT-RECAMAS TCN Profiles/RAHIMI Nadia (TCN-00412)"
        >
          <AvatarDisplayForm
            avatar="NR"
            groups={headerGroups}
            bottomBadges={headerBottomBadges}
            colCount={4}
          />
          <div style={{ marginTop: 40 }}>
            <Tabs tabs={profileTabs} />
          </div>
        </PageContainer>
      </div>
    </>
  );
}
