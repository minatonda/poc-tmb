import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

export default defineConfig({
  plugins: [react()],
  server: {
    host: true,
    port: 4200,
    proxy: {
      '/api': {
        target: 'http://orderapi:5000', // nome do serviço do backend + porta interna
        changeOrigin: true,
      },
    },
  },
})
