import React, { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { useCart } from "../Cart/CartProvider";
import { useAddress } from "../hooks/useAddress";
import AddressManager from "../Components/AddressManager";
import OrderAPI from "../API/OrderAPI";
import PaymentAPI from "../API/PaymentAPI";
import LocationAPI from "../API/LocationAPI";
import OrderSummary from "../Components/OrderSummary";
import formatCurrency from "../Utils/formatCurrency";
import { getErrorMessage } from "../API/ApiClient";
import { isAuthenticated, getUserEmail } from "../Auth/useAuth";
import "../Decorate/Payment.css";

export default function Payment() {
  const navigate = useNavigate();
  const { items: cartItems, count, clearCart } = useCart();
  const { addresses, defaultAddress, addAddress } = useAddress();

  const [loading, setLoading] = useState(false);
  const [showAddressBook, setShowAddressBook] = useState(false);
  const [selectedAddress, setSelectedAddress] = useState(null);
  const [paymentMethod, setPaymentMethod] = useState("vnpay");
  const [orderNotes, setOrderNotes] = useState("");
  const [error, setError] = useState(null);

  const [formData, setFormData] = useState({
    fullName: "",
    phoneNumber: "",
    email: "", // ⭐ THÊM
    city: "",
    district: "",
    ward: "",
    detailedAddress: "",
    saveAddress: false,
  });

  // Location data
  const [cities, setCities] = useState([]);
  const [districts, setDistricts] = useState([]);
  const [wards, setWards] = useState([]);
  const [loadingCities, setLoadingCities] = useState(false);
  const [loadingDistricts, setLoadingDistricts] = useState(false);
  const [loadingWards, setLoadingWards] = useState(false);

  // Check authentication
  useEffect(() => {
    if (!isAuthenticated()) {
      navigate("/login?redirect=/payment");
      return;
    }

    // Load user email from token
    const userEmail = getUserEmail();
    if (userEmail && !formData.email) {
      setFormData((prev) => ({ ...prev, email: userEmail }));
    }
  }, [navigate]);

  // Redirect nếu giỏ hàng trống
  useEffect(() => {
    if (!cartItems || cartItems.length === 0) {
      navigate("/cart");
    }
  }, [cartItems, navigate]);

  // Load cities on mount
  useEffect(() => {
    const fetchCities = async () => {
      try {
        setLoadingCities(true);
        const citiesData = await LocationAPI.getCities();
        setCities(Array.isArray(citiesData) ? citiesData : []);
      } catch (error) {
        console.error("Error loading cities:", error);
      } finally {
        setLoadingCities(false);
      }
    };
    fetchCities();
  }, []);

  // Set default address khi có
  useEffect(() => {
    if (defaultAddress && !selectedAddress && !showAddressBook) {
      setFormData((prev) => ({
        ...prev,
        fullName: defaultAddress.fullName || "",
        phoneNumber: defaultAddress.phoneNumber || "",
        city: defaultAddress.city || "",
        district: defaultAddress.district || "",
        ward: defaultAddress.ward || "",
        detailedAddress: defaultAddress.street || "",
      }));
      setSelectedAddress(defaultAddress);
    }
  }, [defaultAddress, selectedAddress, showAddressBook]);

  // Load districts khi chọn city
  useEffect(() => {
    if (!formData.city) {
      setDistricts([]);
      setFormData((prev) => ({ ...prev, district: "", ward: "" }));
      return;
    }
    const fetchDistricts = async () => {
      try {
        setLoadingDistricts(true);
        const districtsData = await LocationAPI.getDistricts(formData.city);
        setDistricts(Array.isArray(districtsData) ? districtsData : []);
      } catch (error) {
        console.error("Error loading districts:", error);
      } finally {
        setLoadingDistricts(false);
      }
    };
    fetchDistricts();
  }, [formData.city]);

  // Load wards khi chọn district
  useEffect(() => {
    if (!formData.district) {
      setWards([]);
      setFormData((prev) => ({ ...prev, ward: "" }));
      return;
    }
    const fetchWards = async () => {
      try {
        setLoadingWards(true);
        const wardsData = await LocationAPI.getWards(
          formData.city,
          formData.district
        );
        setWards(Array.isArray(wardsData) ? wardsData : []);
      } catch (error) {
        console.error("Error loading wards:", error);
      } finally {
        setLoadingWards(false);
      }
    };
    fetchWards();
  }, [formData.city, formData.district]);

  // Use shared helper for currency formatting

  // Tính toán
  const calculateSubtotal = () => {
    return cartItems.reduce(
      (sum, item) => sum + (item.price || 0) * (item.qty || item.quantity || 0),
      0
    );
  };

  const calculateShipping = () => {
    const subtotal = calculateSubtotal();
    return subtotal >= 500000 ? 0 : 30000;
  };

  const calculateTax = () => {
    return Math.round(calculateSubtotal() * 0.1);
  };

  const calculateTotal = () => {
    return calculateSubtotal() + calculateShipping() + calculateTax();
  };

  // Handle place order
  const handlePlaceOrder = async () => {
    // Validate form
    if (
      !formData.fullName ||
      !formData.phoneNumber ||
      !formData.email ||
      !formData.city ||
      !formData.district ||
      !formData.ward ||
      !formData.detailedAddress
    ) {
      setError("Vui lòng điền đầy đủ thông tin giao hàng");
      return;
    }

    // Validate email
    const emailRegex = /^\S+@\S+\.\S+$/;
    if (!emailRegex.test(formData.email)) {
      setError("Email không hợp lệ");
      return;
    }

    // Validate phone
    const phoneRegex = /^(0[3|5|7|8|9])+([0-9]{8})$/;
    if (!phoneRegex.test(formData.phoneNumber)) {
      setError("Số điện thoại không hợp lệ");
      return;
    }

    setLoading(true);
    setError(null);

    try {
      // Lưu địa chỉ nếu user chọn
      if (formData.saveAddress && isAuthenticated()) {
        try {
          await addAddress({
            fullName: formData.fullName,
            phoneNumber: formData.phoneNumber,
            city: formData.city,
            district: formData.district,
            ward: formData.ward,
            street: formData.detailedAddress,
            isDefault: addresses.length === 0, // Đặt làm mặc định nếu là địa chỉ đầu tiên
          });
        } catch (err) {
          console.error("Error saving address:", err);
          // Không block order nếu lưu địa chỉ thất bại
        }
      }

      const orderData = {
        items: cartItems.map((item) => ({
          gameId: item.gameId,
          quantity: item.qty || item.quantity || 1,
          price: item.price,
        })),
        total: calculateTotal(),
        paymentMethod: paymentMethod,
        customerName: formData.fullName,
        customerEmail: formData.email,
        customerPhone: formData.phoneNumber,
        shippingAddress: `${formData.detailedAddress}, ${formData.ward}, ${formData.district}, ${formData.city}`,
        shippingCity: formData.city,
        shippingDistrict: formData.district,
        orderNotes: orderNotes,
      };

      console.log("Creating order:", orderData);
      const response = await OrderAPI.createOrder(orderData);

      if (response?.id || response?.orderNumber) {
        const orderNumber = response.orderNumber || response.OrderNumber;

        // XỬ LÝ THANH TOÁN VNPAY
        if (paymentMethod === "vnpay") {
          try {
            // Gọi PaymentAPI để tạo payment URL
            const paymentData = await PaymentAPI.createPaymentUrl(orderNumber);

            if (paymentData.paymentUrl) {
              // Chuyển hướng sang cổng thanh toán VNPay
              window.location.href = paymentData.paymentUrl;
              return;
            } else {
              setError("Không thể tạo link thanh toán VNPay");
            }
          } catch (err) {
            console.error("Payment error:", err);
            setError("Lỗi kết nối với VNPay: " + err.message);
          }
          setLoading(false);
          return;
        }

        // XỬ LÝ THANH TOÁN COD (giữ nguyên)
        await clearCart();
        navigate("/order/success", {
          state: {
            orderId: response.id,
            orderNumber: orderNumber,
            total: response.total || orderData.total,
            createdAt: response.createdAt || new Date().toISOString(),
          },
          replace: true,
        });
      } else {
        throw new Error("Không thể tạo đơn hàng");
      }
    } catch (err) {
      console.error("Error placing order:", err);
      setError(getErrorMessage(err));
    } finally {
      setLoading(false);
    }
  };

  // Handle select from address book
  const handleSelectFromAddressBook = (address) => {
    setFormData({
      fullName: address.fullName,
      phoneNumber: address.phoneNumber,
      city: address.city,
      district: address.district,
      ward: address.ward,
      detailedAddress: address.street,
    });
    setSelectedAddress(address);
    setShowAddressBook(false);
  };

  // Show loading while checking cart
  if (!cartItems) {
    return (
      <div style={{ padding: "20px", textAlign: "center" }}>Đang tải...</div>
    );
  }

  // Debug log
  console.log("Payment render - cartItems:", cartItems);
  console.log("Payment render - addresses:", addresses);
  console.log("Payment render - cities:", cities);

  return (
    <div className="payment-page">
      <div className="payment-container">
        {/* Left - Address & Payment */}
        <div className="payment-left">
          <h2>Thanh toán</h2>

          {/* Address Form */}
          <section className="payment-section">
            <div
              style={{
                display: "flex",
                justifyContent: "space-between",
                alignItems: "center",
                marginBottom: "16px",
              }}
            >
              <h3>📍 Địa chỉ giao hàng</h3>
              {addresses && addresses.length > 0 && !showAddressBook && (
                <button
                  type="button"
                  className="link-btn"
                  onClick={() => setShowAddressBook(true)}
                  style={{
                    color: "#1976d2",
                    background: "none",
                    border: "none",
                    cursor: "pointer",
                    fontSize: "14px",
                    textDecoration: "underline",
                  }}
                >
                  📖 Chọn từ sổ địa chỉ
                </button>
              )}
            </div>

            {/* Address Book Modal */}
            {showAddressBook && (
              <div
                style={{
                  marginBottom: "20px",
                  padding: "16px",
                  background: "#f5f5f5",
                  borderRadius: "8px",
                }}
              >
                <div
                  style={{
                    display: "flex",
                    justifyContent: "space-between",
                    marginBottom: "12px",
                  }}
                >
                  <h4>Sổ địa chỉ của bạn</h4>
                  <button
                    type="button"
                    onClick={() => setShowAddressBook(false)}
                    style={{
                      background: "none",
                      border: "none",
                      cursor: "pointer",
                      fontSize: "18px",
                    }}
                  >
                    ✕
                  </button>
                </div>
                <AddressManager
                  selectMode={true}
                  onSelectAddress={handleSelectFromAddressBook}
                />
              </div>
            )}

            {/* Address Input Form */}
            {!showAddressBook && (
              <div className="address-form">
                <div
                  className="form-row"
                  style={{
                    display: "grid",
                    gridTemplateColumns: "1fr 1fr",
                    gap: "12px",
                  }}
                >
                  <div className="input">
                    <label className="label">
                      Họ và tên <span style={{ color: "red" }}>*</span>
                    </label>
                    <input
                      type="text"
                      placeholder="Nguyễn Văn A"
                      value={formData.fullName}
                      onChange={(e) =>
                        setFormData({ ...formData, fullName: e.target.value })
                      }
                      style={{
                        width: "100%",
                        padding: "10px 12px",
                        border: "1px solid #ddd",
                        borderRadius: "6px",
                        fontSize: "14px",
                      }}
                    />
                  </div>

                  <div className="input">
                    <label className="label">
                      Số điện thoại <span style={{ color: "red" }}>*</span>
                    </label>
                    <input
                      type="tel"
                      placeholder="0901234567"
                      value={formData.phoneNumber}
                      onChange={(e) =>
                        setFormData({
                          ...formData,
                          phoneNumber: e.target.value,
                        })
                      }
                      style={{
                        width: "100%",
                        padding: "10px 12px",
                        border: "1px solid #ddd",
                        borderRadius: "6px",
                        fontSize: "14px",
                      }}
                    />
                  </div>
                </div>

                {/* Email field */}
                <div className="input">
                  <label className="label">
                    Email <span style={{ color: "red" }}>*</span>
                  </label>
                  <input
                    type="email"
                    placeholder="example@email.com"
                    value={formData.email}
                    onChange={(e) =>
                      setFormData({ ...formData, email: e.target.value })
                    }
                    style={{
                      width: "100%",
                      padding: "10px 12px",
                      border: "1px solid #ddd",
                      borderRadius: "6px",
                      fontSize: "14px",
                    }}
                    required
                  />
                </div>

                <div
                  className="form-row"
                  style={{
                    display: "grid",
                    gridTemplateColumns: "1fr 1fr 1fr",
                    gap: "12px",
                    marginTop: "12px",
                  }}
                >
                  <div className="input">
                    <label className="label">
                      Tỉnh/Thành phố <span style={{ color: "red" }}>*</span>
                    </label>
                    <select
                      value={formData.city}
                      onChange={(e) => {
                        setFormData({ ...formData, city: e.target.value });
                        setDistricts([]);
                        setFormData((prev) => ({
                          ...prev,
                          district: "",
                          ward: "",
                        }));
                      }}
                      disabled={loadingCities}
                      style={{
                        width: "100%",
                        padding: "10px 12px",
                        border: "1px solid #ddd",
                        borderRadius: "6px",
                        fontSize: "14px",
                        background: "white",
                      }}
                    >
                      <option value="">
                        {loadingCities ? "Đang tải..." : "Chọn tỉnh/thành"}
                      </option>
                      {cities.map((c) => (
                        <option key={c} value={c}>
                          {c}
                        </option>
                      ))}
                    </select>
                  </div>

                  <div className="input">
                    <label className="label">
                      Quận/Huyện <span style={{ color: "red" }}>*</span>
                    </label>
                    <select
                      value={formData.district}
                      onChange={(e) => {
                        setFormData({ ...formData, district: e.target.value });
                        setWard("");
                      }}
                      disabled={!formData.city || loadingDistricts}
                      style={{
                        width: "100%",
                        padding: "10px 12px",
                        border: "1px solid #ddd",
                        borderRadius: "6px",
                        fontSize: "14px",
                        background: "white",
                      }}
                    >
                      <option value="">
                        {loadingDistricts ? "Đang tải..." : "Chọn quận/huyện"}
                      </option>
                      {districts.map((d) => (
                        <option key={d} value={d}>
                          {d}
                        </option>
                      ))}
                    </select>
                  </div>

                  <div className="input">
                    <label className="label">
                      Phường/Xã <span style={{ color: "red" }}>*</span>
                    </label>
                    <select
                      value={formData.ward}
                      onChange={(e) =>
                        setFormData({ ...formData, ward: e.target.value })
                      }
                      disabled={!formData.district || loadingWards}
                      style={{
                        width: "100%",
                        padding: "10px 12px",
                        border: "1px solid #ddd",
                        borderRadius: "6px",
                        fontSize: "14px",
                        background: "white",
                      }}
                    >
                      <option value="">
                        {loadingWards ? "Đang tải..." : "Chọn phường/xã"}
                      </option>
                      {wards.map((w) => (
                        <option key={w} value={w}>
                          {w}
                        </option>
                      ))}
                    </select>
                  </div>
                </div>

                <div className="input" style={{ marginTop: "12px" }}>
                  <label className="label">
                    Địa chỉ chi tiết <span style={{ color: "red" }}>*</span>
                  </label>
                  <input
                    type="text"
                    placeholder="Số nhà, tên đường..."
                    value={formData.detailedAddress}
                    onChange={(e) =>
                      setFormData({
                        ...formData,
                        detailedAddress: e.target.value,
                      })
                    }
                    style={{
                      width: "100%",
                      padding: "10px 12px",
                      border: "1px solid #ddd",
                      borderRadius: "6px",
                      fontSize: "14px",
                    }}
                  />
                </div>

                {/* Save address checkbox */}
                {isAuthenticated() && (
                  <div style={{ marginTop: "12px" }}>
                    <label
                      style={{
                        display: "flex",
                        alignItems: "center",
                        cursor: "pointer",
                        fontSize: "14px",
                      }}
                    >
                      <input
                        type="checkbox"
                        checked={formData.saveAddress}
                        onChange={(e) =>
                          setFormData({
                            ...formData,
                            saveAddress: e.target.checked,
                          })
                        }
                        style={{ marginRight: "8px" }}
                      />
                      💾 Lưu địa chỉ này vào sổ địa chỉ
                    </label>
                  </div>
                )}
              </div>
            )}
          </section>

          {/* Payment Method */}
          <section className="payment-section">
            <h3>💳 Phương thức thanh toán</h3>
            <div className="options">
              <label className="option">
                <input
                  type="radio"
                  name="paymentMethod"
                  value="vnpay"
                  checked={paymentMethod === "vnpay"}
                  onChange={(e) => setPaymentMethod(e.target.value)}
                />
                <span>
                  🏛️ <strong>VNPay</strong>
                  <br />
                  <small>Thanh toán qua cổng VNPay</small>
                </span>
              </label>

              <label className="option">
                <input
                  type="radio"
                  name="paymentMethod"
                  value="cod"
                  checked={paymentMethod === "cod"}
                  onChange={(e) => setPaymentMethod(e.target.value)}
                />
                <span>
                  💸 <strong>Thanh toán khi nhận hàng (COD)</strong>
                  <br />
                  <small>Thanh toán bằng tiền mặt khi nhận hàng</small>
                </span>
              </label>

              <label className="option">
                <input
                  type="radio"
                  name="paymentMethod"
                  value="momo"
                  checked={paymentMethod === "momo"}
                  onChange={(e) => setPaymentMethod(e.target.value)}
                />
                <span>
                  📱 <strong>Ví MoMo</strong>
                  <br />
                  <small>Thanh toán qua ví điện tử MoMo</small>
                </span>
              </label>
            </div>
          </section>

          {/* Order Notes */}
          <section className="payment-section">
            <h3>📝 Ghi chú đơn hàng</h3>
            <textarea
              className="order-notes-textarea"
              placeholder="Nhập ghi chú cho người giao hàng (nếu có)..."
              value={orderNotes}
              onChange={(e) => setOrderNotes(e.target.value)}
              rows={4}
            />
          </section>

          {error && <div className="error-box">❌ {error}</div>}
        </div>

        {/* Right - Order Summary (extracted) */}
        <div className="payment-right">
          <OrderSummary
            items={cartItems}
            count={count}
            subtotal={calculateSubtotal()}
            shipping={calculateShipping()}
            tax={calculateTax()}
            total={calculateTotal()}
          />

          <button
            type="button"
            className="place-btn"
            onClick={handlePlaceOrder}
            disabled={
              loading ||
              !formData.fullName ||
              !formData.phoneNumber ||
              !formData.city ||
              !formData.district ||
              !formData.ward ||
              !formData.detailedAddress
            }
          >
            {loading
              ? "⏳ Đang xử lý..."
              : `🛍️ Đặt hàng • ${formatCurrency(calculateTotal())}`}
          </button>
        </div>
      </div>
    </div>
  );
}
