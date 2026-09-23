import { useState } from 'react';
import { Outlet } from 'react-router-dom';
import { Header } from '../components/Header/Header';
import { Sidebar } from '../components/Sidebar/Sidebar';

export function MainLayout() {
  const [collapsed, setCollapsed] = useState(false);

  return (
    <div className={`app-shell${collapsed ? ' app-shell--collapsed' : ''}`}>
      <Header collapsed={collapsed} onMenuToggle={() => setCollapsed((value) => !value)} />
      <Sidebar collapsed={collapsed} />
      <main className="app-main">
        <Outlet />
      </main>
    </div>
  );
}
