import type { ReactNode } from 'react';
import styles from './PageContainer.module.css';

type PageContainerProps = {
  title: string;
  eyebrow?: string;
  intro?: string;
  children: ReactNode;
};

export function PageContainer({ title, eyebrow, intro, children }: PageContainerProps) {
  return (
    <section>
      {eyebrow && <p className={styles.eyebrow}>{eyebrow}</p>}
      <h1>{title}</h1>
      {intro && <p className={styles.intro}>{intro}</p>}
      {children}
    </section>
  );
}
