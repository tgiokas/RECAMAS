import i18n from 'i18next';
import { initReactI18next } from 'react-i18next';
import { DEFAULT_LANGUAGE } from './langConfig';
import { loadPersistedLang } from './langStorage';
import { resources } from './localeResources';
void i18n.use(initReactI18next).init({
  resources,
  lng: loadPersistedLang() ?? DEFAULT_LANGUAGE,
  fallbackLng: DEFAULT_LANGUAGE,
  interpolation: { escapeValue: false },
});

export default i18n;
