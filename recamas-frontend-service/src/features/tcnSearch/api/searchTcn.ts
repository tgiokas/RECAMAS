import { tcnSearchMock } from '../config/tcnSearchMock';
import type { TcnSearchCriteria, TcnSearchResult } from '../tcnSearchTypes';

const SEARCH_DELAY_MS = 250;

function toDateOnly(value: Date | string | undefined): string | undefined {
  if (value instanceof Date) {
    if (Number.isNaN(value.getTime())) {
      return undefined;
    }

    const year = String(value.getFullYear());
    const month = String(value.getMonth() + 1).padStart(2, '0');
    const day = String(value.getDate()).padStart(2, '0');
    return `${year}-${month}-${day}`;
  }

  if (typeof value === 'string' && value.trim() !== '') {
    return value.slice(0, 10);
  }

  return undefined;
}

function matchesText(actual: string, query: string | undefined): boolean {
  if (query === undefined || query.trim() === '') {
    return true;
  }

  return actual.toLowerCase().includes(query.trim().toLowerCase());
}

function matchesExact(actual: string, query: string | undefined): boolean {
  if (query === undefined || query.trim() === '') {
    return true;
  }

  return actual === query;
}

function matchesRecord(record: TcnSearchResult, criteria: TcnSearchCriteria): boolean {
  const dateOfBirth = toDateOnly(criteria.dateOfBirth);

  return (
    matchesText(record.arc, criteria.arc) &&
    matchesText(record.firstName, criteria.name) &&
    matchesText(record.lastName, criteria.surname) &&
    matchesExact(record.nationality, criteria.nationality) &&
    matchesText(record.passportNo, criteria.passportNo) &&
    (dateOfBirth === undefined || record.dateOfBirth === dateOfBirth) &&
    matchesText(record.mdFileNo, criteria.mdFileNumber)
  );
}

export function searchTcn(criteria: TcnSearchCriteria): Promise<TcnSearchResult[]> {
  const results = tcnSearchMock.filter((record) => matchesRecord(record, criteria));

  return new Promise((resolve) => {
    window.setTimeout(() => {
      resolve(results);
    }, SEARCH_DELAY_MS);
  });
}
