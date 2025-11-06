import React, { useState, useRef, useEffect } from "react";
import { useNavigate, Link } from "react-router-dom";
import { Formik, Form, Field, ErrorMessage } from "formik";
import * as Yup from "yup";
import "../Decorate/form.css";
import UserAPI from "../API/UserAPI";

// simple password strength helper
function passwordStrength(pw) {
  if (!pw) return { score: 0, label: "Very weak" };
  let score = 0;
  if (pw.length >= 8) score++;
  if (/[A-Z]/.test(pw)) score++;
  if (/[0-9]/.test(pw)) score++;
  if (/[^A-Za-z0-9]/.test(pw)) score++;
  const labels = ["Very weak", "Weak", "Okay", "Strong", "Very strong"];
  return { score, label: labels[Math.min(score, labels.length - 1)] };
}

const validationSchema = Yup.object({
  fullName: Yup.string()
    .min(2, "Họ tên phải có ít nhất 2 ký tự")
    .max(100, "Họ tên quá dài")
    .matches(
      /^[a-zA-ZÀ-ỹ]+(\s+[a-zA-ZÀ-ỹ]+)+$/,
      "Vui lòng nhập họ và tên đầy đủ (ít nhất 2 từ)"
    )
    .required("Vui lòng nhập họ và tên"),
  userName: Yup.string()
    .min(3, "Tên đăng nhập phải có ít nhất 3 ký tự")
    .max(30, "Tên đăng nhập không được quá 30 ký tự")
    .matches(
      /^[a-zA-Z0-9._-]{3,30}$/,
      "Tên đăng nhập chỉ được chứa chữ cái, số, dấu chấm (.), gạch dưới (_), gạch ngang (-)"
    )
    .required("Vui lòng nhập tên đăng nhập"),
  email: Yup.string()
    .email("Email không hợp lệ")
    .max(200, "Email quá dài")
    .required("Vui lòng nhập email"),
  phoneNumber: Yup.string()
    .matches(
      /^(0|\+84)[0-9]{9,10}$/,
      "Số điện thoại không hợp lệ (VD: 0901234567 hoặc +84901234567)"
    )
    .required("Vui lòng nhập số điện thoại"),
  password: Yup.string()
    .min(8, "Mật khẩu phải có ít nhất 8 ký tự")
    .max(100, "Mật khẩu quá dài")
    .matches(
      /(?=.*[A-Za-z])(?=.*\d)/,
      "Mật khẩu phải chứa ít nhất 1 chữ cái và 1 chữ số"
    )
    .matches(
      /^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d@$!%*#?&]{8,}$/,
      "Mật khẩu chỉ được chứa chữ cái, số và các ký tự đặc biệt (@$!%*#?&)"
    )
    .required("Vui lòng nhập mật khẩu"),
  confirmPassword: Yup.string()
    .oneOf([Yup.ref("password"), null], "Mật khẩu xác nhận không khớp")
    .required("Vui lòng xác nhận mật khẩu"),
  termsAccepted: Yup.boolean().oneOf(
    [true],
    "Bạn phải đồng ý với điều khoản sử dụng"
  ),
});

export default function Register() {
  const navigate = useNavigate();
  const [showPwd, setShowPwd] = useState(false);
  const [showConfirm, setShowConfirm] = useState(false);
  const [avatarPreview, setAvatarPreview] = useState(null);
  const fileRef = useRef();
  const [message, setMessage] = useState(null);
  const [messageType, setMessageType] = useState(null);
  const clearMessage = () => {
    setMessage(null);
    setMessageType(null);
  };

  // Kiểm tra nếu đã đăng nhập thì redirect về home
  useEffect(() => {
    const token = localStorage.getItem("token");
    if (token) {
      console.log("User already logged in, redirecting to home");
      navigate("/", { replace: true });
    }
  }, [navigate]);

  const handleAvatarChange = (e, setFieldValue) => {
    const file = e.currentTarget.files && e.currentTarget.files[0];
    setFieldValue("avatar", file);
    if (file) setAvatarPreview(URL.createObjectURL(file));
    else setAvatarPreview(null);
  };

  return (
    <div className="form-shell">
      <h2>Tạo tài khoản mới</h2>

      <Formik
        initialValues={{
          fullName: "",
          userName: "",
          email: "",
          phoneNumber: "",
          password: "",
          confirmPassword: "",
          termsAccepted: false,
          avatar: null,
        }}
        validationSchema={validationSchema}
        onSubmit={async (values, { setSubmitting, setFieldError }) => {
          setSubmitting(true);
          try {
            const res = await UserAPI.register(values);
            // If backend returned a token, store it so user is logged in immediately
            const token = res?.Token || res?.token || null;
            if (token) UserAPI.setAuthToken(token);
            setMessageType("success");
            setMessage("Đăng ký thành công. Chuyển sang trang đăng nhập...");
            setTimeout(() => navigate("/"), 900);
          } catch (err) {
            const resp = err?.response?.data;
            if (resp?.errors) {
              const errors = resp.errors;
              for (const key in errors) {
                const field =
                  key === "UserName"
                    ? "userName"
                    : key === "PhoneNumber"
                    ? "phoneNumber"
                    : key === "TermsAccepted"
                    ? "termsAccepted"
                    : key === "FullName"
                    ? "fullName"
                    : key === "Password"
                    ? "password"
                    : key === "ConfirmPassword"
                    ? "confirmPassword"
                    : key === "Email"
                    ? "email"
                    : key;
                setFieldError(
                  field,
                  Array.isArray(errors[key])
                    ? errors[key].join(" ")
                    : String(errors[key])
                );
              }
            } else {
              const msg = resp?.title || err?.message || "Registration failed";
              setMessageType("error");
              setMessage(String(msg));
            }
          } finally {
            setSubmitting(false);
          }
        }}
      >
        {({ isSubmitting, values, setFieldValue }) => {
          const strength = passwordStrength(values.password);
          return (
            <Form className="form-grid" noValidate>
              {message && (
                <div
                  className={`notification ${messageType || "error"}`}
                  role="alert"
                >
                  <div className="msg">{message}</div>
                  <button
                    type="button"
                    className="close-btn"
                    onClick={clearMessage}
                  >
                    ×
                  </button>
                </div>
              )}

              <div className="field-row">
                <label htmlFor="avatar">Ảnh đại diện (tùy chọn)</label>
                <input
                  ref={fileRef}
                  id="avatar"
                  name="avatar"
                  type="file"
                  accept="image/*"
                  onChange={(e) => handleAvatarChange(e, setFieldValue)}
                />
                {avatarPreview && (
                  <img
                    src={avatarPreview}
                    alt="avatar preview"
                    style={{
                      width: 72,
                      height: 72,
                      objectFit: "cover",
                      borderRadius: 8,
                      marginTop: 8,
                    }}
                  />
                )}
                <div className="error">
                  <ErrorMessage name="avatar" />
                </div>
              </div>

              <div className="field-row">
                <label htmlFor="userName">Tên đăng nhập</label>
                <Field
                  id="userName"
                  name="userName"
                  type="text"
                  autoComplete="username"
                />
                <div className="error">
                  <ErrorMessage name="userName" />
                </div>
              </div>

              <div className="field-row">
                <label htmlFor="fullName">Họ và tên</label>
                <Field
                  id="fullName"
                  name="fullName"
                  type="text"
                  autoComplete="name"
                />
                <div className="error">
                  <ErrorMessage name="fullName" />
                </div>
              </div>

              <div className="field-row">
                <label htmlFor="phoneNumber">Số điện thoại</label>
                <Field
                  id="phoneNumber"
                  name="phoneNumber"
                  type="text"
                  placeholder="0901234567"
                  autoComplete="tel"
                />
                <div className="error">
                  <ErrorMessage name="phoneNumber" />
                </div>
              </div>

              <div className="field-row">
                <label htmlFor="email">Email</label>
                <Field
                  id="email"
                  name="email"
                  type="email"
                  autoComplete="email"
                />
                <div className="error">
                  <ErrorMessage name="email" />
                </div>
              </div>

              <div className="field-row">
                <label htmlFor="password">Mật khẩu</label>
                <div style={{ display: "flex", gap: 8, alignItems: "center" }}>
                  <Field
                    id="password"
                    name="password"
                    type={showPwd ? "text" : "password"}
                    autoComplete="new-password"
                  />
                  <button
                    type="button"
                    onClick={() => setShowPwd((s) => !s)}
                    className="btn secondary"
                  >
                    {showPwd ? "Ẩn" : "Hiện"}
                  </button>
                </div>
                <div className="helper">
                  Độ mạnh:{" "}
                  {strength.score === 0
                    ? "Rất yếu"
                    : strength.score === 1
                    ? "Yếu"
                    : strength.score === 2
                    ? "Trung bình"
                    : strength.score === 3
                    ? "Mạnh"
                    : "Rất mạnh"}
                </div>
                <div className="error">
                  <ErrorMessage name="password" />
                </div>
              </div>

              <div className="field-row">
                <label htmlFor="confirmPassword">Xác nhận mật khẩu</label>
                <div style={{ display: "flex", gap: 8, alignItems: "center" }}>
                  <Field
                    id="confirmPassword"
                    name="confirmPassword"
                    type={showConfirm ? "text" : "password"}
                    autoComplete="new-password"
                  />
                  <button
                    type="button"
                    onClick={() => setShowConfirm((s) => !s)}
                    className="btn secondary"
                  >
                    {showConfirm ? "Ẩn" : "Hiện"}
                  </button>
                </div>
                <div className="error">
                  <ErrorMessage name="confirmPassword" />
                </div>
              </div>

              <label style={{ display: "flex", gap: 8, alignItems: "center" }}>
                <Field name="termsAccepted" type="checkbox" />{" "}
                <span className="helper">
                  Tôi đồng ý với điều khoản sử dụng
                </span>
              </label>
              <div className="error">
                <ErrorMessage name="termsAccepted" />
              </div>

              <div className="actions">
                <button
                  className="btn"
                  type="submit"
                  disabled={isSubmitting || !values.termsAccepted}
                >
                  {isSubmitting ? "Đang tạo tài khoản..." : "Tạo tài khoản"}
                </button>
                <Link to="/login" className="btn secondary">
                  Đã có tài khoản?
                </Link>
              </div>
            </Form>
          );
        }}
      </Formik>
    </div>
  );
}
