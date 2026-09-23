import type { StepperTypes } from 'devextreme-react/stepper';
import type { StepperNavigationMode, StepperOrientation } from './types.ts';

//Μπορούμε να εχουμε διαφορετικά items για διαφορετικά steppers.
export const caseStepItems: StepperTypes.Item[] = [
  {
    text: '1',
    label: 'Counselling',
    icon: 'user',
  },
  {
    text: '2',
    label: 'Application Processing',
    icon: 'folder',
  },
  {
    text: '3',
    label: 'Evaluation',
    icon: 'check',
  },
  {
    text: '4',
    label: 'Pre-Return',
    icon: 'airplane',
  },
  {
    text: '5',
    label: 'Return Implementation',
    icon: 'card',
  },
  {
    text: '6',
    label: 'Re-integration',
    icon: 'home',
  },
  {
    text: '7',
    label: 'Closed',
    icon: 'checkcircle',
  },
];

export const orientations: StepperOrientation[] = [
  { text: 'Horizontal', value: 'horizontal' },
  { text: 'Vertical', value: 'vertical' },
];

export const navigationModes: StepperNavigationMode[] = [
  { text: 'Non-linear', value: false },
  { text: 'Linear', value: true },
];
