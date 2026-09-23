import { useTranslation } from 'react-i18next';
import type { TranslationKeys } from '../../localization/langTypes';

type WorkspacePageProps = {
  titleKey: TranslationKeys;
};

export function WorkspacePage({ titleKey }: WorkspacePageProps) {
  const { t } = useTranslation();

  return (
    <section className="page-content">
      <p className="page-eyebrow">RECAMAS</p>
      <h1>{t(titleKey)}</h1>
      <p>Η λειτουργική ενότητα είναι έτοιμη για σύνδεση με τις υπηρεσίες RECAMAS.</p>
    </section>
  );
}
