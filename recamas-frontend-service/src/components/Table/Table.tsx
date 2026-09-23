import * as React from 'react';
import DataGrid, {
  Column,
  ColumnChooser,
  HeaderFilter,
  Item,
  Scrolling,
  SearchPanel,
  Toolbar,
} from 'devextreme-react/data-grid';
import TableRowActions from './TableRowActions';
import styles from './Table.module.css';
import type { GridAction } from '@/components/types/tableRowActionButton';

export type TableColumnProps<T = object> = {
  caption?: string;
  dataField?: keyof T | string;
  render?: (value: unknown, row: T) => React.ReactNode;
  width?: number | string;
  className?: string;
  children?: React.ReactNode;
};

export type IColumnProps<T = object> = TableColumnProps<T>;

type TableProps<T extends object> = {
  dataSource?: T[];
  columns?: IColumnProps<T>[];
  rowActions?: GridAction<T>[];
  toolbarLeft?: React.ReactNode;
  children?: React.ReactNode;
};

const TableColumn = <T,>(props: TableColumnProps<T> & { children?: React.ReactNode }) => {
  return <>{props.children}</>;
};

const Table = <T extends object>({
  dataSource = [] as T[],
  columns = [],
  rowActions,
  toolbarLeft,
  children,
}: TableProps<T>) => {
  const childColumns = React.Children.toArray(children)
    .filter(React.isValidElement)
    .map((child) => (child.props ?? {}) as TableColumnProps<T>)
    .filter((column) => typeof column.caption === 'string' || typeof column.dataField === 'string');

  const resolvedColumns: IColumnProps<T>[] =
    columns.length > 0
      ? columns
      : childColumns.length > 0
        ? childColumns
      : Object.keys(dataSource[0] ?? {}).map((key) => ({
          caption: key,
          dataField: key,
          width: undefined,
          className: '',
        }));

  return (
    <div className={styles.wrapper}>
      <DataGrid
        dataSource={dataSource}
        noDataText="No records available."
        showBorders
        rowAlternationEnabled
        width="100%"
      >
        <SearchPanel visible placeholder="Search records..." />
        <ColumnChooser enabled mode="select" />
        <HeaderFilter visible />
        <Scrolling mode="standard" />
        <Toolbar>
          {toolbarLeft ? (
            <Item location="before" locateInMenu="never">
              <div className={styles.toolbarLeft}>{toolbarLeft}</div>
            </Item>
          ) : null}
          <Item name="columnChooserButton" locateInMenu="never" />
          <Item name="searchPanel" locateInMenu="never" />
        </Toolbar>
        {rowActions && rowActions.length > 0 ? (
          <Column
            caption="Actions"
            alignment="center"
            width="auto"
            cssClass={styles.actionsCell}
            allowSorting={false}
            allowFiltering={false}
            allowHeaderFiltering={false}
            allowHiding={false}
            showInColumnChooser={false}
            cellRender={(cellInfo) => <TableRowActions row={cellInfo.data as T} actions={rowActions} />}
          />
        ) : null}
        {resolvedColumns.map((column, index) => {
          const dataField = column.dataField ? String(column.dataField) : undefined;

          return (
            <Column
              key={String(column.dataField ?? column.caption ?? index)}
              dataField={dataField}
              caption={column.caption ?? dataField}
              width={column.width}
              cssClass={column.className}
              cellRender={
                column.render
                  ? (cellInfo) => column.render?.(cellInfo.value, cellInfo.data as T)
                  : undefined
              }
            />
          );
        })}
      </DataGrid>
    </div>
  );
};

export { Table, TableColumn };
