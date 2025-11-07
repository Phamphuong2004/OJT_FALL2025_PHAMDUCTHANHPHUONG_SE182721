import axios from "axios";

const API_URL =
  import.meta.env.VITE_API_URL ||
  import.meta.env.VITE_API_BASE + "/api" ||
  "http://localhost:5179/api";

const getAuthHeader = () => {
  const token = localStorage.getItem("token");
  return token ? { Authorization: `Bearer ${token}` } : {};
};

const adminReviewAPI = {
  // Lấy tất cả reviews (Admin)
  async getAllReviews(filters = {}) {
    const params = new URLSearchParams();

    if (filters.gameId) params.append("gameId", filters.gameId);
    if (filters.userId) params.append("userId", filters.userId);
    if (filters.search) params.append("search", filters.search);
    if (filters.minRating) params.append("minRating", filters.minRating);
    if (filters.maxRating) params.append("maxRating", filters.maxRating);
    if (filters.verifiedPurchaseOnly)
      params.append("verifiedPurchaseOnly", "true");
    if (filters.sortBy) params.append("sortBy", filters.sortBy);
    if (filters.sortOrder) params.append("sortOrder", filters.sortOrder);
    if (filters.page) params.append("page", filters.page);
    if (filters.pageSize) params.append("pageSize", filters.pageSize);

    const response = await axios.get(
      `${API_URL}/admin/adminreviews?${params}`,
      {
        headers: getAuthHeader(),
      }
    );
    return response.data;
  },

  // Lấy thống kê
  async getStatistics() {
    const response = await axios.get(
      `${API_URL}/admin/adminreviews/statistics`,
      {
        headers: getAuthHeader(),
      }
    );
    return response.data;
  },

  // Xóa review (Admin)
  async deleteReview(reviewId, reason) {
    const params = reason ? `?reason=${encodeURIComponent(reason)}` : "";
    const response = await axios.delete(
      `${API_URL}/admin/adminreviews/${reviewId}${params}`,
      {
        headers: getAuthHeader(),
      }
    );
    return response.data;
  },

  // Ẩn/hiện review
  async toggleHideReview(reviewId, isHidden, reason) {
    const response = await axios.put(
      `${API_URL}/admin/adminreviews/${reviewId}/hide`,
      { isHidden, reason },
      {
        headers: getAuthHeader(),
      }
    );
    return response.data;
  },
};

export default adminReviewAPI;
