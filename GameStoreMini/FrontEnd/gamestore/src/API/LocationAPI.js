import axios from "axios";

const API_BASE = import.meta.env.VITE_API_BASE || "/api";
const api = axios.create({ baseURL: API_BASE });

api.interceptors.request.use((config) => {
  config.headers = config.headers || {};
  const token = localStorage.getItem("token");
  if (token) config.headers.Authorization = `Bearer ${token}`;

  let anon = localStorage.getItem("anonCartId");
  if (!anon) {
    try {
      anon = crypto.randomUUID();
    } catch {
      anon = "anon-" + Math.random().toString(36).slice(2, 12);
    }
    localStorage.setItem("anonCartId", anon);
  }
  config.headers["X-Anonymous-Id"] = anon;
  return config;
});

// Public endpoints - không cần authentication
export async function getAllLocations() {
  const res = await api.get("/locations");
  return res.data;
}

export async function getCities(country = "Việt Nam") {
  const res = await api.get("/locations/cities", {
    params: { country },
  });
  return res.data;
}

export async function getDistricts(city) {
  if (!city) throw new Error("City parameter is required");

  const res = await api.get("/locations/districts", {
    params: { city },
  });
  return res.data;
}

export default {
  getAllLocations,
  getCities,
  getDistricts,
};
