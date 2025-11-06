import { defineConfig } from "vite";
import react from "@vitejs/plugin-react";

// Vite config with dev proxy to backend. Backend listens on https://localhost:7154
// and also http://localhost:5179 (from ASP.NET launchSettings). We forward
// requests starting with /api and /uploads to the HTTPS endpoint and set
// secure:false so self-signed dev certs don't block the proxy.
export default defineConfig({
  plugins: [react()],
  server: {
    port: 5173,
    strictPort: false,
    proxy: {
      // API proxy (all backend endpoints)
      "/api": {
        // Use the backend's HTTP URL so the dev proxy does not cause the
        // backend to think the original request was HTTPS. This prevents
        // the server from setting Secure cookies that are then rejected
        // by the browser when the frontend runs over HTTP via Vite.
        target: "http://localhost:5179",
        changeOrigin: true,
        secure: false,
        headers: {
          // Ensure backend sees the original protocol as HTTP when proxied from Vite.
          // This helps the server decide not to set Secure cookies during local dev.
          "X-Forwarded-Proto": "http",
        },
        // preserve path as-is; we don't need to rewrite
        rewrite: (path) => path,
        // enable websockets in case backend uses SignalR or similar
        ws: true,
      },

      // SignalR hub websocket proxy
      "/hubs": {
        target: "http://localhost:5179",
        changeOrigin: true,
        secure: false,
        headers: { "X-Forwarded-Proto": "http" },
        ws: true,
        rewrite: (path) => path,
      },

      // Static uploads served from backend wwwroot/uploads
      "/uploads": {
        target: "http://localhost:5179",
        changeOrigin: true,
        secure: false,
        headers: { "X-Forwarded-Proto": "http" },
        rewrite: (path) => path,
      },
    },
    // Allow the dev server to accept requests from the frontend origin
    // (useful when visiting via different hostnames). Not strictly required
    // for the proxy, but helpful during development.
    host: true,
  },
});
