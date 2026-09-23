import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react';
import path from 'node:path';

const srcDir = path.resolve(import.meta.dirname, './src');

export default defineConfig({
  plugins: [react()],
  resolve: {
    alias: {
      '@': srcDir,
      '@app': path.resolve(srcDir, './app'),
      '@api': path.resolve(srcDir, './api'),
      '@components': path.resolve(srcDir, './components'),
      '@features': path.resolve(srcDir, './features'),
      '@layout': path.resolve(srcDir, './layouts'),
      '@models': path.resolve(srcDir, './models'),
      '@pages': path.resolve(srcDir, './pages'),
      '@utils': path.resolve(srcDir, './utils'),
      '@config': path.resolve(srcDir, './config'),
    },
  },
  server: {
    port: 5173,
    strictPort: true,
  },
});
