import axios from "axios";

const API_URL =
  import.meta.env.VITE_API_URL ||
  import.meta.env.VITE_API_BASE + "/api" ||
  "http://localhost:5179/api";

const getAuthHeader = () => {
  const token = localStorage.getItem("token");
  return token ? { Authorization: `Bearer ${token}` } : {};
};

const wishlistAPI = {
  // Lấy wishlist của user
  async getWishlist() {
    const response = await axios.get(`${API_URL}/wishlist`, {
      headers: getAuthHeader(),
    });
    return response.data;
  },

  // Thêm game vào wishlist
  async addToWishlist(gameId) {
    const response = await axios.post(
      `${API_URL}/wishlist/${gameId}`,
      {},
      {
        headers: getAuthHeader(),
      }
    );
    return response.data;
  },

  // Xóa game khỏi wishlist
  async removeFromWishlist(gameId) {
    const response = await axios.delete(`${API_URL}/wishlist/${gameId}`, {
      headers: getAuthHeader(),
    });
    return response.data;
  },

  // Kiểm tra game có trong wishlist không
  async checkInWishlist(gameId) {
    const response = await axios.get(`${API_URL}/wishlist/check/${gameId}`, {
      headers: getAuthHeader(),
    });
    return response.data;
  },

  // Lấy số lượng items trong wishlist
  async getWishlistCount() {
    const response = await axios.get(`${API_URL}/wishlist/count`, {
      headers: getAuthHeader(),
    });
    return response.data;
  },
};

export default wishlistAPI;
