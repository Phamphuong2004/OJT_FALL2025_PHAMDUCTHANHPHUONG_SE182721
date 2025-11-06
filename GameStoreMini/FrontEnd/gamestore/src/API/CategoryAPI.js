import axios from "axios";

const API_BASE = import.meta.env.VITE_API_BASE || "/api";
const api = axios.create({ baseURL: API_BASE });

function makeAnonId() {
  try {
    return ([1e7] + -1e3 + -4e3 + -8e3 + -1e11).replace(/[018]/g, (c) =>
      (
        c ^
        (crypto.getRandomValues(new Uint8Array(1))[0] & (15 >> (c / 4)))
      ).toString(16)
    );
  } catch {
    return "anon-" + Math.random().toString(36).slice(2, 12);
  }
}

api.interceptors.request.use((config) => {
  config.headers = config.headers || {};
  const token = localStorage.getItem("token");
  if (token) config.headers.Authorization = `Bearer ${token}`;

  let anon = localStorage.getItem("anonCartId");
  if (!anon) {
    anon = makeAnonId();
    localStorage.setItem("anonCartId", anon);
  }
  config.headers["X-Anonymous-Id"] = anon;

  return config;
});

// Public
export async function getAll(params) {
  const res = await api.get("/categories", { params });
  return res.data;
}

export async function getOne(id) {
  const res = await api.get(`/categories/${id}`);
  return res.data;
}

// Admin
export async function addCategory(payload) {
  const res = await api.post("/categories", payload);
  return res.data;
}

export async function updateCategory(id, payload) {
  const res = await api.put(`/categories/${id}`, payload);
  return res.data;
}

export async function deleteCategory(id) {
  const res = await api.delete(`/categories/${id}`);
  return res.data;
}

export default { getAll, getOne, addCategory, updateCategory, deleteCategory };
