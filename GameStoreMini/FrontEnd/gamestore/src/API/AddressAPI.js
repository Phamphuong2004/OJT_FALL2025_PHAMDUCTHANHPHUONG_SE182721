import api, { getErrorMessage } from "./ApiClient";

/**
 * Address API - Quản lý địa chỉ giao hàng
 */
const AddressAPI = {
  /**
   * Lấy tất cả địa chỉ của user hiện tại
   * @returns {Promise<Array>} Danh sách địa chỉ
   */
  getAddresses: async () => {
    try {
      const response = await api.get("/Addresses");
      return response.data.data || [];
    } catch (error) {
      console.error("[AddressAPI] Error getting addresses:", error);
      throw new Error(getErrorMessage(error));
    }
  },

  /**
   * Lấy địa chỉ mặc định
   * @returns {Promise<Object|null>} Địa chỉ mặc định hoặc null
   */
  getDefaultAddress: async () => {
    try {
      const response = await api.get("/Addresses/default");
      return response.data.data || null;
    } catch (error) {
      if (error.response?.status === 404) {
        return null; // Chưa có địa chỉ mặc định
      }
      console.error("[AddressAPI] Error getting default address:", error);
      throw new Error(getErrorMessage(error));
    }
  },

  /**
   * Lấy chi tiết 1 địa chỉ theo ID
   * @param {number} id - ID của địa chỉ
   * @returns {Promise<Object>} Thông tin địa chỉ
   */
  getAddressById: async (id) => {
    try {
      const response = await api.get(`/Addresses/${id}`);
      return response.data.data;
    } catch (error) {
      console.error("[AddressAPI] Error getting address by id:", error);
      throw new Error(getErrorMessage(error));
    }
  },

  /**
   * Tạo địa chỉ mới
   * @param {Object} addressData - Dữ liệu địa chỉ
   * @param {string} addressData.fullName - Họ tên người nhận
   * @param {string} addressData.phoneNumber - Số điện thoại
   * @param {string} addressData.street - Số nhà, tên đường
   * @param {string} addressData.ward - Phường/Xã
   * @param {string} addressData.district - Quận/Huyện
   * @param {string} addressData.city - Tỉnh/Thành phố
   * @param {string} addressData.postalCode - Mã bưu điện (optional)
   * @param {boolean} addressData.isDefault - Đặt làm địa chỉ mặc định
   * @returns {Promise<Object>} Địa chỉ vừa tạo
   */
  createAddress: async (addressData) => {
    try {
      const response = await api.post("/Addresses", addressData);
      return response.data.data;
    } catch (error) {
      console.error("[AddressAPI] Error creating address:", error);
      throw new Error(getErrorMessage(error));
    }
  },

  /**
   * Cập nhật địa chỉ
   * @param {number} id - ID của địa chỉ
   * @param {Object} addressData - Dữ liệu cập nhật
   * @returns {Promise<Object>} Địa chỉ sau khi cập nhật
   */
  updateAddress: async (id, addressData) => {
    try {
      const response = await api.put(`/Addresses/${id}`, addressData);
      return response.data.data;
    } catch (error) {
      console.error("[AddressAPI] Error updating address:", error);
      throw new Error(getErrorMessage(error));
    }
  },

  /**
   * Đặt địa chỉ làm mặc định
   * @param {number} id - ID của địa chỉ
   * @returns {Promise<Object>} Response thành công
   */
  setDefaultAddress: async (id) => {
    try {
      const response = await api.put(`/Addresses/${id}/set-default`);
      return response.data;
    } catch (error) {
      console.error("[AddressAPI] Error setting default address:", error);
      throw new Error(getErrorMessage(error));
    }
  },

  /**
   * Xóa địa chỉ
   * @param {number} id - ID của địa chỉ
   * @returns {Promise<Object>} Response thành công
   */
  deleteAddress: async (id) => {
    try {
      const response = await api.delete(`/Addresses/${id}`);
      return response.data;
    } catch (error) {
      console.error("[AddressAPI] Error deleting address:", error);
      throw new Error(getErrorMessage(error));
    }
  },

  /**
   * Validate số điện thoại Việt Nam
   * @param {string} phoneNumber - Số điện thoại cần validate
   * @returns {boolean} True nếu hợp lệ
   */
  validatePhoneNumber: (phoneNumber) => {
    const regex = /^(0|\+84)[0-9]{9,10}$/;
    return regex.test(phoneNumber);
  },

  /**
   * Format địa chỉ đầy đủ
   * @param {Object} address - Object địa chỉ
   * @returns {string} Địa chỉ đầy đủ
   */
  formatFullAddress: (address) => {
    const parts = [
      address.street,
      address.ward,
      address.district,
      address.city,
    ].filter(Boolean);
    return parts.join(", ");
  },
};

export default AddressAPI;
