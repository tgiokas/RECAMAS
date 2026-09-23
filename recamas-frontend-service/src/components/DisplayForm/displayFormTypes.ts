import type { IFormOptions, ISimpleItemProps } from 'devextreme-react/form';
import type { FormItemComponent } from 'devextreme/ui/form';
import type { ReactNode } from 'react';

export type BadgeVariant = 'primary' | 'info' | 'success' | 'warning' | 'danger' | 'default';

export interface DisplayBadgeConfig {
  text: string;
  variant?: BadgeVariant;
  color?: string;
  backgroundColor?: string;
  /** Optional bullet dot before the text (e.g. for active status) */
  showDot?: boolean;
}

export interface DisplayGroupHeaderConfig {
  /** Group title / description */
  title?: string;
  /** Optional badge shown next to the title (e.g. ARS, CASS, RECAMAS) */
  badge?: DisplayBadgeConfig | string;
}

/**
 * Extends DevExtreme's native SimpleItem props so any dx item option (helpText,
 * isRequired, validationRules, name, ...) can be bound directly without being
 * re-declared here. `label` and `render` are overridden with DisplayForm-specific shapes.
 */
export interface DisplayFieldConfig extends Omit<
  ISimpleItemProps,
  'label' | 'render' | 'component'
> {
  /** Field label displayed above the value */
  label?: string;
  /** Explicit value override (if not reading from formData) */
  value?: string | number | boolean | null;
  /** Text to show when value is null, undefined, or empty string. Default: '-' */
  emptyText?: string;
  /** Custom formatter or renderer for the field value in read-only mode */
  render?: (value: unknown, formData?: Record<string, unknown>) => ReactNode;
  /** Whether the field is disabled in edit mode */
  disabled?: boolean;
  /** Whether the field is read-only even in edit mode */
  readOnly?: boolean;
}

export interface DisplayGroupConfig {
  /** Unique key or identifier for the group */
  id?: string;
  /** Direct group title / description */
  title?: string;
  /** Direct badge shown next to the title (e.g. ARS, CASS, RECAMAS) */
  badge?: DisplayBadgeConfig | string;
  /** Nested group configuration object holding title and badge */
  group?: DisplayGroupHeaderConfig;
  /** Number of columns in this group grid (default: form's colCount or 4) */
  colCount?: number;
  /** Number of columns this group spans in parent grid */
  colSpan?: number;
  /** Default editorType for all fields in this group (unless overridden per field) */
  editorType?: FormItemComponent;
  /** Default editorOptions for all fields in this group */

  editorOptions?: Record<string, unknown>;
  /** List of fields in this group */
  fields: DisplayFieldConfig[];
  /** Optional CSS class for the group container */
  cssClass?: string;
  /** Whether the group is visible (default: true) */
  visible?: boolean;
  /** Whether all fields in the group are disabled */
  disabled?: boolean;
  /** Whether all fields in the group are read-only */
  readOnly?: boolean;
}

export interface DisplayFormProps extends Omit<IFormOptions, 'items'> {
  /** Direct list of fields to render */
  fields?: DisplayFieldConfig[];
  /** Grouped list of fields with optional group headers and badges */
  groups?: DisplayGroupConfig[];
  /** Default fallback text for empty values across all fields (default: '-') */
  defaultEmptyText?: string;
  /** Controlled editable state */
  isEditing?: boolean;
  /** Initial editable state when uncontrolled */
  defaultEditing?: boolean;
  /** Callback fired when edit mode changes */
  onEditingChange?: (isEditing: boolean) => void;
  /** Show the built-in edit / save / cancel button toolbar (default: false) */
  showEditButton?: boolean;
  /** Custom CSS class for the root container */
  className?: string;
  /** Optional child elements (custom items/buttons/templates) */
  children?: ReactNode;
}

/** Imperative handle exposed via ref, for consumers rendering the toolbar externally */
export interface DisplayFormHandle {
  /** Validates the form and exits edit mode when valid */
  save: () => void;
}
