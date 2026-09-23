 import type { LangCode } from './langTypes';
 import { resources } from './localeResources';

export const LANG_STORAGE_KEY = 'recamas-locale';

export const SUPPORTED_LANGUAGES = Object.keys(resources) as LangCode[];

export function isLangCode(value: unknown): value is LangCode {
  return typeof value === 'string' && SUPPORTED_LANGUAGES.includes(value as LangCode);
}

const configuredLanguage: unknown = import.meta.env.VITE_DEFAULT_LANGUAGE;

export const DEFAULT_LANGUAGE: LangCode = isLangCode(configuredLanguage)
  ? configuredLanguage
  : 'en';

export const LANGUAGE_LABELS: Record<LangCode, string> = {
  en: 'English',
};
