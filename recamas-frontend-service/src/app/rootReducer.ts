import { langReducer } from '@/localization/langSlice';
import { combineReducers } from '@reduxjs/toolkit';

export const rootReducer = combineReducers({
  langSettings: langReducer,
});
