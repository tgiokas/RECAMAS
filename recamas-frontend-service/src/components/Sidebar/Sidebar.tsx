import { NavLink } from 'react-router-dom';
import { useTranslation } from 'react-i18next';
import { navigationItems } from '../../config/navigation';
import styles from './Sidebar.module.css';

type SidebarProps = {
  collapsed: boolean;
};

export function Sidebar({ collapsed }: SidebarProps) {
  const { t } = useTranslation();

  return (
    <aside className={`${styles.sidebar}${collapsed ? ` ${styles.collapsed}` : ''}`}>
      <nav className={styles.nav} aria-label={t('navigation.ariaLabel')}>
        {navigationItems.map((item) => (
          <NavLink
            key={item.path}
            to={item.path}
            end={item.path === '/'}
            className={({ isActive }) => `${styles.navLink}${isActive ? ` ${styles.navLinkActive}` : ''}`}
            title={collapsed ? t(item.labelKey) : undefined}
          >
            <i className={`dx-icon dx-icon-${item.icon} ${styles.navIcon}`} aria-hidden="true" />
            {!collapsed && <span className={styles.navLabel}>{t(item.labelKey)}</span>}
          </NavLink>
        ))}
      </nav>
      {!collapsed && <span className={styles.version}>RECAMAS v1.0</span>}
    </aside>
  );
}
