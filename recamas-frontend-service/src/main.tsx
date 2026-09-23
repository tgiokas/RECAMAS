import { StrictMode } from 'react';
import { createRoot } from 'react-dom/client';
import { Provider } from 'react-redux';
import './styles/devextreme-theme/dx.material.recamas-theme.css';
import './index.css';
import App from './App.tsx';
import { store } from './app/store.ts';
import { LangProvider } from './localization/LangProvider.tsx';
import config from "devextreme/core/config";

config({
  rtlEnabled: false,
  forceIsoDateParsing: false,
  defaultCurrency: "EUR",
  editorStylingMode: "outlined",
});

createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <Provider store={store}>
      <LangProvider>
        <App />
      </LangProvider>
    </Provider>
  </StrictMode>,
);
