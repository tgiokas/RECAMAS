import * as React from 'react';
import { createPortal } from 'react-dom';
import styles from './Popover.module.css';

type PopoverProps = {
  children: React.ReactNode;
  visible: boolean;
  onHiding: () => void;
  target?: HTMLElement | null;
  position?: 'bottom' | 'top' | 'left' | 'right';
};

function getPopoverStyle(target: HTMLElement | null | undefined, position: PopoverProps['position']): React.CSSProperties {
  const rect = target?.getBoundingClientRect();
  const style: React.CSSProperties = {
    position: 'fixed',
    zIndex: 50,
    minWidth: 180,
  };

  if (!rect) {
    return style;
  }

  style.left = rect.left;
  style.top = rect.bottom + 8;

  if (position === 'top') {
    style.top = rect.top - 8;
  }

  if (position === 'left') {
    style.left = rect.left - 180;
  }

  if (position === 'right') {
    style.left = rect.right + 8;
  }

  return style;
}

export function Popover({ children, visible, onHiding, target, position = 'bottom' }: PopoverProps) {
  const [style, setStyle] = React.useState<React.CSSProperties>(() => getPopoverStyle(target, position));
  const popoverRef = React.useRef<HTMLDivElement>(null);

  React.useLayoutEffect(() => {
    if (!visible) {
      return undefined;
    }

    const updatePosition = () => {
      setStyle(getPopoverStyle(target, position));
    };

    updatePosition();
    window.addEventListener('resize', updatePosition);
    window.addEventListener('scroll', updatePosition, true);

    return () => {
      window.removeEventListener('resize', updatePosition);
      window.removeEventListener('scroll', updatePosition, true);
    };
  }, [position, target, visible]);

  React.useEffect(() => {
    if (!visible) {
      return undefined;
    }

    const handlePointerDown = (event: PointerEvent) => {
      const eventTarget = event.target as Node | null;

      if (popoverRef.current?.contains(eventTarget) || target?.contains(eventTarget)) {
        return;
      }

      onHiding();
    };

    document.addEventListener('pointerdown', handlePointerDown);

    return () => {
      document.removeEventListener('pointerdown', handlePointerDown);
    };
  }, [onHiding, target, visible]);

  if (!visible) {
    return null;
  }

  const content = (
    <div ref={popoverRef} style={style}>
      <div className={styles.popover}>
        {children}
      </div>
    </div>
  );

  return createPortal(content, document.body);
}