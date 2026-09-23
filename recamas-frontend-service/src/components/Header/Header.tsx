import styles from './Header.module.css';

type HeaderProps = {
  collapsed: boolean;
  onMenuToggle: () => void;
};

export function Header({ collapsed, onMenuToggle }: HeaderProps) {
  const menuLabel = collapsed ? 'Ανάπτυξη μενού' : 'Σύμπτυξη μενού';

  return (
    <header className={styles.header}>
      <div className={styles.brand}>
        <button
          className={`${styles.iconButton} ${styles.menuButton}`}
          type="button"
          onClick={onMenuToggle}
          aria-label={menuLabel}
          title={menuLabel}
        >
          <i className="dx-icon dx-icon-menu" aria-hidden="true" />
        </button>
        <img className={styles.govcyLogo} src="/brand/govcy-logo-white.svg" alt="gov.cy" />
        <div className={styles.serviceTitle}>
          <strong className={styles.title}>CIT-RECAMAS</strong>
          <span className={styles.subtitle}>Deputy Ministry of Migration and International Protection</span>
        </div>
      </div>
    </header>
  );
}
