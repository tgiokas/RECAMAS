import type { ReactNode } from 'react';
import { I18nextProvider } from 'react-i18next';
import i18n from './i18n';
import { useLangSync } from './useLangSync';

export interface LangProviderProps {
  children: ReactNode;
}

function LangSync({ children }: LangProviderProps) {
  useLangSync();
  return <>{children}</>;
}

export function LangProvider({ children }: LangProviderProps) {
  return (
    <I18nextProvider i18n={i18n}>
      <LangSync>{children}</LangSync>
    </I18nextProvider>
  );
}
