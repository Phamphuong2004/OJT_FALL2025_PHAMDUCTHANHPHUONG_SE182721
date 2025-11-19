import axios from "axios";
import { getToken } from "../Auth/useAuth";

const API_BASE = (import.meta.env.VITE_API_BASE || "/api") + "/admin/users";

const authHeaders = () => {
  const token = getToken();
  return token ? { Authorization: `Bearer ${token}` } : {};
};

const AdminUserAPI = {
  list: async (params = {}) => {
    const res = await axios.get(`${API_BASE}`, {
      params: { page: params.page || 1, pageSize: params.pageSize || 20 },
      headers: authHeaders(),
    });
    return res.data;
  },

  get: async (id) => {
    const res = await axios.get(`${API_BASE}/${id}`, {
      headers: authHeaders(),
    });
    return res.data;
  },

  updateRole: async (id, role) => {
    const res = await axios.post(
      `${API_BASE}/${id}/role`,
      { role },
      { headers: { ...authHeaders(), "Content-Type": "application/json" } }
    );
    return res.data;
  },

  lock: async (id) => {
    const res = await axios.post(`${API_BASE}/${id}/lock`, null, {
      headers: authHeaders(),
    });
    return res.data;
  },

  unlock: async (id) => {
    const res = await axios.post(`${API_BASE}/${id}/unlock`, null, {
      headers: authHeaders(),
    });
    return res.data;
  },
  updateUser: async (id, payload) => {
    const res = await axios.put(`${API_BASE}/${id}`, payload, {
      headers: { ...authHeaders(), "Content-Type": "application/json" },
    });
    return res.data;
  },

  populateFullNames: async () => {
    const res = await axios.post(`${API_BASE}/populate-fullnames`, null, {
      headers: authHeaders(),
    });
    return res.data;
  },
};

export default AdminUserAPI;
