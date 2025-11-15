import { useState, useEffect, useCallback } from "react";
import AddressAPI from "../API/AddressAPI";

/**
 * Custom hook để quản lý địa chỉ
 */
export const useAddress = () => {
  const [addresses, setAddresses] = useState([]);
  const [defaultAddress, setDefaultAddress] = useState(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);

  /**
   * Lấy danh sách địa chỉ
   */
  const fetchAddresses = useCallback(async () => {
    setLoading(true);
    setError(null);
    try {
      const data = await AddressAPI.getAddresses();
      setAddresses(data);

      // Tìm địa chỉ mặc định
      const defaultAddr = data.find((addr) => addr.isDefault);
      setDefaultAddress(defaultAddr || null);

      return data;
    } catch (err) {
      setError(err.message);
      console.error("Error fetching addresses:", err);
      return [];
    } finally {
      setLoading(false);
    }
  }, []);

  /**
   * Lấy địa chỉ mặc định
   */
  const fetchDefaultAddress = useCallback(async () => {
    try {
      const data = await AddressAPI.getDefaultAddress();
      setDefaultAddress(data);
      return data;
    } catch (err) {
      console.error("Error fetching default address:", err);
      return null;
    }
  }, []);

  /**
   * Thêm địa chỉ mới
   */
  const addAddress = useCallback(
    async (addressData) => {
      setLoading(true);
      setError(null);
      try {
        const newAddress = await AddressAPI.createAddress(addressData);
        await fetchAddresses(); // Refresh danh sách
        return { success: true, data: newAddress };
      } catch (err) {
        setError(err.message);
        return { success: false, error: err.message };
      } finally {
        setLoading(false);
      }
    },
    [fetchAddresses]
  );

  /**
   * Cập nhật địa chỉ
   */
  const updateAddress = useCallback(
    async (id, addressData) => {
      setLoading(true);
      setError(null);
      try {
        const updatedAddress = await AddressAPI.updateAddress(id, addressData);
        await fetchAddresses(); // Refresh danh sách
        return { success: true, data: updatedAddress };
      } catch (err) {
        setError(err.message);
        return { success: false, error: err.message };
      } finally {
        setLoading(false);
      }
    },
    [fetchAddresses]
  );

  /**
   * Xóa địa chỉ
   */
  const deleteAddress = useCallback(
    async (id) => {
      setLoading(true);
      setError(null);
      try {
        await AddressAPI.deleteAddress(id);
        await fetchAddresses(); // Refresh danh sách
        return { success: true };
      } catch (err) {
        setError(err.message);
        return { success: false, error: err.message };
      } finally {
        setLoading(false);
      }
    },
    [fetchAddresses]
  );

  /**
   * Đặt địa chỉ làm mặc định
   */
  const setAsDefault = useCallback(
    async (id) => {
      setLoading(true);
      setError(null);
      try {
        await AddressAPI.setDefaultAddress(id);
        await fetchAddresses(); // Refresh danh sách
        return { success: true };
      } catch (err) {
        setError(err.message);
        return { success: false, error: err.message };
      } finally {
        setLoading(false);
      }
    },
    [fetchAddresses]
  );

  /**
   * Lấy địa chỉ theo ID
   */
  const getAddressById = useCallback(async (id) => {
    try {
      const address = await AddressAPI.getAddressById(id);
      return { success: true, data: address };
    } catch (err) {
      return { success: false, error: err.message };
    }
  }, []);

  // Load addresses khi component mount
  useEffect(() => {
    fetchAddresses();
  }, [fetchAddresses]);

  return {
    addresses,
    defaultAddress,
    loading,
    error,
    fetchAddresses,
    fetchDefaultAddress,
    addAddress,
    updateAddress,
    deleteAddress,
    setAsDefault,
    getAddressById,
    refetch: fetchAddresses,
  };
};

export default useAddress;
