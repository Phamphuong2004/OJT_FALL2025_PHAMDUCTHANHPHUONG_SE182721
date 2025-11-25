import axios from "axios";
import { getToken } from "../Auth/useAuth";

const API_BASE = import.meta.env.VITE_API_BASE || "https://localhost:7201";
const BASE = API_BASE + "/api/promotions";
// Some server endpoints are exposed under singular /api/promotion (claim endpoints),
// keep a singular base handy.
const BASE_SINGULAR = API_BASE + "/api/promotion";

const authHeaders = () => {
  const token = getToken();
  return { Authorization: token ? `Bearer ${token}` : "" };
};

const PromotionAPI = {
  getActivePromotions: async (params = {}) => {
    const res = await axios.get(`${BASE}`, { params });
    return res.data;
  },

  getPromotionBySlug: async (slug) => {
    const res = await axios.get(`${BASE}/by-slug/${encodeURIComponent(slug)}`);
    return res.data;
  },

  getPromotionById: async (id) => {
    const res = await axios.get(`${BASE}/${id}`);
    return res.data;
  },

  getFeatured: async (limit = 6) => {
    const res = await axios.get(`${BASE}/featured`, { params: { limit } });
    return res.data;
  },

  getEventTypes: async () => {
    const res = await axios.get(`${BASE}/event-types`);
    return res.data;
  },

  // Customer actions
  claimPromotion: async (promotionId) => {
    const res = await axios.post(
      `${BASE_SINGULAR}/${promotionId}/claim`,
      null,
      { headers: authHeaders() }
    );
    return res.data;
  },

  getMyClaims: async () => {
    const res = await axios.get(`${BASE_SINGULAR}/my-claims`, {
      headers: authHeaders(),
    });
    return res.data;
  },
};

export default PromotionAPI;
