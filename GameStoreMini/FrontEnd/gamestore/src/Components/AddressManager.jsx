import { useState } from "react";
import { useAddress } from "../hooks/useAddress";
import LocationAPI from "../API/LocationAPI";
import "../Decorate/AddressManager.css";

/**
 * Component quản lý địa chỉ giao hàng
 */
const AddressManager = ({ onSelectAddress, selectMode = false }) => {
  const {
    addresses,
    loading,
    addAddress,
    updateAddress,
    deleteAddress,
    setAsDefault,
  } = useAddress();

  const [showForm, setShowForm] = useState(false);
  const [editingId, setEditingId] = useState(null);
  const [locations, setLocations] = useState([]);
  const [loadingLocations, setLoadingLocations] = useState(false);

  const [formData, setFormData] = useState({
    fullName: "",
    phoneNumber: "",
    street: "",
    ward: "",
    district: "",
    city: "",
    postalCode: "",
    isDefault: false,
  });

  const [formErrors, setFormErrors] = useState({});
  const [selectedAddressId, setSelectedAddressId] = useState(null);

  // Load locations khi mở form
  const handleShowForm = async () => {
    setShowForm(true);
    if (locations.length === 0) {
      await loadLocations();
    }
  };

  const loadLocations = async () => {
    setLoadingLocations(true);
    try {
      const data = await LocationAPI.getLocations();
      setLocations(data);
    } catch (error) {
      console.error("Error loading locations:", error);
      alert("Không thể tải danh sách địa điểm");
    } finally {
      setLoadingLocations(false);
    }
  };

  // Reset form
  const resetForm = () => {
    setFormData({
      fullName: "",
      phoneNumber: "",
      street: "",
      ward: "",
      district: "",
      city: "",
      postalCode: "",
      isDefault: false,
    });
    setFormErrors({});
    setEditingId(null);
  };

  // Validate form
  const validateForm = () => {
    const errors = {};

    if (!formData.fullName.trim()) {
      errors.fullName = "Họ tên là bắt buộc";
    }

    if (!formData.phoneNumber.trim()) {
      errors.phoneNumber = "Số điện thoại là bắt buộc";
    } else if (!/^(0|\+84)[0-9]{9,10}$/.test(formData.phoneNumber)) {
      errors.phoneNumber = "Số điện thoại không hợp lệ";
    }

    if (!formData.street.trim()) {
      errors.street = "Địa chỉ đường/số nhà là bắt buộc";
    }

    if (!formData.city) {
      errors.city = "Vui lòng chọn Tỉnh/Thành phố";
    }

    if (!formData.district) {
      errors.district = "Vui lòng chọn Quận/Huyện";
    }

    if (!formData.ward) {
      errors.ward = "Vui lòng chọn Phường/Xã";
    }

    setFormErrors(errors);
    return Object.keys(errors).length === 0;
  };

  // Submit form
  const handleSubmit = async (e) => {
    e.preventDefault();

    if (!validateForm()) {
      return;
    }

    const result = editingId
      ? await updateAddress(editingId, formData)
      : await addAddress(formData);

    if (result.success) {
      setShowForm(false);
      resetForm();
      alert(
        editingId ? "Cập nhật địa chỉ thành công" : "Thêm địa chỉ thành công"
      );
    } else {
      alert(result.error || "Có lỗi xảy ra");
    }
  };

  // Edit address
  const handleEdit = (address) => {
    setFormData({
      fullName: address.fullName,
      phoneNumber: address.phoneNumber,
      street: address.street,
      ward: address.ward,
      district: address.district,
      city: address.city,
      postalCode: address.postalCode || "",
      isDefault: address.isDefault,
    });
    setEditingId(address.id);
    handleShowForm();
  };

  // Delete address
  const handleDelete = async (id) => {
    if (!window.confirm("Bạn có chắc muốn xóa địa chỉ này?")) {
      return;
    }

    const result = await deleteAddress(id);
    if (result.success) {
      alert("Xóa địa chỉ thành công");
    } else {
      alert(result.error || "Không thể xóa địa chỉ");
    }
  };

  // Set default address
  const handleSetDefault = async (id) => {
    const result = await setAsDefault(id);
    if (result.success) {
      alert("Đã đặt làm địa chỉ mặc định");
    } else {
      alert(result.error || "Không thể đặt làm địa chỉ mặc định");
    }
  };

  // Select address (for checkout)
  const handleSelectAddress = (address) => {
    if (selectMode && onSelectAddress) {
      setSelectedAddressId(address.id);
      onSelectAddress(address);
    }
  };

  // Get location options
  const getCityOptions = () => {
    return [...new Set(locations.map((l) => l.city))].sort();
  };

  const getDistrictOptions = (city) => {
    return [
      ...new Set(
        locations.filter((l) => l.city === city).map((l) => l.district)
      ),
    ].sort();
  };

  const getWardOptions = (city, district) => {
    return locations
      .filter((l) => l.city === city && l.district === district)
      .map((l) => l.ward)
      .sort();
  };

  // Handle city change
  const handleCityChange = (city) => {
    setFormData({
      ...formData,
      city,
      district: "",
      ward: "",
    });
  };

  // Handle district change
  const handleDistrictChange = (district) => {
    setFormData({
      ...formData,
      district,
      ward: "",
    });
  };

  if (loading) {
    return <div className="address-loading">Đang tải địa chỉ...</div>;
  }

  return (
    <div className="address-manager">
      {/* Header */}
      <div className="address-header">
        <h3>Địa chỉ giao hàng</h3>
        <button
          onClick={() => {
            resetForm();
            handleShowForm();
          }}
          className="btn-add"
        >
          + Thêm địa chỉ mới
        </button>
      </div>

      {/* Address List */}
      {addresses.length > 0 && (
        <div className="address-list">
          {addresses.map((addr) => (
            <div
              key={addr.id}
              className={`address-card ${addr.isDefault ? "default" : ""} ${
                selectMode && selectedAddressId === addr.id ? "selected" : ""
              }`}
              onClick={() => handleSelectAddress(addr)}
            >
              {/* Radio button for selection */}
              {selectMode && (
                <input
                  type="radio"
                  checked={selectedAddressId === addr.id}
                  onChange={() => handleSelectAddress(addr)}
                  className="address-radio"
                />
              )}

              {/* Address Info */}
              <div className="address-info">
                <div className="address-name">
                  <strong>{addr.fullName}</strong>
                  {addr.isDefault && (
                    <span className="badge-default">Mặc định</span>
                  )}
                </div>
                <p className="address-phone">{addr.phoneNumber}</p>
                <p className="address-text">
                  {addr.street}, {addr.ward}, {addr.district}, {addr.city}
                </p>
              </div>

              {/* Actions */}
              <div className="address-actions">
                <button
                  onClick={(e) => {
                    e.stopPropagation();
                    handleEdit(addr);
                  }}
                  className="btn-edit"
                >
                  Sửa
                </button>
                {!addr.isDefault && (
                  <button
                    onClick={(e) => {
                      e.stopPropagation();
                      handleSetDefault(addr.id);
                    }}
                    className="btn-set-default"
                  >
                    Đặt mặc định
                  </button>
                )}
                <button
                  onClick={(e) => {
                    e.stopPropagation();
                    handleDelete(addr.id);
                  }}
                  className="btn-delete"
                >
                  Xóa
                </button>
              </div>
            </div>
          ))}
        </div>
      )}

      {/* Empty state */}
      {addresses.length === 0 && !showForm && (
        <div className="address-empty">
          <p>Bạn chưa có địa chỉ giao hàng nào</p>
          <button onClick={handleShowForm} className="btn-add-first">
            Thêm địa chỉ đầu tiên
          </button>
        </div>
      )}

      {/* Add/Edit Form */}
      {showForm && (
        <div className="address-form-overlay">
          <div className="address-form-container">
            <div className="form-header">
              <h3>{editingId ? "Cập nhật địa chỉ" : "Thêm địa chỉ mới"}</h3>
              <button
                onClick={() => {
                  setShowForm(false);
                  resetForm();
                }}
                className="btn-close"
              >
                ×
              </button>
            </div>

            <form onSubmit={handleSubmit} className="address-form">
              {/* Full Name */}
              <div className="form-group">
                <label>Họ tên người nhận *</label>
                <input
                  type="text"
                  value={formData.fullName}
                  onChange={(e) =>
                    setFormData({ ...formData, fullName: e.target.value })
                  }
                  placeholder="Nguyễn Văn A"
                  className={formErrors.fullName ? "error" : ""}
                />
                {formErrors.fullName && (
                  <span className="error-text">{formErrors.fullName}</span>
                )}
              </div>

              {/* Phone Number */}
              <div className="form-group">
                <label>Số điện thoại *</label>
                <input
                  type="tel"
                  value={formData.phoneNumber}
                  onChange={(e) =>
                    setFormData({ ...formData, phoneNumber: e.target.value })
                  }
                  placeholder="0901234567"
                  className={formErrors.phoneNumber ? "error" : ""}
                />
                {formErrors.phoneNumber && (
                  <span className="error-text">{formErrors.phoneNumber}</span>
                )}
              </div>

              {/* City */}
              <div className="form-group">
                <label>Tỉnh/Thành phố *</label>
                <select
                  value={formData.city}
                  onChange={(e) => handleCityChange(e.target.value)}
                  disabled={loadingLocations}
                  className={formErrors.city ? "error" : ""}
                >
                  <option value="">-- Chọn Tỉnh/Thành phố --</option>
                  {getCityOptions().map((city) => (
                    <option key={city} value={city}>
                      {city}
                    </option>
                  ))}
                </select>
                {formErrors.city && (
                  <span className="error-text">{formErrors.city}</span>
                )}
              </div>

              {/* District */}
              <div className="form-group">
                <label>Quận/Huyện *</label>
                <select
                  value={formData.district}
                  onChange={(e) => handleDistrictChange(e.target.value)}
                  disabled={!formData.city || loadingLocations}
                  className={formErrors.district ? "error" : ""}
                >
                  <option value="">-- Chọn Quận/Huyện --</option>
                  {getDistrictOptions(formData.city).map((district) => (
                    <option key={district} value={district}>
                      {district}
                    </option>
                  ))}
                </select>
                {formErrors.district && (
                  <span className="error-text">{formErrors.district}</span>
                )}
              </div>

              {/* Ward */}
              <div className="form-group">
                <label>Phường/Xã *</label>
                <select
                  value={formData.ward}
                  onChange={(e) =>
                    setFormData({ ...formData, ward: e.target.value })
                  }
                  disabled={!formData.district || loadingLocations}
                  className={formErrors.ward ? "error" : ""}
                >
                  <option value="">-- Chọn Phường/Xã --</option>
                  {getWardOptions(formData.city, formData.district).map(
                    (ward) => (
                      <option key={ward} value={ward}>
                        {ward}
                      </option>
                    )
                  )}
                </select>
                {formErrors.ward && (
                  <span className="error-text">{formErrors.ward}</span>
                )}
              </div>

              {/* Street */}
              <div className="form-group">
                <label>Địa chỉ cụ thể *</label>
                <input
                  type="text"
                  value={formData.street}
                  onChange={(e) =>
                    setFormData({ ...formData, street: e.target.value })
                  }
                  placeholder="Số nhà, tên đường"
                  className={formErrors.street ? "error" : ""}
                />
                {formErrors.street && (
                  <span className="error-text">{formErrors.street}</span>
                )}
              </div>

              {/* Postal Code */}
              <div className="form-group">
                <label>Mã bưu điện</label>
                <input
                  type="text"
                  value={formData.postalCode}
                  onChange={(e) =>
                    setFormData({ ...formData, postalCode: e.target.value })
                  }
                  placeholder="700000"
                  maxLength="10"
                />
              </div>

              {/* Set as Default */}
              <div className="form-group checkbox-group">
                <label className="checkbox-label">
                  <input
                    type="checkbox"
                    checked={formData.isDefault}
                    onChange={(e) =>
                      setFormData({ ...formData, isDefault: e.target.checked })
                    }
                  />
                  Đặt làm địa chỉ mặc định
                </label>
              </div>

              {/* Buttons */}
              <div className="form-actions">
                <button
                  type="button"
                  onClick={() => {
                    setShowForm(false);
                    resetForm();
                  }}
                  className="btn-cancel"
                >
                  Hủy
                </button>
                <button type="submit" className="btn-submit">
                  {editingId ? "Cập nhật" : "Thêm địa chỉ"}
                </button>
              </div>
            </form>
          </div>
        </div>
      )}
    </div>
  );
};

export default AddressManager;
