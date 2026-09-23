import { useEffect } from 'react';
import { useTranslation } from 'react-i18next';
import { loadMessages, locale } from 'devextreme/localization';
import en from './resources/en.json';
import { useLang } from './useLang';

loadMessages({ en: en.devextremeMessages });

export function useLangSync(): void {
  const { lang } = useLang();
  const { i18n } = useTranslation();

  useEffect(() => {
    if (i18n.language !== lang) {
      void i18n.changeLanguage(lang);
    }
    locale(lang);
  }, [lang, i18n]);
}
