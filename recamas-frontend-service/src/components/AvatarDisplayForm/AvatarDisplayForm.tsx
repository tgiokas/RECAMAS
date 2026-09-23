import type {
  DisplayBadgeConfig,
  DisplayFormHandle,
  DisplayFormProps,
} from '@/components/DisplayForm/DisplayForm';
import { DisplayForm } from '@/components/DisplayForm/DisplayForm';
import type { ReactNode } from 'react';
import { forwardRef } from 'react';
import styles from './AvatarDisplayForm.module.css';

function resolveBadge(badge: DisplayBadgeConfig | string): DisplayBadgeConfig {
  return typeof badge === 'string' ? { text: badge, variant: 'primary' } : badge;
}
export interface AvatarDisplayFormProps extends DisplayFormProps {
  avatar?: ReactNode;
  bottomBadges?: (DisplayBadgeConfig | string)[];
  wrapperClassName?: string;
}

export const AvatarDisplayForm = forwardRef<DisplayFormHandle, AvatarDisplayFormProps>(
  function AvatarDisplayForm(
    { avatar, bottomBadges, wrapperClassName, children, ...displayFormProps },
    ref,
  ) {
    const badgesContent = bottomBadges && bottomBadges.length > 0 && (
      <div className={styles.bottomBadges}>
        {bottomBadges.map((badge, idx) => {
          const b = resolveBadge(badge);
          return (
            <span
              key={idx}
              className={`dx-badge ${styles.bottomBadge} ${styles[`bottomBadge--${b.variant ?? 'default'}`] ?? ''}`.trim()}
              style={{
                ...(b.color ? { color: b.color } : {}),
                ...(b.backgroundColor ? { backgroundColor: b.backgroundColor } : {}),
              }}
            >
              {b.showDot && <span className={styles.bottomBadgeDot} />}
              {b.text}
            </span>
          );
        })}
      </div>
    );

    return (
      <div className={`${styles.wrapper} ${wrapperClassName ?? ''}`.trim()}>
        {avatar && <div className={styles.avatar}>{avatar}</div>}
        <div className={styles.content}>
          <DisplayForm ref={ref} {...displayFormProps}>
            {children}
          </DisplayForm>
          {badgesContent}
        </div>
      </div>
    );
  },
);

AvatarDisplayForm.displayName = 'AvatarDisplayForm';
