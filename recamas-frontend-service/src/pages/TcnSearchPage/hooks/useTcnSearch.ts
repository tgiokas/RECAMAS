import { useRef, useState } from 'react';
import type { FormRef } from 'devextreme-react/form';
import { nationalityOptions, searchTcn } from '@/features/tcnSearch';
import type { TcnSearchCriteria, TcnSearchResult } from '@/features/tcnSearch';

export const emptyCriteria: TcnSearchCriteria = {
  arc: '',
  name: '',
  surname: '',
  nationality: undefined,
  passportNo: '',
  dateOfBirth: undefined,
  mdFileNumber: '',
};

export const dateEditorOptions = {
  displayFormat: 'dd/MM/yyyy',
  openOnFieldClick: true,
  showClearButton: true,
  type: 'date' as const,
};

export const nationalityEditorOptions = {
  dataSource: nationalityOptions,
  displayExpr: 'text',
  placeholder: 'Select nationality',
  searchEnabled: true,
  showClearButton: true,
  valueExpr: 'value',
};

function readString(value: unknown): string | undefined {
  return typeof value === 'string' ? value : undefined;
}

function readDate(value: unknown): Date | string | undefined {
  if (value instanceof Date || typeof value === 'string') {
    return value;
  }

  return undefined;
}

function readNationality(value: unknown): TcnSearchCriteria['nationality'] {
  return nationalityOptions.find((option) => option.value === value)?.value;
}

function readCriteria(form: FormRef | null): TcnSearchCriteria {
  const formData = form?.instance().option('formData');
  if (formData === undefined || formData === null || typeof formData !== 'object') {
    return { ...emptyCriteria };
  }

  const data = formData as Record<string, unknown>;

  return {
    arc: readString(data.arc),
    name: readString(data.name),
    surname: readString(data.surname),
    nationality: readNationality(data.nationality),
    passportNo: readString(data.passportNo),
    dateOfBirth: readDate(data.dateOfBirth),
    mdFileNumber: readString(data.mdFileNumber),
  };
}

export function useTcnSearch() {
  const formRef = useRef<FormRef>(null);
  const [results, setResults] = useState<TcnSearchResult[]>([]);
  const [hasSearched, setHasSearched] = useState(false);
  const [isSearching, setIsSearching] = useState(false);

  const handleSearch = () => {
    void (async () => {
      setIsSearching(true);

      try {
        const matches = await searchTcn(readCriteria(formRef.current));
        setResults(matches);
        setHasSearched(true);
      } finally {
        setIsSearching(false);
      }
    })();
  };

  const handleClear = () => {
    formRef.current?.instance().option('formData', { ...emptyCriteria });
    setResults([]);
    setHasSearched(false);
  };

  return {
    formRef,
    results,
    hasSearched,
    isSearching,
    handleSearch,
    handleClear,
    initialFormData: { ...emptyCriteria },
    dateEditorOptions,
    nationalityEditorOptions,
  };
}
