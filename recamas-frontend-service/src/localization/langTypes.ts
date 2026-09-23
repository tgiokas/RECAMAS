import { resources } from './localeResources';

export type LangCode = keyof typeof resources;

export interface LangState {
  lang: LangCode;
}

type Join<K, P> = K extends string | number
  ? P extends string | number
    ? `${K}.${P}`
    : never
  : never;

type NestedKeys<T> = {
  [K in keyof T & string]: T[K] extends Record<string, unknown> ? Join<K, NestedKeys<T[K]>> : K;
}[keyof T & string];

type ResourceKeys = NestedKeys<typeof resources.en.translation>;

export type TranslationKeys = [ResourceKeys] extends [never] ? string : ResourceKeys;
