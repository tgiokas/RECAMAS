import { isLangCode, LANG_STORAGE_KEY } from './langConfig';
import type { LangCode } from './langTypes';

export function loadPersistedLang(): LangCode | null {
  const stored = localStorage.getItem(LANG_STORAGE_KEY);
  if (!stored) {
    return null;
  }

  return isLangCode(stored) ? stored : null;
}

export function persistLang(lang: LangCode): void {
  localStorage.setItem(LANG_STORAGE_KEY, lang);
}
