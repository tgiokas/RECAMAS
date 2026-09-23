import { SimpleItem } from 'devextreme-react/form';
import styles from './DisplayForm.module.css';
import type {
  DisplayBadgeConfig,
  DisplayFieldConfig,
  DisplayGroupConfig,
} from './displayFormTypes';

export interface FormItemData {
  component?: { option: (name: string) => unknown };
  dataField?: string;
  editorOptions?: { value?: unknown };
}

export function resolveBadge(badge: DisplayBadgeConfig | string): DisplayBadgeConfig {
  return typeof badge === 'string' ? { text: badge, variant: 'primary' } : badge;
}

export function buildSections(data: DisplayGroupConfig[] | undefined): DisplayGroupConfig[] {
  return (data ?? []).filter((group) => group.visible !== false);
}

function getFieldValue(
  field: DisplayFieldConfig,
  formData?: Record<string, unknown>,
  editorValue?: unknown,
) {
  if (field.dataField && formData && Object.prototype.hasOwnProperty.call(formData, field.dataField)) {
    return formData[field.dataField];
  }
  return field.value ?? editorValue;
}

export function renderGroupHeader(group: DisplayGroupConfig) {
  const badgeProp = group.group?.badge ?? group.badge;
  const title = group.group?.title ?? group.title;
  const badge = badgeProp ? resolveBadge(badgeProp) : undefined;

  if (!badge && !title) return null;

  return (
    <div className={styles.groupHeader}>
      {badge && (
        <span
          className={`${styles.badge} ${badge.variant ? (styles[`badge--${badge.variant}`] ?? '') : ''}`.trim()}
          style={{
            ...(badge.color && { color: badge.color }),
            ...(badge.backgroundColor && { backgroundColor: badge.backgroundColor }),
          }}
        >
          {badge.text}
        </span>
      )}
      {title && <span>{title}</span>}
    </div>
  );
}

function renderFieldValue(
  field: DisplayFieldConfig,
  data: FormItemData | undefined,
  defaultEmptyText: string,
  rootFormData?: Record<string, unknown>,
) {
  const formData =
    (data?.component?.option?.('formData') as Record<string, unknown> | undefined) ?? rootFormData;
  const value = getFieldValue(field, formData, data?.editorOptions?.value);

  if (field.render) {
    return <div className={styles.fieldValue}>{field.render(value, formData)}</div>;
  }

  if (value === null || value === undefined || value === '') {
    return (
      <div className={styles.fieldValue}>
        <span className={styles.emptyValue}>{field.emptyText ?? defaultEmptyText}</span>
      </div>
    );
  }

  return <div className={styles.fieldValue}>{String(value)}</div>;
}

function buildFieldLabel(field: DisplayFieldConfig) {
  const labelText = field.label ?? field.dataField;

  return {
    text: labelText,
    location: 'top' as const,
    alignment: 'left' as const,
    showColon: false,
    visible: Boolean(labelText),
  };
}

function getFieldEditorOptions(
  field: DisplayFieldConfig,
  groupDefaults?: Partial<DisplayGroupConfig>,
) {
  return {
    ...groupDefaults?.editorOptions,
    ...field.editorOptions,
    ...(field.value !== undefined && { value: field.value }),
    ...((field.disabled ?? groupDefaults?.disabled) && { disabled: true }),
    ...((field.readOnly ?? groupDefaults?.readOnly) && { readOnly: true }),
  };
}

export function renderSimpleItems(
  fields: DisplayFieldConfig[],
  isEditing: boolean,
  defaultEmptyText: string,
  formData?: Record<string, unknown>,
  groupDefaults?: Partial<DisplayGroupConfig>,
) {
  return fields
    .filter((field) => field.visible !== false)
    .map((field, index) => {
      const key = field.dataField ?? field.label ?? `field-${index}`;
      const label = buildFieldLabel(field);

      // field already carries every native dx SimpleItem prop; only label/editor/render need overriding per mode
      const itemProps = isEditing
        ? {
            ...field,
            editorType: field.editorType ?? groupDefaults?.editorType ?? 'dxTextBox',
            editorOptions: getFieldEditorOptions(field, groupDefaults),
            label,
          }
        : {
            ...field,
            label,
            render: (data?: FormItemData) =>
              renderFieldValue(field, data, defaultEmptyText, formData),
          };

      return <SimpleItem key={key} {...itemProps} />;
    });
}
