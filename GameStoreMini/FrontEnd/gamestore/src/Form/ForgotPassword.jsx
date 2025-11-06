import React, { useState } from "react";
import { Formik, Form, Field, ErrorMessage } from "formik";
import * as Yup from "yup";
import { useNavigate, Link } from "react-router-dom";
import "../Decorate/form.css";

const validationSchema = Yup.object({
  identifier: Yup.string()
    .required("Vui lòng nhập email hoặc tên đăng nhập")
    .min(3, "Tối thiểu 3 ký tự")
    .max(50, "Tối đa 50 ký tự")
    .test(
      "is-email-or-username",
      "Email không hợp lệ hoặc tên đăng nhập phải có 3-30 ký tự (chỉ chữ, số, dấu chấm, gạch dưới, gạch ngang)",
      (value) => {
        if (!value) return false;
        // Nếu có @ thì validate như email
        if (value.includes("@")) {
          return /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(value);
        }
        // Nếu không có @ thì validate như username
        return /^[a-zA-Z0-9._-]{3,30}$/.test(value);
      }
    ),
  newPassword: Yup.string()
    .min(8, "Mật khẩu mới phải có ít nhất 8 ký tự")
    .max(100, "Mật khẩu quá dài")
    .matches(
      /(?=.*[A-Za-z])(?=.*\d)/,
      "Mật khẩu phải chứa ít nhất 1 chữ cái và 1 chữ số"
    )
    .matches(
      /^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d@$!%*#?&]{8,}$/,
      "Mật khẩu chỉ được chứa chữ cái, số và các ký tự đặc biệt (@$!%*#?&)"
    )
    .required("Vui lòng nhập mật khẩu mới"),
  confirmNewPassword: Yup.string()
    .oneOf([Yup.ref("newPassword"), null], "Mật khẩu xác nhận không khớp")
    .required("Vui lòng xác nhận mật khẩu mới"),
});

export default function ForgotPassword() {
  const navigate = useNavigate();
  const [message, setMessage] = useState(null);
  const [messageType, setMessageType] = useState(null); // 'success' | 'error'
  const [showPassword, setShowPassword] = useState(false);
  const [showConfirmPassword, setShowConfirmPassword] = useState(false);

  const clearMessage = () => {
    setMessage(null);
    setMessageType(null);
  };

  return (
    <div className="form-shell">
      <h2>Đặt lại mật khẩu</h2>

      <Formik
        initialValues={{
          identifier: "",
          newPassword: "",
          confirmNewPassword: "",
        }}
        validationSchema={validationSchema}
        onSubmit={(values, { setSubmitting }) => {
          setSubmitting(true);
          // simulate reset (replace with real API call later)
          setTimeout(() => {
            setSubmitting(false);
            // show success message then redirect to login
            setMessageType("success");
            setMessage(
              "Đổi mật khẩu thành công. Vui lòng đăng nhập bằng mật khẩu mới."
            );
            setTimeout(() => navigate("/login"), 1200);
          }, 900);
        }}
      >
        {({ isSubmitting }) => (
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
              <label htmlFor="identifier">Email hoặc Tên đăng nhập</label>
              <Field
                name="identifier"
                type="text"
                placeholder="your@email.com hoặc username"
                autoComplete="username"
              />
              <div className="error">
                <ErrorMessage name="identifier" />
              </div>
            </div>

            <div className="field-row">
              <label htmlFor="newPassword">Mật khẩu mới</label>
              <div style={{ display: "flex", gap: 8, alignItems: "center" }}>
                <Field
                  name="newPassword"
                  type={showPassword ? "text" : "password"}
                  placeholder="Nhập mật khẩu mới (ít nhất 8 ký tự)"
                  autoComplete="new-password"
                  style={{ flex: 1 }}
                />
                <button
                  type="button"
                  onClick={() => setShowPassword((s) => !s)}
                  style={{
                    padding: "8px 12px",
                    background:
                      "linear-gradient(135deg, #f1f5f9 0%, #e2e8f0 100%)",
                    border: "2px solid #cbd5e1",
                    borderRadius: "8px",
                    color: "#475569",
                    fontWeight: "500",
                    cursor: "pointer",
                    minWidth: "60px",
                  }}
                >
                  {showPassword ? "Ẩn" : "Hiện"}
                </button>
              </div>
              <div className="helper">
                Mật khẩu phải có ít nhất 8 ký tự, bao gồm chữ cái và số
              </div>
              <div className="error">
                <ErrorMessage name="newPassword" />
              </div>
            </div>

            <div className="field-row">
              <label htmlFor="confirmNewPassword">Xác nhận mật khẩu mới</label>
              <div style={{ display: "flex", gap: 8, alignItems: "center" }}>
                <Field
                  name="confirmNewPassword"
                  type={showConfirmPassword ? "text" : "password"}
                  placeholder="Nhập lại mật khẩu mới"
                  autoComplete="new-password"
                  style={{ flex: 1 }}
                />
                <button
                  type="button"
                  onClick={() => setShowConfirmPassword((s) => !s)}
                  style={{
                    padding: "8px 12px",
                    background:
                      "linear-gradient(135deg, #f1f5f9 0%, #e2e8f0 100%)",
                    border: "2px solid #cbd5e1",
                    borderRadius: "8px",
                    color: "#475569",
                    fontWeight: "500",
                    cursor: "pointer",
                    minWidth: "60px",
                  }}
                >
                  {showConfirmPassword ? "Ẩn" : "Hiện"}
                </button>
              </div>
              <div className="error">
                <ErrorMessage name="confirmNewPassword" />
              </div>
            </div>

            <div className="actions">
              <button className="btn" type="submit" disabled={isSubmitting}>
                {isSubmitting ? "Đang đặt lại..." : "Đặt lại mật khẩu"}
              </button>

              <Link to="/login" className="btn secondary">
                Quay lại đăng nhập
              </Link>
            </div>
          </Form>
        )}
      </Formik>

      <p className="helper" style={{ marginTop: 16, textAlign: "center" }}>
        Đã nhớ mật khẩu?{" "}
        <Link
          to="/login"
          style={{
            color: "#3b82f6",
            textDecoration: "none",
            fontWeight: "600",
          }}
        >
          Đăng nhập ngay
        </Link>
      </p>
    </div>
  );
}
