import axios from "axios";
import { getToken } from "../Auth/useAuth";

const API_BASE_URL =
  (process.env.REACT_APP_API_BASE || "https://localhost:7201") +
  "/api/admin/promotions";

const authHeaders = () => {
  const token = getToken();
  return {
    Authorization: token ? `Bearer ${token}` : "",
  };
};

const AdminPromotionAPI = {
  // Admin: list with params
  getAllPromotions: async (params = {}) => {
    const res = await axios.get(`${API_BASE_URL}`, {
      params: {
        page: params.page || 1,
        pageSize: params.limit || params.pageSize || 20,
        search: params.search,
        isActive: params.isActive,
      },
      headers: authHeaders(),
    });
    return res.data;
  },

  getPromotionById: async (id) => {
    const res = await axios.get(`${API_BASE_URL}/${id}`, {
      headers: authHeaders(),
    });
    return res.data;
  },

  // create: supports multipart/form-data when sending FormData
  createPromotion: async (formData) => {
    const headers = { ...authHeaders(), "Content-Type": "multipart/form-data" };
    const res = await axios.post(`${API_BASE_URL}`, formData, { headers });
    return res.data;
  },

  updatePromotion: async (id, formData) => {
    const headers = { ...authHeaders(), "Content-Type": "multipart/form-data" };
    const res = await axios.put(`${API_BASE_URL}/${id}`, formData, { headers });
    return res.data;
  },

  deletePromotion: async (id) => {
    const res = await axios.delete(`${API_BASE_URL}/${id}`, {
      headers: authHeaders(),
    });
    return res.data;
  },

  togglePromotionStatus: async (id) => {
    const res = await axios.patch(`${API_BASE_URL}/${id}/toggle-status`, null, {
      headers: authHeaders(),
    });
    return res.data;
  },

  togglePromotionFeatured: async (id) => {
    const res = await axios.patch(
      `${API_BASE_URL}/${id}/toggle-featured`,
      null,
      { headers: authHeaders() }
    );
    return res.data;
  },

  getStatistics: async () => {
    const res = await axios.get(`${API_BASE_URL}/stats`, {
      headers: authHeaders(),
    });
    return res.data;
  },

  // helper: get available games for selection
  getAvailableGames: async (q = "") => {
    const res = await axios.get(`${API_BASE_URL}/available-games`, {
      params: { q },
      headers: authHeaders(),
    });
    return res.data;
  },
};

export default AdminPromotionAPI;
