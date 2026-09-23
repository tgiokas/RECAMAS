import { Badge, type BadgeVariant } from '@/components/Badge/Badge';
import TableRowActions from '@/components/Table/TableRowActions';
import type { GridAction } from '@/components/types/tableRowActionButton';
import type { Column } from 'devextreme-react/data-grid';
import { CaseFlag, CaseStatus } from '../Details/casesTypes';
import type { RecamasCase } from '../Details/casesTypes';
import styles from '../components/CasesGrid.module.css';

const statusToBadgeVariant: Record<CaseStatus, BadgeVariant> = {
  [CaseStatus.Pending]: 'orange',
  [CaseStatus.InProgress]: 'blue',
  [CaseStatus.Completed]: 'green',
  [CaseStatus.Cancelled]: 'red',
};

const flagToBadgeVariant: Record<CaseFlag, BadgeVariant> = {
  [CaseFlag.NeedsAttention]: 'red',
  [CaseFlag.NoArc]: 'gray',
  [CaseFlag.NoTravelDocument]: 'gray',
  [CaseFlag.Minor]: 'orange',
};

function getCaseFlagValues(row: RecamasCase): CaseFlag[] {
  const flags = row.caseFlags ?? [];

  return Array.from(new Set(row.needsAttention ? [CaseFlag.NeedsAttention, ...flags] : flags));
}

export type CaseGridColumn = React.ComponentProps<typeof Column>;

const OVERFLOW_ACTIONS_WIDTH = 80;
const SINGLE_ACTION_CHROME = 92;
const SINGLE_ACTION_CHAR_WIDTH = 10;

function getActionsColumnWidth(rowActions: GridAction<RecamasCase>[]) {
  if (rowActions.length > 1) {
    return OVERFLOW_ACTIONS_WIDTH;
  }

  const [action] = rowActions;
  const iconWidth = action.icon ? 26 : 0;
  const labelWidth = action.label.length * SINGLE_ACTION_CHAR_WIDTH;

  return Math.max(OVERFLOW_ACTIONS_WIDTH, SINGLE_ACTION_CHROME + iconWidth + labelWidth);
}

export const buildCaseGridColumns = (onManageCase: (caseId: string) => void): CaseGridColumn[] => {
  const rowActions: GridAction<RecamasCase>[] = [
    {
      label: 'View',
      icon: 'eyeopen',
      onClick: (row) => onManageCase(row.caseId),
    },
  ];

  const actionsColumnWidth = getActionsColumnWidth(rowActions);

  return [
    {
      caption: 'Actions',
      width: actionsColumnWidth,
      minWidth: actionsColumnWidth,
      alignment: 'center',
      cssClass: styles.actionsCell,
      allowSorting: false,
      allowFiltering: false,
      allowHeaderFiltering: false,
      allowHiding: false,
      showInColumnChooser: false,
      cellRender: (cellInfo: { data: RecamasCase }) => (
        <TableRowActions row={cellInfo.data as RecamasCase} actions={rowActions} />
      ),
    },
    {
      dataField: 'caseId',
      caption: 'Case Reference ID',
      sortOrder: 'desc',
    },
    { dataField: 'tcnName', caption: 'TCN Name' },
    { dataField: 'arc', caption: 'ARC / Profile ID' },
    { dataField: 'type', caption: 'Case Type' },
    { dataField: 'currentStage', caption: 'Current Stage' },
    {
      dataField: 'currentStatus',
      caption: 'Current Status',
      allowFiltering: true,
      cellRender: (cellInfo: { value: CaseStatus }) => {
        const status = cellInfo.value;

        return status ? <Badge text={status} variant={statusToBadgeVariant[status]} /> : '—';
      },
    },
    {
      dataField: 'caseFlags',
      caption: 'Flags',
      allowSorting: false,
      alignment: 'left',
      width: 'auto',
      cssClass: styles.flagsColumn,
      cellRender: (cellInfo: { data: RecamasCase }) => {
        const flags = getCaseFlagValues(cellInfo.data);

        if (!flags.length) {
          return '—';
        }

        return (
          <div className={styles.flagsCell}>
            {flags.map((flag) => (
              <Badge key={flag} text={flag} variant={flagToBadgeVariant[flag]} />
            ))}
          </div>
        );
      },
    },
    {
      dataField: 'createdDate',
      caption: 'Created Date',
      cellRender: (cellInfo: { value: unknown }) =>
        cellInfo.value ? new Date(cellInfo.value as string).toISOString().slice(0, 10) : '',
    },
    {
      dataField: 'assignedOfficer',
      caption: 'Assigned Officer',
      cellRender: (cellInfo: { value: unknown }) =>
        cellInfo.value ? String(cellInfo.value) : 'Unassigned',
    },
  ];
};
