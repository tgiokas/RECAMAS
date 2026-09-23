import { forwardRef } from 'react';
import DxForm, { IFormOptions, type FormRef } from 'devextreme-react/form';

export type FormProps = IFormOptions;

// Company-wide defaults; override per-instance via props.
const DEFAULTS: Partial<IFormOptions> = {
  labelLocation: 'top',
  showColonAfterLabel: true,
  colCount: 3,
};

export const Form = forwardRef<FormRef, FormProps>((props, ref) => (
  <DxForm ref={ref} {...DEFAULTS} {...props} />
));

Form.displayName = 'Form';
