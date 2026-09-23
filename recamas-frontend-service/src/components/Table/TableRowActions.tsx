import { useCallback, useState } from 'react';
import Button from '../Button/Button';
import { Popover } from '../Popover/Popover';
import type { GridAction } from '../types/tableRowActionButton';
import styles from './Table.module.css';

type TableRowActionsProps<T extends object> = {
  row: T;
  actions: GridAction<T>[];
};

function invokeAction<T>(action: GridAction<T>, row: T) {
  try {
    const maybePromise = action.onClick(row) as unknown;
    if (maybePromise instanceof Promise) {
      void (maybePromise as Promise<unknown>).catch(() => undefined);
    }
  } catch {
    // no-op
  }
}

const TableRowActions = <T extends object>({ row, actions }: TableRowActionsProps<T>) => {
  const [visible, setVisible] = useState(false);
  const [buttonElement, setButtonElement] = useState<HTMLElement | null>(null);

  const visibleActions = actions.filter((action) =>
    typeof action.show === 'function' ? action.show(row) : action.show !== false
  );

  const handleButtonRef = useCallback((element: HTMLButtonElement | null) => {
    setButtonElement(element);
  }, []);

  const handleTogglePopover = useCallback(() => setVisible((prev) => !prev), []);
  const handleHidePopover = useCallback(() => setVisible(false), []);

  if (visibleActions.length === 0) {
    return null;
  }

  if (visibleActions.length === 1) {
    const action = visibleActions[0];
    const isDisabled = typeof action.disabled === 'function' ? action.disabled(row) : (action.disabled ?? false);

    return (
      <div className={styles.singleAction}>
        <Button
          icon={action.icon}
          text={action.label}
          stylingMode="outlined"
          aria-label={action.label}
          disabled={isDisabled}
          onClick={() => invokeAction(action, row)}
        />
      </div>
    );
  }

  return (
    <div className={styles.actionHost}>
      <Button
        ref={handleButtonRef}
        icon="overflow"
        stylingMode="text"
        aria-label="Open row actions"
        onClick={handleTogglePopover}
      />
      <Popover visible={visible} onHiding={handleHidePopover} target={buttonElement ?? undefined} position="bottom">
        <div>
          {visibleActions.map((action) => {
            const isDisabled =
              typeof action.disabled === 'function' ? action.disabled(row) : (action.disabled ?? false);

            return (
              <>
                <button key={action.label}
                  className={`${styles.action} ${action.style ?? ''}`}
                  disabled={isDisabled}
                  onClick={() => {
                    invokeAction(action, row);
                    setVisible(false);
                  }}
                >
                  {action?.icon ? <i className={`dx-icon dx-icon-${action.icon} ${styles.actionIcon}`} aria-hidden="true" /> : null}
                  <span>{action.label}</span>
                </button>
                {(typeof action.hrAfter === 'function' ? action.hrAfter(row) : action.hrAfter) ? (
                  <hr className="my-1 dark:border-gray-700" />
                ) : null}
              </>
            );
          })}
        </div>
      </Popover>
    </div>
  );
};

export default TableRowActions;