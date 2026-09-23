import DataGrid, { type DataGridRef, type IDataGridOptions } from 'devextreme-react/data-grid';
import * as React from 'react';
import styles from './DxDataGrid.module.css';

export type DxDataGridProps<TRowData = object, TKey = unknown> = IDataGridOptions<TRowData, TKey>;

const DxDataGridInner = <TRowData = object, TKey = unknown>(
  props: DxDataGridProps<TRowData, TKey>,
  ref: React.ForwardedRef<DataGridRef<TRowData, TKey>>,
) => {
  return (
    <DataGrid<TRowData, TKey> ref={ref} {...props} className={props.className ?? styles.wrapper} />
  );
};

// Generic forwardRef components lose their type params, so we recast to a generic-aware signature.
const DxDataGrid = React.forwardRef(DxDataGridInner) as <TRowData = object, TKey = unknown>(
  props: DxDataGridProps<TRowData, TKey> & {
    ref?: React.ForwardedRef<DataGridRef<TRowData, TKey>>;
  },
) => ReturnType<typeof DxDataGridInner>;

export { DxDataGrid };
