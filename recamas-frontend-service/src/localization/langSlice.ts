import { createSlice } from '@reduxjs/toolkit';
import type { PayloadAction } from '@reduxjs/toolkit';
import type { LangCode, LangState } from './langTypes';
import { DEFAULT_LANGUAGE } from './langConfig';
import { loadPersistedLang, persistLang } from './langStorage';
import type { RootState } from '@/app/store';

export const langInitialState: LangState = {
  lang: loadPersistedLang() ?? DEFAULT_LANGUAGE,
};

const langSlice = createSlice({
  name: 'langSettings',
  initialState: langInitialState,
  reducers: {
    setLang: (state, action: PayloadAction<LangCode>) => {
      state.lang = action.payload;
      persistLang(state.lang);
    },
  },
});

export const { setLang } = langSlice.actions;
export const langReducer = langSlice.reducer;
export const selectLang = (state: RootState) => state.langSettings.lang;
