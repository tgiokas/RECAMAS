import {
  Column,
  ColumnChooser,
  Export,
  HeaderFilter,
  Pager,
  Paging,
  Scrolling,
  SearchPanel,
  Sorting,
} from 'devextreme-react/data-grid';

import type { RecamasCase } from '../Details/casesTypes';
import { buildCaseGridColumns } from './casesGridColumns';

export const getCaseGridProps = (cases: RecamasCase[]) => ({
  dataSource: cases,
  noDataText: 'No records available.',
  showBorders: true,
  rowAlternationEnabled: true,
  width: '100%',
  height: '100%',
  columnAutoWidth: true,
  allowColumnResizing: true,
});

export const renderCaseGridChildren = (onManageCase: (caseId: string) => void) => {
  const columns = buildCaseGridColumns(onManageCase);

  return (
    <>
      <SearchPanel visible placeholder="Search records..." />
      <HeaderFilter visible />
      <Sorting mode="multiple" />
      <Scrolling mode="standard" />
      <ColumnChooser
        enabled
        mode="select"
        position={{
          my: 'right top',
          at: 'right bottom',
          of: '.dx-datagrid-column-chooser-button',
        }}
      />
      <Export enabled />
      <Paging defaultPageSize={10} />
      <Pager
        visible
        allowedPageSizes={[10, 25, 50]}
        showPageSizeSelector
        showInfo
        showNavigationButtons
      />
      {columns.map((column) => (
        <Column
          key={String(column.dataField ?? column.caption ?? 'column')}
          {...column}
          cellRender={column.cellRender}
        />
      ))}
    </>
  );
};
