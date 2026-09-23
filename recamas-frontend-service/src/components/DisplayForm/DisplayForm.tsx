import type { FormRef } from 'devextreme-react/form';
import { forwardRef, useCallback, useImperativeHandle, useMemo, useRef, useState } from 'react';
import { Form } from '../ui/Form';
import styles from './DisplayForm.module.css';
import { DisplayFormToolbar } from './DisplayFormToolbar';
import { buildSections, renderGroupHeader, renderSimpleItems } from './displayFormHelpers';
import type {
  BadgeVariant,
  DisplayBadgeConfig,
  DisplayFieldConfig,
  DisplayFormHandle,
  DisplayFormProps,
  DisplayGroupConfig,
  DisplayGroupHeaderConfig,
} from './displayFormTypes';

export type {
  BadgeVariant,
  DisplayBadgeConfig,
  DisplayFieldConfig,
  DisplayFormHandle,
  DisplayFormProps,
  DisplayGroupConfig,
  DisplayGroupHeaderConfig,
};

const DEFAULT_FORM_OPTIONS = {
  labelLocation: 'top' as const,
  showColonAfterLabel: false,
  colCount: 4,
  defaultEmptyText: '-',
};

export const DisplayForm = forwardRef<DisplayFormHandle, DisplayFormProps>(function DisplayForm(
  {
    fields,
    groups,
    defaultEmptyText = DEFAULT_FORM_OPTIONS.defaultEmptyText,
    className,
    formData,
    colCount = DEFAULT_FORM_OPTIONS.colCount,
    isEditing: controlledIsEditing,
    defaultEditing = false,
    onEditingChange,
    showEditButton = false,
    children,
    ...restProps
  },
  ref,
) {
  const formRefs = useRef<(FormRef | null)[]>([]);
  const [internalIsEditing, setInternalIsEditing] = useState(defaultEditing);

  const isEditing = controlledIsEditing ?? internalIsEditing;

  const toggleEditing = useCallback(
    (nextState: boolean) => {
      if (controlledIsEditing === undefined) {
        setInternalIsEditing(nextState);
      }
      onEditingChange?.(nextState);
    },
    [controlledIsEditing, onEditingChange],
  );

  const handleSave = useCallback(() => {
    const instances = formRefs.current
      .map((formRef) => formRef?.instance())
      .filter((instance): instance is NonNullable<typeof instance> => Boolean(instance));

    if (instances.length === 0) return;

    const isValid = instances.every((instance) => {
      const validationResult = instance.validate();
      if (
        validationResult &&
        typeof validationResult === 'object' &&
        'isValid' in validationResult
      ) {
        return validationResult.isValid !== false;
      }
      return true;
    });
    if (!isValid) return;

    console.log(
      'DisplayForm submit data:',
      instances.map((instance) => instance.option('formData')),
    );
    toggleEditing(false);
  }, [toggleEditing]);

  useImperativeHandle(ref, () => ({ save: handleSave }), [handleSave]);

  const formProps = useMemo(
    () => ({
      ...DEFAULT_FORM_OPTIONS,
      ...restProps,
      labelLocation: restProps.labelLocation ?? DEFAULT_FORM_OPTIONS.labelLocation,
      showColonAfterLabel:
        restProps.showColonAfterLabel ?? DEFAULT_FORM_OPTIONS.showColonAfterLabel,
      readOnly: !isEditing,
      formData,
      colCount,
    }),
    [colCount, formData, isEditing, restProps],
  );

  const resolvedColCount = typeof colCount === 'number' ? colCount : DEFAULT_FORM_OPTIONS.colCount;

  const sections = useMemo(() => {
    const data =
      groups && groups.length > 0
        ? groups
        : fields && fields.length > 0
          ? [{ fields, colCount: resolvedColCount }]
          : undefined;
    return buildSections(data);
  }, [fields, groups, resolvedColCount]);

  return (
    <div
      className={`${styles.displayForm} ${!isEditing ? styles['displayForm--readonly'] : ''} ${className ?? ''}`.trim()}
    >
      {showEditButton && (
        <DisplayFormToolbar
          isEditing={isEditing}
          onEdit={() => toggleEditing(true)}
          onCancel={() => toggleEditing(false)}
          onSave={handleSave}
        />
      )}

      {sections.length > 0 ? (
        sections.map((section, index) => (
          <div
            key={section.id ?? section.group?.title ?? section.title ?? `section-${index}`}
            className={`${styles.groupWrapper} ${section.cssClass ?? ''}`.trim()}
          >
            {renderGroupHeader(section)}
            <Form
              ref={(instance) => {
                formRefs.current[index] = instance;
              }}
              {...formProps}
              colCount={section.colCount ?? colCount}
              disabled={section.disabled ?? formProps.disabled}
            >
              {renderSimpleItems(section.fields, isEditing, defaultEmptyText, formData, section)}
              {index === sections.length - 1 && children}
            </Form>
          </div>
        ))
      ) : (
        <Form
          ref={(instance) => {
            formRefs.current[0] = instance;
          }}
          {...formProps}
        >
          {children}
        </Form>
      )}
    </div>
  );
});

DisplayForm.displayName = 'DisplayForm';
