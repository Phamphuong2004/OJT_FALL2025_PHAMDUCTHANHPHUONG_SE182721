import axios from "axios";

const API_URL =
  import.meta.env.VITE_API_URL ||
  import.meta.env.VITE_API_BASE + "/api" ||
  "http://localhost:5179/api";

const getAuthHeader = () => {
  const token = localStorage.getItem("token");
  return token ? { Authorization: `Bearer ${token}` } : {};
};

const viewHistoryAPI = {
  // Lấy lịch sử xem
  async getViewHistory(page = 1, pageSize = 20) {
    const response = await axios.get(
      `${API_URL}/viewhistory?page=${page}&pageSize=${pageSize}`,
      {
        headers: getAuthHeader(),
      }
    );
    return response.data;
  },

  // Thêm vào lịch sử xem
  async addViewHistory(gameId) {
    const response = await axios.post(
      `${API_URL}/viewhistory`,
      { gameId },
      {
        headers: getAuthHeader(),
      }
    );
    return response.data;
  },

  // Xóa khỏi lịch sử
  async removeFromHistory(gameId) {
    const response = await axios.delete(`${API_URL}/viewhistory/${gameId}`, {
      headers: getAuthHeader(),
    });
    return response.data;
  },

  // Xóa toàn bộ lịch sử
  async clearHistory() {
    const response = await axios.delete(`${API_URL}/viewhistory/clear`, {
      headers: getAuthHeader(),
    });
    return response.data;
  },

  // Lấy số lượng
  async getHistoryCount() {
    const response = await axios.get(`${API_URL}/viewhistory/count`, {
      headers: getAuthHeader(),
    });
    return response.data;
  },
};

export default viewHistoryAPI;
