import { useDispatch, useSelector } from 'react-redux';
import type { AppDispatch } from '@/app/store';
import { selectLang, setLang } from './langSlice';
import type { LangCode } from './langTypes';

export function useLang() {
  const dispatch = useDispatch<AppDispatch>();
  const lang = useSelector(selectLang);

  const changeLang = (next: LangCode) => dispatch(setLang(next));

  return { lang, setLang: changeLang };
}
