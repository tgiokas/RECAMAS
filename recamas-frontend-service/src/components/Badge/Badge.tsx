import styles from './Badge.module.css';

export type BadgeVariant =
  | 'navy'
  | 'outline'
  | 'green'
  | 'red'
  | 'blue'
  | 'orange'
  | 'gray'
  | 'maroon';

export interface BadgeProps {
  text: string;
  variant?: BadgeVariant;
  /** Renders a small colored status dot before the text instead of a solid pill background */
  dotColor?: string;
}

const variantClassNames: Record<BadgeVariant, string> = {
  navy: 'badgeNavy',
  outline: 'badgeOutline',
  green: 'badgeGreen',
  red: 'badgeRed',
  blue: 'badgeBlue',
  orange: 'badgeOrange',
  gray: 'badgeGray',
  maroon: 'badgeMaroon',
};

export function Badge({ text, variant = 'navy', dotColor }: BadgeProps) {
  if (dotColor) {
    return (
      <div className={styles.statusRow}>
        <span className={styles.statusDot} style={{ background: dotColor }} />
        <span className={styles.statusText}>{text}</span>
      </div>
    );
  }

  return <span className={styles[variantClassNames[variant]]}>{text}</span>;
}
