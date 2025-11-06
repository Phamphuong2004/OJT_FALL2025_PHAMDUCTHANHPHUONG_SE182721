import axios from "axios";

export const API_BASE = import.meta.env.VITE_API_BASE || "/api";

// Central axios instance used across the app. withCredentials=true so HttpOnly
// cookies (refreshToken) are sent automatically by the browser.
export const api = axios.create({ baseURL: API_BASE, withCredentials: true });

// Interceptor để tự động gắn token vào mọi request
api.interceptors.request.use((config) => {
  try {
    const token = localStorage.getItem("token");
    if (token) {
      config.headers = config.headers || {};
      config.headers.Authorization = `Bearer ${token}`;
      console.log(
        "[ApiClient] Added Authorization header, token length:",
        token.length
      );
    } else {
      console.log("[ApiClient] No token found in localStorage");
    }
  } catch (e) {
    console.error("[ApiClient] Error reading token:", e);
  }

  // Ensure an anonymous cart id is present
  try {
    config.headers = config.headers || {};
    let anon = localStorage.getItem("anonCartId");
    if (!anon) {
      try {
        anon = crypto.randomUUID();
      } catch (err) {
        anon = "anon-" + Math.random().toString(36).slice(2, 12);
      }
      localStorage.setItem("anonCartId", anon);
    }
    config.headers["X-Anonymous-Id"] = anon;
  } catch (e) {
    console.error("[ApiClient] Error setting anonymous ID:", e);
  }

  return config;
});

export default api;
