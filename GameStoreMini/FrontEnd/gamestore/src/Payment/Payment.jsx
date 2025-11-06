import React, { useState, useEffect, useRef } from "react";
import { useNavigate, useLocation } from "react-router-dom";
import { Formik, Form, Field, ErrorMessage } from "formik";
import * as Yup from "yup";
import { useCart } from "../Cart/CartProvider";
import { useToast } from "../Components/Toast";
import OrderAPI from "../API/OrderAPI";
import LocationAPI from "../API/LocationAPI";
import { isAuthenticated, getToken, decodeToken } from "../Auth/useAuth";
import "../Decorate/Payment.css";

// Validation schema cho form thanh toán
const validationSchema = Yup.object({
  // Thông tin khách hàng (luôn bắt buộc)
  customerName: Yup.string()
    .min(2, "Tên phải có ít nhất 2 ký tự")
    .max(50, "Tên không được vượt quá 50 ký tự")
    .matches(
      /^[a-zA-ZÀ-ỹ]+(\s+[a-zA-ZÀ-ỹ]+)+$/,
      "Vui lòng nhập họ và tên đầy đủ (ít nhất 2 từ)"
    )
    .required("Vui lòng nhập họ tên"),

  customerEmail: Yup.string()
    .email("Email không hợp lệ")
    .required("Vui lòng nhập email"),

  customerPhone: Yup.string()
    .matches(/^[0-9]{10,11}$/, "Số điện thoại phải có 10-11 chữ số")
    .required("Vui lòng nhập số điện thoại"),

  // Địa chỉ giao hàng
  shippingAddress: Yup.string()
    .min(10, "Địa chỉ phải có ít nhất 10 ký tự")
    .max(200, "Địa chỉ không được vượt quá 200 ký tự")
    .required("Vui lòng nhập địa chỉ giao hàng"),

  shippingCity: Yup.string()
    .min(2, "Tên thành phố phải có ít nhất 2 ký tự")
    .required("Vui lòng chọn tỉnh/thành phố"),

  shippingDistrict: Yup.string()
    .min(2, "Tên quận/huyện phải có ít nhất 2 ký tự")
    .required("Vui lòng chọn quận/huyện"),

  // Phương thức thanh toán
  paymentMethod: Yup.string()
    .oneOf(
      ["cod", "banking", "momo", "vnpay"],
      "Phương thức thanh toán không hợp lệ"
    )
    .required("Vui lòng chọn phương thức thanh toán"),

  // Thông tin thẻ (nếu chọn banking)
  cardNumber: Yup.string().when("paymentMethod", {
    is: "banking",
    then: (schema) =>
      schema
        .matches(/^[0-9]{16}$/, "Số thẻ phải có 16 chữ số")
        .required("Vui lòng nhập số thẻ"),
    otherwise: (schema) => schema.notRequired(),
  }),

  cardExpiry: Yup.string().when("paymentMethod", {
    is: "banking",
    then: (schema) =>
      schema
        .matches(/^(0[1-9]|1[0-2])\/\d{2}$/, "Định dạng MM/YY")
        .required("Vui lòng nhập ngày hết hạn"),
    otherwise: (schema) => schema.notRequired(),
  }),

  cardCvv: Yup.string().when("paymentMethod", {
    is: "banking",
    then: (schema) =>
      schema
        .matches(/^[0-9]{3,4}$/, "CVV phải có 3-4 chữ số")
        .required("Vui lòng nhập CVV"),
    otherwise: (schema) => schema.notRequired(),
  }),

  cardName: Yup.string().when("paymentMethod", {
    is: "banking",
    then: (schema) =>
      schema
        .min(2, "Tên chủ thẻ phải có ít nhất 2 ký tự")
        .required("Vui lòng nhập tên chủ thẻ"),
    otherwise: (schema) => schema.notRequired(),
  }),

  // Ghi chú đơn hàng
  orderNotes: Yup.string().max(500, "Ghi chú không được vượt quá 500 ký tự"),
});

// Component Payment - 100% sử dụng API, không có dữ liệu cứng

export default function Payment() {
  const navigate = useNavigate();
  const location = useLocation();
  const toast = useToast();
  const { items, count, clearCart } = useCart();

  const [loading, setLoading] = useState(false);
  const [showCardDetails, setShowCardDetails] = useState(false);
  const [showMoreShipping, setShowMoreShipping] = useState(false);
  const [userInfo, setUserInfo] = useState(null);
  const [cities, setCities] = useState([]);
  const [districts, setDistricts] = useState([]);
  const [loadingCities, setLoadingCities] = useState(true);
  const [loadingDistricts, setLoadingDistricts] = useState(false);

  const isAuth = isAuthenticated();

  // Lấy thông tin user nếu đã đăng nhập
  useEffect(() => {
    if (isAuth) {
      const token = getToken();
      const decoded = decodeToken(token);
      if (decoded) {
        setUserInfo({
          email: decoded.email,
          name: decoded.name || decoded.fullName || decoded.userName,
          phone: decoded.phoneNumber || decoded.phone || "",
          role: decoded.role,
        });
      }
    }
  }, [isAuth]);

  // Load danh sách tỉnh/thành phố từ API
  useEffect(() => {
    const loadCities = async () => {
      try {
        setLoadingCities(true);
        const citiesData = await LocationAPI.getCities("Việt Nam");
        setCities(Array.isArray(citiesData) ? citiesData : []);
      } catch (error) {
        console.error("Error loading cities from API:", error);
        toast.error(
          "Không thể tải danh sách tỉnh/thành phố từ server. Vui lòng thử lại sau."
        );
        // Chỉ sử dụng data từ API
        setCities([]);
      } finally {
        setLoadingCities(false);
      }
    };

    loadCities();
  }, [toast]);

  const hasShownEmptyCartToast = useRef(false);
  // Redirect nếu giỏ hàng trống
  useEffect(() => {
    if (items.length === 0 && !hasShownEmptyCartToast.current) {
      toast.error(
        "Giỏ hàng trống. Vui lòng thêm sản phẩm trước khi thanh toán.",
        {
          position: "top-center",
          autoClose: 10000,
        }
      );
      hasShownEmptyCartToast.current = true; // Đánh dấu đã hiển thị
    }
  }, [items, toast]);

  // Tính tổng giá
  const subtotal = items.reduce((sum, item) => sum + item.price * item.qty, 0);
  const shippingFee = subtotal >= 500000 ? 0 : 30000; // Miễn phí ship từ 500k
  const tax = Math.round(subtotal * 0.1); // VAT 10%
  const total = subtotal + shippingFee + tax;

  // Format currency
  const formatCurrency = (amount) => {
    return new Intl.NumberFormat("vi-VN").format(amount) + " đ";
  };

  // Handle form submit
  const handleSubmit = async (values, { setSubmitting, setFieldError }) => {
    try {
      setLoading(true);
      setSubmitting(true);

      const orderData = {
        items: items.map((item) => ({
          gameId: item.gameId,
          quantity: item.qty,
          price: item.price,
        })),
        total: total,
        paymentMethod: values.paymentMethod,
        customerName: values.customerName,
        customerEmail: values.customerEmail,
        customerPhone: values.customerPhone,
        shippingAddress: `${values.shippingAddress}, ${values.shippingDistrict}, ${values.shippingCity}`,
        shippingCity: values.shippingCity,
        shippingDistrict: values.shippingDistrict,
        orderNotes: values.orderNotes || "",
      };

      // Thêm thông tin thanh toán nếu là banking
      if (values.paymentMethod === "banking") {
        orderData.cardInfo = {
          number: values.cardNumber,
          expiry: values.cardExpiry,
          cvv: values.cardCvv,
          name: values.cardName,
        };
      }

      console.log("Order data:", orderData); // Debug log

      // KIỂM TRA TOKEN TRƯỚC KHI GỬI REQUEST
      const currentToken = localStorage.getItem("token");
      console.log(
        "Current token:",
        currentToken
          ? "EXISTS (length: " + currentToken.length + ")"
          : "NOT FOUND"
      );

      // Gọi API tạo đơn hàng
      const result = await OrderAPI.createOrder(orderData);

      if (result?.id || result?.orderNumber) {
        // Xóa giỏ hàng sau khi đặt hàng thành công
        await clearCart();

        // Chuyển hướng TRỰC TIẾP đến trang Order Success (không có toast, không delay)
        navigate("/order/success", {
          state: {
            orderId: result.id,
            orderNumber: result.orderNumber || result.OrderNumber,
            email: orderData.customerEmail,
            total: result.total || orderData.total,
            createdAt: result.createdAt || new Date().toISOString(),
          },
          replace: true,
        });
      } else {
        throw new Error("Không thể tạo đơn hàng");
      }
    } catch (error) {
      console.error("=== PAYMENT ERROR START ===");
      console.error("Payment error:", error);
      console.error("Error response:", error.response);
      console.error("Error status:", error.response?.status);
      console.error("Error data:", error.response?.data);
      console.error("=== PAYMENT ERROR END ===");

      // DỪNG TRANG HOÀN TOÀN BẰNG ALERT
      let errorMessage = "Lỗi không xác định";

      if (error.response?.data) {
        const errorData = error.response.data;
        if (typeof errorData === "string") {
          errorMessage = errorData;
        } else if (errorData.message) {
          errorMessage = errorData.message;
        } else if (errorData.title) {
          errorMessage = errorData.title;
        } else {
          errorMessage = JSON.stringify(errorData);
        }
      } else if (error.response?.status === 400) {
        errorMessage = "Thông tin đơn hàng không hợp lệ";
      } else if (error.response?.status === 401) {
        errorMessage =
          "Phiên đăng nhập hết hạn. Bạn sẽ được chuyển đến trang đăng nhập";
        // Alert trước khi redirect
        alert(
          `LỖI THANH TOÁN:\n\nStatus: 401\n\nMessage: ${errorMessage}\n\nVui lòng đăng nhập lại để tiếp tục!`
        );
        // Xóa token cũ
        localStorage.removeItem("token");
        localStorage.removeItem("refreshExpected");
        // Redirect sau 1 giây
        setTimeout(() => {
          window.location.href = "/login";
        }, 1000);
        return; // Dừng không chạy code bên dưới
      } else if (error.response?.status === 404) {
        errorMessage = "Không tìm thấy sản phẩm trong giỏ hàng";
      } else {
        errorMessage = error.message || "Có lỗi xảy ra khi xử lý đơn hàng";
      }

      // ALERT ĐỂ DỪNG TRANG
      alert(
        `LỖI THANH TOÁN:\n\nStatus: ${
          error.response?.status || "unknown"
        }\n\nMessage: ${errorMessage}\n\nVui lòng mở Console (F12) để xem chi tiết!`
      );

      toast.error(errorMessage, { autoClose: false });
    } finally {
      setLoading(false);
      setSubmitting(false);
    }
  };

  // Handle district change based on city - load from API
  const handleCityChange = async (city, setFieldValue) => {
    setFieldValue("shippingDistrict", "");

    if (!city) {
      setDistricts([]);
      return;
    }

    try {
      setLoadingDistricts(true);
      const districtsData = await LocationAPI.getDistricts(city);
      setDistricts(Array.isArray(districtsData) ? districtsData : []);

      if (districtsData.length === 0) {
        toast.warn(
          `Không có dữ liệu quận/huyện cho ${city}. Vui lòng liên hệ admin để cập nhật.`
        );
      }
    } catch (error) {
      console.error("Error loading districts from API:", error);
      toast.error(`Không thể tải danh sách quận/huyện cho ${city} từ server.`);
      setDistricts([]);
    } finally {
      setLoadingDistricts(false);
    }
  };

  if (count === 0) {
    return null; // Will redirect in useEffect
  }

  return (
    <div className="payment-page">
      <div className="payment-container">
        <div className="payment-left">
          <h2>Thông tin thanh toán</h2>

          <Formik
            initialValues={{
              isGuest: !isAuth,
              customerName: userInfo?.name || "",
              customerEmail: userInfo?.email || "",
              customerPhone: userInfo?.phone || "",
              shippingAddress: "",
              shippingCity: "",
              shippingDistrict: "",
              paymentMethod: "cod",
              cardNumber: "",
              cardExpiry: "",
              cardCvv: "",
              cardName: "",
              orderNotes: "",
            }}
            enableReinitialize={true}
            validationSchema={validationSchema}
            onSubmit={handleSubmit}
            validateOnBlur={true}
            validateOnChange={false}
          >
            {({ values, setFieldValue, isSubmitting, errors, touched }) => (
              <Form>
                {/* Thông tin khách hàng */}
                <section>
                  <h3>Thông tin khách hàng</h3>
                  <p className="section-subtitle">
                    {isAuth
                      ? "Thông tin của bạn (có thể chỉnh sửa nếu cần)"
                      : "Vui lòng cung cấp thông tin để chúng tôi liên hệ về đơn hàng"}
                  </p>

                  <div className="form-row">
                    <div className="input">
                      <label className="label">
                        Họ và tên <span className="req">*</span>
                      </label>
                      <Field
                        name="customerName"
                        type="text"
                        placeholder="Nhập họ và tên đầy đủ"
                        autoComplete="name"
                      />
                      <ErrorMessage
                        name="customerName"
                        component="div"
                        className="error"
                      />
                    </div>
                  </div>

                  <div className="form-row">
                    <div className="input">
                      <label className="label">
                        Email <span className="req">*</span>
                      </label>
                      <Field
                        name="customerEmail"
                        type="email"
                        placeholder="example@email.com"
                        autoComplete="email"
                      />
                      <ErrorMessage
                        name="customerEmail"
                        component="div"
                        className="error"
                      />
                    </div>

                    <div className="input">
                      <label className="label">
                        Số điện thoại <span className="req">*</span>
                      </label>
                      <Field
                        name="customerPhone"
                        type="tel"
                        placeholder="0901234567"
                        autoComplete="tel"
                      />
                      <ErrorMessage
                        name="customerPhone"
                        component="div"
                        className="error"
                      />
                    </div>
                  </div>
                </section>

                {/* Thông tin giao hàng */}
                <section>
                  <h3>Thông tin giao hàng</h3>
                  <p className="section-subtitle">Địa chỉ nhận hàng của bạn</p>

                  <div className="shipping-grid">
                    <div className="input">
                      <label className="label">
                        Tỉnh/Thành phố <span className="req">*</span>
                      </label>
                      <Field
                        as="select"
                        name="shippingCity"
                        disabled={loadingCities}
                        onChange={(e) => {
                          setFieldValue("shippingCity", e.target.value);
                          handleCityChange(e.target.value, setFieldValue);
                        }}
                      >
                        <option value="">
                          {loadingCities
                            ? "Đang tải..."
                            : "Chọn tỉnh/thành phố"}
                        </option>
                        {cities.map((city) => (
                          <option key={city} value={city}>
                            {city}
                          </option>
                        ))}
                      </Field>
                      <ErrorMessage
                        name="shippingCity"
                        component="div"
                        className="error"
                      />
                    </div>

                    <div className="input">
                      <label className="label">
                        Quận/Huyện <span className="req">*</span>
                      </label>
                      <Field
                        as="select"
                        name="shippingDistrict"
                        disabled={loadingDistricts}
                      >
                        <option value="">
                          {loadingDistricts ? "Đang tải..." : "Chọn quận/huyện"}
                        </option>
                        {districts.map((district) => (
                          <option key={district} value={district}>
                            {district}
                          </option>
                        ))}
                      </Field>
                      <ErrorMessage
                        name="shippingDistrict"
                        component="div"
                        className="error"
                      />
                    </div>

                    <div className="input full-width">
                      <label className="label">
                        Địa chỉ chi tiết <span className="req">*</span>
                      </label>
                      <Field
                        name="shippingAddress"
                        type="text"
                        placeholder="Số nhà, tên đường, phường/xã..."
                        autoComplete="street-address"
                      />
                      <ErrorMessage
                        name="shippingAddress"
                        component="div"
                        className="error"
                      />
                    </div>
                  </div>

                  {!showMoreShipping && (
                    <div className="more-toggle">
                      <button
                        type="button"
                        className="link-btn"
                        onClick={() => setShowMoreShipping(true)}
                      >
                        + Thêm ghi chú giao hàng
                      </button>
                    </div>
                  )}

                  {showMoreShipping && (
                    <div className="shipping-extras">
                      <div className="input">
                        <label className="label">Ghi chú đơn hàng</label>
                        <Field
                          as="textarea"
                          name="orderNotes"
                          rows="3"
                          placeholder="Ghi chú cho người giao hàng (tùy chọn)..."
                          style={{
                            width: "100%",
                            padding: "10px 12px",
                            border: "1px solid rgba(15, 23, 42, 0.08)",
                            borderRadius: "8px",
                            resize: "vertical",
                            background: "#fbfdff",
                          }}
                        />
                        <ErrorMessage
                          name="orderNotes"
                          component="div"
                          className="error"
                        />
                      </div>
                    </div>
                  )}
                </section>

                {/* Phương thức thanh toán */}
                <section className="payment-method">
                  <h3>Phương thức thanh toán</h3>

                  <div className="options">
                    <label className="option">
                      <Field
                        type="radio"
                        name="paymentMethod"
                        value="cod"
                        onChange={(e) => {
                          setFieldValue("paymentMethod", e.target.value);
                          setShowCardDetails(false);
                        }}
                      />
                      <span>
                        💸 <strong>Thanh toán khi nhận hàng (COD)</strong>
                        <br />
                        <small>Thanh toán bằng tiền mặt khi nhận hàng</small>
                      </span>
                    </label>

                    <label className="option">
                      <Field
                        type="radio"
                        name="paymentMethod"
                        value="banking"
                        onChange={(e) => {
                          setFieldValue("paymentMethod", e.target.value);
                          setShowCardDetails(e.target.value === "banking");
                        }}
                      />
                      <span>
                        💳 <strong>Thẻ tín dụng/ghi nợ</strong>
                        <br />
                        <small>Visa, Mastercard, JCB</small>
                      </span>
                    </label>

                    <label className="option">
                      <Field
                        type="radio"
                        name="paymentMethod"
                        value="momo"
                        onChange={(e) => {
                          setFieldValue("paymentMethod", e.target.value);
                          setShowCardDetails(false);
                        }}
                      />
                      <span>
                        📱 <strong>Ví MoMo</strong>
                        <br />
                        <small>Thanh toán qua ví điện tử MoMo</small>
                      </span>
                    </label>

                    <label className="option">
                      <Field
                        type="radio"
                        name="paymentMethod"
                        value="vnpay"
                        onChange={(e) => {
                          setFieldValue("paymentMethod", e.target.value);
                          setShowCardDetails(false);
                        }}
                      />
                      <span>
                        🏛️ <strong>VNPay</strong>
                        <br />
                        <small>Thanh toán qua cổng VNPay</small>
                      </span>
                    </label>
                  </div>

                  <ErrorMessage
                    name="paymentMethod"
                    component="div"
                    className="error"
                  />

                  {/* Chi tiết thẻ tín dụng */}
                  {showCardDetails && (
                    <div className="card-info">
                      <h4>Thông tin thẻ</h4>

                      <div className="form-row">
                        <div className="input">
                          <label className="label">
                            Số thẻ <span className="req">*</span>
                          </label>
                          <Field
                            name="cardNumber"
                            type="text"
                            placeholder="1234 5678 9012 3456"
                            maxLength="16"
                            onChange={(e) => {
                              const value = e.target.value.replace(/\D/g, "");
                              setFieldValue("cardNumber", value);
                            }}
                          />
                          <ErrorMessage
                            name="cardNumber"
                            component="div"
                            className="error"
                          />
                        </div>
                      </div>

                      <div className="form-row">
                        <div className="input">
                          <label className="label">
                            Ngày hết hạn <span className="req">*</span>
                          </label>
                          <Field
                            name="cardExpiry"
                            type="text"
                            placeholder="MM/YY"
                            maxLength="5"
                            onChange={(e) => {
                              let value = e.target.value.replace(/\D/g, "");
                              if (value.length >= 2) {
                                value =
                                  value.substring(0, 2) +
                                  "/" +
                                  value.substring(2, 4);
                              }
                              setFieldValue("cardExpiry", value);
                            }}
                          />
                          <ErrorMessage
                            name="cardExpiry"
                            component="div"
                            className="error"
                          />
                        </div>

                        <div className="input">
                          <label className="label">
                            CVV <span className="req">*</span>
                          </label>
                          <Field
                            name="cardCvv"
                            type="text"
                            placeholder="123"
                            maxLength="4"
                            onChange={(e) => {
                              const value = e.target.value.replace(/\D/g, "");
                              setFieldValue("cardCvv", value);
                            }}
                          />
                          <ErrorMessage
                            name="cardCvv"
                            component="div"
                            className="error"
                          />
                        </div>
                      </div>

                      <div className="input">
                        <label className="label">
                          Tên chủ thẻ <span className="req">*</span>
                        </label>
                        <Field
                          name="cardName"
                          type="text"
                          placeholder="NGUYEN VAN A"
                          style={{ textTransform: "uppercase" }}
                        />
                        <ErrorMessage
                          name="cardName"
                          component="div"
                          className="error"
                        />
                      </div>

                      <small
                        style={{ color: "var(--muted)", fontSize: "12px" }}
                      >
                        🔒 Thông tin thẻ của bạn được mã hóa và bảo mật
                      </small>
                    </div>
                  )}
                </section>

                {/* Submit button */}
                <button
                  type="submit"
                  className="place-btn"
                  disabled={isSubmitting || loading || count === 0}
                >
                  {loading || isSubmitting ? (
                    <>
                      <span style={{ opacity: 0.7 }}>⏳ Đang xử lý...</span>
                    </>
                  ) : (
                    `🛍️ Đặt hàng • ${formatCurrency(total)}`
                  )}
                </button>

                {/* Error display */}
                {Object.keys(errors).length > 0 && touched && (
                  <div className="error">
                    ⚠️ Vui lòng kiểm tra lại thông tin đã nhập
                  </div>
                )}
              </Form>
            )}
          </Formik>
        </div>

        {/* Order Summary */}
        <div className="payment-right">
          <h3>Đơn hàng của bạn</h3>

          <ul className="order-summary-list">
            {items.map((item) => (
              <li key={item.gameId}>
                <div>
                  <strong>{item.title || `Game ${item.gameId}`}</strong>
                  <br />
                  <small>Số lượng: {item.qty}</small>
                </div>
                <div>{formatCurrency(item.price * item.qty)}</div>
              </li>
            ))}
          </ul>

          <div className="order-summary-summary">
            <div className="order-line">
              <span>Tạm tính ({count} sản phẩm):</span>
              <span>{formatCurrency(subtotal)}</span>
            </div>

            <div className="order-line">
              <span>
                Phí vận chuyển:
                {shippingFee === 0 && (
                  <small style={{ color: "green", display: "block" }}>
                    ✅ Miễn phí (đơn ≥ 500k)
                  </small>
                )}
              </span>
              <span>{formatCurrency(shippingFee)}</span>
            </div>

            <div className="order-line">
              <span>VAT (10%):</span>
              <span>{formatCurrency(tax)}</span>
            </div>

            <hr
              style={{
                margin: "12px 0",
                border: "none",
                borderTop: "1px solid #eee",
              }}
            />

            <div className="order-line total-amount">
              <span>Tổng cộng:</span>
              <span>{formatCurrency(total)}</span>
            </div>

            {subtotal < 500000 && (
              <div
                style={{
                  marginTop: "12px",
                  padding: "10px",
                  background: "#fff3cd",
                  borderRadius: "6px",
                  fontSize: "13px",
                  color: "#856404",
                }}
              >
                💡 Mua thêm {formatCurrency(500000 - subtotal)} để được miễn phí
                ship!
              </div>
            )}

            {/* API Status Information */}
            <div
              style={{
                marginTop: "12px",
                padding: "8px",
                background: "#e8f5e8",
                borderRadius: "6px",
                fontSize: "12px",
                color: "#2d5a2d",
                borderLeft: "3px solid #4caf50",
              }}
            >
              🌐 Dữ liệu địa chỉ được tải từ server
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}
