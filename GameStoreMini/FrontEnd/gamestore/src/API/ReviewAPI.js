import axios from "axios";

const API_URL =
  import.meta.env.VITE_API_URL ||
  import.meta.env.VITE_API_BASE ||
  "http://localhost:5179/api";

const reviewService = {
  // Lấy danh sách reviews
  async getReviews(filters = {}) {
    try {
      const params = new URLSearchParams();

      if (filters.gameId) params.append("gameId", filters.gameId);
      if (filters.userId) params.append("userId", filters.userId);
      if (filters.minRating) params.append("minRating", filters.minRating);
      if (filters.maxRating) params.append("maxRating", filters.maxRating);
      if (filters.verifiedPurchaseOnly)
        params.append("verifiedPurchaseOnly", "true");
      if (filters.sortBy) params.append("sortBy", filters.sortBy);
      if (filters.sortOrder) params.append("sortOrder", filters.sortOrder);
      if (filters.page) params.append("page", filters.page);
      if (filters.pageSize) params.append("pageSize", filters.pageSize);

      console.log("📡 Calling API:", `${API_URL}/reviews?${params}`);
      const response = await axios.get(`${API_URL}/reviews?${params}`);
      console.log("✅ API Response:", response.data);
      return response.data;
    } catch (error) {
      console.error("❌ API Error:", error);
      throw error;
    }
  },

  // Lấy chi tiết 1 review
  async getReviewById(id) {
    try {
      const response = await axios.get(`${API_URL}/reviews/${id}`);
      return response.data;
    } catch (error) {
      console.error("Error getting review:", error);
      throw error;
    }
  },

  // Tạo review mới (cần token)
  async createReview(reviewData, token) {
    try {
      const response = await axios.post(`${API_URL}/reviews`, reviewData, {
        headers: { Authorization: `Bearer ${token}` },
      });
      return response.data;
    } catch (error) {
      console.error("Error creating review:", error);
      throw error;
    }
  },

  // Cập nhật review (cần token)
  async updateReview(id, reviewData, token) {
    try {
      const response = await axios.put(`${API_URL}/reviews/${id}`, reviewData, {
        headers: { Authorization: `Bearer ${token}` },
      });
      return response.data;
    } catch (error) {
      console.error("Error updating review:", error);
      throw error;
    }
  },

  // Xóa review (cần token)
  async deleteReview(id, token) {
    try {
      const response = await axios.delete(`${API_URL}/reviews/${id}`, {
        headers: { Authorization: `Bearer ${token}` },
      });
      return response.data;
    } catch (error) {
      console.error("Error deleting review:", error);
      throw error;
    }
  },

  // Toggle helpful (cần token)
  async toggleHelpful(id, token) {
    try {
      const response = await axios.post(
        `${API_URL}/reviews/${id}/helpful`,
        {},
        {
          headers: { Authorization: `Bearer ${token}` },
        }
      );
      return response.data;
    } catch (error) {
      console.error("Error toggling helpful:", error);
      throw error;
    }
  },

  // Lấy thống kê rating của game
  async getGameRatingStatistics(gameId) {
    try {
      const response = await axios.get(
        `${API_URL}/reviews/game/${gameId}/statistics`
      );
      return response.data;
    } catch (error) {
      console.error("Error getting statistics:", error);
      throw error;
    }
  },

  // Kiểm tra user có thể review không (cần token)
  async canUserReviewGame(gameId, token) {
    try {
      const response = await axios.get(
        `${API_URL}/reviews/user/can-review/${gameId}`,
        {
          headers: { Authorization: `Bearer ${token}` },
        }
      );
      return response.data;
    } catch (error) {
      console.error("Error checking review eligibility:", error);
      throw error;
    }
  },
};

export default reviewService;
