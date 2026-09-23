import type { ReactNode } from 'react';

import styles from './CaseInfoSection.module.css';

export interface CaseInfoRow {
  /** Optional short code/id shown before the label (e.g. questionnaire item code) */
  code?: string;
  label: string;
  value?: ReactNode;
  muted?: boolean;
  /** When true, this row is skipped entirely */
  hideRow?: boolean;
}

export interface CaseInfoSectionProps {
  title?: string;
  rows?: CaseInfoRow[];
  listTitle?: string;
  listItems?: string[];
}

export function CaseInfoSection({ title, rows, listTitle, listItems }: CaseInfoSectionProps) {
  const visibleRows = rows?.filter((row) => !row.hideRow) ?? [];

  return (
    <>
      {title && <div className={styles.sectionTitle}>{title}</div>}

      {visibleRows.map((row, index) => (
        <div key={row.code ?? row.label ?? index} className={styles.row}>
          {row.code && <span className={styles.code}>{row.code}</span>}
          <span className={styles.label}>{row.label}</span>
          <span className={`${styles.value} ${row.muted ? styles.valueMuted : ''}`}>
            {row.value}
          </span>
        </div>
      ))}

      {listItems && listItems.length > 0 && (
        <div className={styles.listWrapper}>
          {listTitle && <div className={styles.listTitle}>{listTitle}</div>}
          <div className={styles.listContent}>
            {listItems.map((item) => (
              <span key={item} className={styles.listItem}>
                • {item}
              </span>
            ))}
          </div>
        </div>
      )}
    </>
  );
}
