import { Nationality } from '../tcnSearchTypes';

export const nationalityOptions = Object.values(Nationality).map((value) => ({
  value,
  text: value,
}));
