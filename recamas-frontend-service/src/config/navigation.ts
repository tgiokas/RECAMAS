import type { TranslationKeys } from '../localization/langTypes';

export type NavigationItem = {
  labelKey: TranslationKeys;
  path: string;
  icon: string;
};

export const navigationItems: readonly NavigationItem[] = [
  { labelKey: 'navigation.recamasProfiles', path: '/recamas-profiles', icon: 'group' },
  { labelKey: 'navigation.cases', path: '/cases', icon: 'folder' },
  { labelKey: 'navigation.detention', path: '/detention', icon: 'lock' },
  { labelKey: 'navigation.returnImplementation', path: '/return-implementation', icon: 'airplane' },
  { labelKey: 'navigation.reports', path: '/reports', icon: 'chart' },
  { labelKey: 'navigation.administration', path: '/admin', icon: 'preferences' },
  { labelKey: 'navigation.notifications', path: '/notifications', icon: 'bell' },
];
