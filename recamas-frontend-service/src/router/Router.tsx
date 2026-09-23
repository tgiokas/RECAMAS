import { CaseManagementPage } from '@/features/Cases/CaseManagementPage';
import { CaseDetailsPage } from '@/features/Cases/Details/CaseDetailsPage';
import { RecamasProfilesPage } from '@/pages/RecamasProfilesPage/RecamasProfilesPage';
import { createBrowserRouter } from 'react-router-dom';
import { navigationItems } from '../config/navigation';
import { TCNProfileDetailPage } from '../features/TCNProfile/TCNProfileDetailPage';
import { WorkspacePage } from '../features/Workspace/WorkspacePage';
import { MainLayout } from '../layouts/MainLayout';
import { TcnSearchPage } from '../pages/TcnSearchPage/TcnSearchPage';

export const router = createBrowserRouter([
  {
    path: '/',
    element: <MainLayout />,
    children: [
      { index: true, element: <TcnSearchPage /> },
      { path: 'tcn-search', element: <TcnSearchPage /> },
      { path: 'tcn-profiles', element: <TCNProfileDetailPage /> },
      { path: 'recamas-profiles', element: <RecamasProfilesPage /> },
      { path: 'cases', element: <CaseManagementPage /> },
      { path: 'cases/:caseId', element: <CaseDetailsPage /> },
      ...navigationItems
        .filter((item) => item.path !== '/')
        .map((item) => ({
          path: item.path.slice(1),
          element: <WorkspacePage titleKey={item.labelKey} />,
        })),
    ],
  },
]);
