import { useState } from 'react';
import type * as React from 'react';
import styles from './Container.module.css';

interface ContainerProps {
  children: React.ReactNode;
  expandable?: boolean;
  title?: React.ReactNode;
  defaultExpanded?: boolean;
}

export function Container({
  children,
  expandable = false,
  title,
  defaultExpanded = true,
}: ContainerProps) {
  const [expanded, setExpanded] = useState(defaultExpanded);

  if (!expandable) {
    return (
      <div className={styles.container}>
        {title ? <div className={styles.title}>{title}</div> : null}
        {children}
      </div>
    );
  }

  return (
    <div className={styles.container}>
      <button
        type="button"
        className={styles.toggle}
        onClick={() => setExpanded((prev) => !prev)}
        aria-expanded={expanded}
      >
        <span>{title}</span>
        <i
          className={`dx-icon dx-icon-chevrondown ${expanded ? styles.iconExpanded : styles.icon}`}        />
      </button>
      {expanded && <div className={styles.content}>{children}</div>}
    </div>
  );
}
