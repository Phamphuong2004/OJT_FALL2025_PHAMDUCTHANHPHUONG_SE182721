import React, { useState, useEffect } from "react";
import UserAPI from "../API/UserAPI";
import { useNavigate } from "react-router-dom";
import { Formik, Form, Field, ErrorMessage } from "formik";
import * as Yup from "yup";
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
  password: Yup.string()
    .min(6, "Mật khẩu phải có ít nhất 6 ký tự")
    .max(100, "Mật khẩu quá dài")
    .required("Vui lòng nhập mật khẩu"),
});

export default function Login() {
  const navigate = useNavigate();
  const [showPassword, setShowPassword] = useState(false);
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

  return (
    <div className="form-shell">
      <h2>Đăng nhập</h2>

      <Formik
        initialValues={{ identifier: "", password: "" }}
        validationSchema={validationSchema}
        onSubmit={async (values, { setSubmitting }) => {
          setSubmitting(true);
          try {
            const res = await UserAPI.login(values.identifier, values.password);

            // Robust token extraction: server may return { Token } or { token } or res.data
            const extractToken = (r) => {
              if (!r) return null;
              if (typeof r === "string") return r;
              if (r.token) return r.token;
              if (r.Token) return r.Token;
              if (r.data) {
                const d = r.data;
                if (typeof d === "string") return d;
                if (d.token) return d.token;
                if (d.Token) return d.Token;
              }
              return null;
            };

            const token = extractToken(res);
            if (token) {
              UserAPI.setAuthToken(token);
              setMessageType("success");
              setMessage("Đăng nhập thành công. Đang chuyển hướng...");
              console.log("Login successful, token:", token);
              // Wait a bit longer to let side effects complete
              setTimeout(() => {
                window.location.href = "/";
              }, 800);
            } else {
              // No token returned but no error: show generic success
              setMessageType("success");
              setMessage("Đăng nhập thành công. Đang chuyển hướng...");
              console.log("Login successful (no token)");
              setTimeout(() => {
                window.location.href = "/";
              }, 800);
            }
          } catch (err) {
            // Prefer friendly backend validation messages when present
            let friendly = "Đăng nhập thất bại";
            const resp = err?.response?.data;
            if (resp) {
              // ASP.NET validation shape: { errors: { Field: ["msg"] }, title, status }
              if (resp.errors && typeof resp.errors === "object") {
                // flatten messages for display
                const parts = [];
                for (const k of Object.keys(resp.errors)) {
                  const v = resp.errors[k];
                  if (Array.isArray(v)) parts.push(v.join(" "));
                  else parts.push(String(v));
                }
                friendly = parts.join(" \n ");
              } else if (typeof resp === "string") {
                friendly = resp;
              } else if (resp.title) {
                friendly =
                  resp.title +
                  (resp.errors ? ": " + JSON.stringify(resp.errors) : "");
              } else {
                friendly = JSON.stringify(resp);
              }
            } else if (err?.message) {
              friendly = err.message;
            }

            setMessageType("error");
            setMessage(friendly);
            console.error("Login failed", err);
          } finally {
            setSubmitting(false);
          }
        }}
      >
        {({ isSubmitting }) => (
          <Form className="form-grid">
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
              <label htmlFor="password">Mật khẩu</label>
              <div style={{ display: "flex", gap: 8, alignItems: "center" }}>
                <Field
                  name="password"
                  type={showPassword ? "text" : "password"}
                  placeholder="Nhập mật khẩu của bạn"
                  autoComplete="current-password"
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
              <div className="error">
                <ErrorMessage name="password" />
              </div>
            </div>

            <div className="actions">
              <button type="submit" disabled={isSubmitting} className="btn">
                {isSubmitting ? "Đang đăng nhập..." : "Đăng nhập"}
              </button>

              <button
                type="button"
                onClick={() => navigate("/forgot-password")}
                className="btn secondary"
              >
                Quên mật khẩu?
              </button>
            </div>
          </Form>
        )}
      </Formik>

      <hr
        style={{
          margin: "24px 0",
          border: "none",
          borderTop: "1px solid #e2e8f0",
        }}
      />
      <p style={{ textAlign: "center", color: "#64748b", fontSize: "0.95rem" }}>
        Chưa có tài khoản?{" "}
        <a
          href="/register"
          style={{
            color: "#3b82f6",
            textDecoration: "none",
            fontWeight: "600",
          }}
        >
          Đăng ký ngay
        </a>
      </p>
    </div>
  );
}
