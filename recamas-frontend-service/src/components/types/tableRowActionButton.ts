import type { ReactNode } from 'react';

export type TableRowActionButton<T> = {
  label: string;
  icon?: string;
  onClick: (row: T) => void | Promise<void>;
  show?: boolean | ((row: T) => boolean);
  disabled?: boolean | ((row: T) => boolean);
  style?: string;
  hrAfter?: boolean | ((row: T) => boolean);
};

export type GridAction<T> = TableRowActionButton<T>;
