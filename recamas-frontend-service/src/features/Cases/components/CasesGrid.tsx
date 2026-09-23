import { DxDataGrid } from '@/components/DxDataGrid/DxDataGrid';
import {
  Column,
  ColumnChooser,
  Export,
  FilterRow,
  HeaderFilter,
  Pager,
  Paging,
  Scrolling,
  SearchPanel,
  Sorting,
} from 'devextreme-react/data-grid';
import type { RecamasCase } from '../Details/casesTypes';
import { buildCaseGridColumns } from '../config/casesGridColumns';
import styles from './CasesGrid.module.css';

type CasesGridProps = {
  cases: RecamasCase[];
  onManageCase: (caseId: string) => void;
};

export function CasesGrid({ cases, onManageCase }: CasesGridProps) {
  const columns = buildCaseGridColumns(onManageCase);

  return (
    <div className={styles.gridWrapper}>
      <DxDataGrid<RecamasCase>
        dataSource={cases}
        noDataText="No records available."
        showBorders
        rowAlternationEnabled
        width="100%"
      >
        <SearchPanel visible placeholder="Search records..." />
        <FilterRow visible />
        <HeaderFilter visible />
        <Sorting mode="multiple" />
        <Scrolling mode="standard" />
        <ColumnChooser enabled mode="select" />
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
      </DxDataGrid>
    </div>
  );
}
