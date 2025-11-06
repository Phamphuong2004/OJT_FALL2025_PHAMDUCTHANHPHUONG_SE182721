import React, { useEffect, useState } from "react";
import { Link, NavLink, useNavigate } from "react-router-dom";
import {
  getUserRole,
  isAuthenticated,
  getToken,
  decodeToken,
} from "../Auth/useAuth";
import { logout as apiLogout, refreshToken } from "../API/UserAPI";
import "../Decorate/Navbar.css";
import { useCart } from "../Cart/CartProvider";

export default function Navbar({ onSearch }) {
  const { count } = useCart(); // get count from context
  const [open, setOpen] = useState(false);
  const [query, setQuery] = useState("");
  const [auth, setAuth] = useState(() => isAuthenticated());
  const [role, setRole] = useState(() => getUserRole());
  const [accountOpen, setAccountOpen] = useState(false);
  const [userInfo, setUserInfo] = useState(() => {
    // Prefer a full profile fetched from the server (saved after login)
    try {
      const raw = localStorage.getItem("userProfile");
      if (raw) return JSON.parse(raw);
    } catch (e) {
      /* ignore parse errors */
    }

    // Fallback: decode token like before
    const t = getToken();
    const d = decodeToken(t);
    if (!d) return null;

    let fullName =
      d.fullName ||
      d.FullName ||
      d.name ||
      d.Name ||
      d.given_name ||
      d.family_name ||
      `${d.firstName || ""} ${d.lastName || ""}`.trim();

    if (!fullName || fullName.trim() === "") {
      const rawName =
        d.userName || d.UserName || d.email?.split("@")[0] || "User";
      if (rawName.includes("@")) fullName = rawName.split("@")[0];
      else fullName = rawName;
    }

    const normalizedRole =
      d.role ||
      d.Role ||
      d["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"] ||
      null;

    return {
      email: d.email || d.Email,
      name: fullName,
      role: normalizedRole,
    };
  });
  const navigate = useNavigate();

  useEffect(() => {
    const refresh = () => {
      setAuth(isAuthenticated());
      setRole(getUserRole());
      // Prefer server-provided profile if available
      try {
        const raw = localStorage.getItem("userProfile");
        if (raw) {
          setUserInfo(JSON.parse(raw));
          return;
        }
      } catch (e) {
        // ignore
      }

      const t = getToken();
      const d = decodeToken(t);
      if (!d) {
        setUserInfo(null);
        return;
      }

      // Fallback: reconstruct a small userInfo object from token
      let fullName =
        d.fullName ||
        d.FullName ||
        d.name ||
        d.Name ||
        d.given_name ||
        d.family_name ||
        `${d.firstName || ""} ${d.lastName || ""}`.trim();
      if (!fullName || fullName.trim() === "") {
        const rawName =
          d.userName || d.UserName || d.email?.split("@")[0] || "User";
        if (rawName.includes("@")) fullName = rawName.split("@")[0];
        else fullName = rawName;
      }

      const normalizedRole =
        d.role ||
        d.Role ||
        d["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"] ||
        null;

      setUserInfo({
        email: d.email || d.Email,
        name: fullName,
        role: normalizedRole,
      });
    };

    const onUnauthorized = () => {
      navigate("/login");
    };

    window.addEventListener("authChanged", refresh);
    window.addEventListener("storage", refresh);
    window.addEventListener("unauthorized", onUnauthorized);

    return () => {
      window.removeEventListener("authChanged", refresh);
      window.removeEventListener("storage", refresh);
      window.removeEventListener("unauthorized", onUnauthorized);
    };
  }, []);

  async function onLoginClick() {
    if (isAuthenticated()) {
      navigate("/account");
      return;
    }
    const ok = await refreshToken();
    if (ok) {
      navigate("/account");
    } else {
      navigate("/login");
    }
  }

  return (
    <nav className="gs-navbar">
      <div className="gs-container">
        <div className="gs-brand">
          <Link className="gs-logo" to="/">
            ĐAM MÊ GAME
          </Link>

          <button
            className="gs-burger"
            aria-label="Toggle menu"
            onClick={() => setOpen((s) => !s)}
          >
            <span className="burger-line" />
            <span className="burger-line" />
            <span className="burger-line" />
          </button>
        </div>

        <div className={`gs-links ${open ? "open" : ""}`}>
          <ul>
            <li>
              <NavLink to="/" end>
                Trang chủ
              </NavLink>
            </li>
            <li>
              <NavLink to="/store">Games</NavLink>
            </li>
            <li>
              <NavLink to="/categories">Thể loại</NavLink>
            </li>
            <li>
              <NavLink to="/promotions">Khuyến mãi</NavLink>
            </li>
            <li>
              <NavLink to="/about">Về chúng tôi</NavLink>
            </li>
            <li className="mobile-only">
              <NavLink to="/contact">Liên hệ</NavLink>
            </li>
            {/* Admin link uses role state */}
            {auth && role === "Admin" && (
              <li className="admin-menu">
                <div className="admin-trigger">Quản trị ▾</div>
                <ul className="admin-submenu">
                  <li>
                    <NavLink to="/admin/promotion">Quản lý khuyến mãi</NavLink>
                  </li>
                  <li>
                    <NavLink to="/admin/add-game">Quản lý game</NavLink>
                  </li>
                </ul>
              </li>
            )}
          </ul>
        </div>

        <div className="gs-actions">
          <div className="gs-search">
            <input
              type="text"
              placeholder="Search games..."
              value={query}
              onChange={(e) => setQuery(e.target.value)}
              onKeyDown={(e) => {
                if (e.key === "Enter") onSearch && onSearch(query);
              }}
            />
            <button
              className="search-btn"
              onClick={() => onSearch && onSearch(query)}
              aria-label="Search"
            >
              🔍
            </button>
          </div>

          <Link className="gs-cart" to="/cart" aria-label="Cart">
            <svg
              width="20"
              height="20"
              viewBox="0 0 24 24"
              fill="none"
              xmlns="http://www.w3.org/2000/svg"
            >
              <path
                d="M6 6H21L20 12H8L6 6Z"
                stroke="currentColor"
                strokeWidth="1.5"
                strokeLinecap="round"
                strokeLinejoin="round"
              />
              <circle cx="10" cy="19" r="1" fill="currentColor" />
              <circle cx="18" cy="19" r="1" fill="currentColor" />
            </svg>
            {count > 0 && <span className="gs-cart-count">{count}</span>}
          </Link>

          {auth ? (
            <div className="gs-user-menu">
              <div className="gs-user-info">
                <div className="gs-user-avatar">
                  <span className="avatar-text">
                    {userInfo?.name
                      ? userInfo.name
                          .split(" ")
                          .map((n) => n[0])
                          .join("")
                          .toUpperCase()
                          .slice(0, 2)
                      : "U"}
                  </span>
                </div>
                <div className="gs-user-details">
                  <div className="gs-user-greeting">
                    Xin chào,{" "}
                    <span className="gs-user-name" title={userInfo?.name}>
                      {userInfo?.name || "User"}
                    </span>
                  </div>
                  <div className="gs-user-role">
                    {userInfo?.role || "Customer"}
                  </div>
                  {/* Show status warnings if profile indicates issues */}
                  {userInfo?.emailConfirmed === false && (
                    <div className="gs-user-status warn">
                      Vui lòng xác nhận email
                    </div>
                  )}
                  {userInfo?.lockoutEnd &&
                    new Date(userInfo.lockoutEnd) > new Date() && (
                      <div className="gs-user-status warn">
                        Tài khoản đã bị khoá
                      </div>
                    )}
                </div>
              </div>
              <button
                className="gs-logout-btn"
                onClick={() => {
                  apiLogout();
                  setAuth(false);
                  setRole(null);
                  setUserInfo(null);
                  navigate("/");
                }}
                aria-label="Đăng xuất"
                title="Đăng xuất"
              >
                <svg
                  width="18"
                  height="18"
                  viewBox="0 0 24 24"
                  fill="none"
                  xmlns="http://www.w3.org/2000/svg"
                >
                  <path
                    d="M9 21H5C4.46957 21 3.96086 20.7893 3.58579 20.4142C3.21071 20.0391 3 19.5304 3 19V5C3 4.46957 3.21071 3.96086 3.58579 3.58579C3.96086 3.21071 4.46957 3 5 3H9"
                    stroke="currentColor"
                    strokeWidth="2"
                    strokeLinecap="round"
                    strokeLinejoin="round"
                  />
                  <path
                    d="M16 17L21 12L16 7"
                    stroke="currentColor"
                    strokeWidth="2"
                    strokeLinecap="round"
                    strokeLinejoin="round"
                  />
                  <path
                    d="M21 12H9"
                    stroke="currentColor"
                    strokeWidth="2"
                    strokeLinecap="round"
                    strokeLinejoin="round"
                  />
                </svg>
              </button>
            </div>
          ) : (
            <div className="gs-avatar-dropdown" tabIndex={0}>
              <button
                className="gs-avatar"
                aria-haspopup="true"
                aria-expanded={accountOpen}
                onClick={() => setAccountOpen((s) => !s)}
                onBlur={() => {
                  // small delay so click on menu items registers before close
                  setTimeout(() => setAccountOpen(false), 150);
                }}
                title="Tài khoản"
              >
                <svg
                  width="28"
                  height="28"
                  viewBox="0 0 24 24"
                  fill="none"
                  xmlns="http://www.w3.org/2000/svg"
                >
                  <path
                    d="M20 21V19C20 16.7909 18.2091 15 16 15H8C5.79086 15 4 16.7909 4 19V21"
                    stroke="currentColor"
                    strokeWidth="1.5"
                    strokeLinecap="round"
                    strokeLinejoin="round"
                  />
                  <circle
                    cx="12"
                    cy="8"
                    r="3"
                    stroke="currentColor"
                    strokeWidth="1.5"
                    strokeLinecap="round"
                    strokeLinejoin="round"
                  />
                </svg>
              </button>

              {accountOpen && (
                <div className="gs-account-menu" role="menu">
                  <button
                    className="gs-account-menu-item"
                    role="menuitem"
                    onClick={(e) => {
                      e.preventDefault();
                      onLoginClick();
                      setAccountOpen(false);
                    }}
                  >
                    Đăng nhập
                  </button>
                  <Link
                    to="/register"
                    className="gs-account-menu-item"
                    role="menuitem"
                    onClick={() => setAccountOpen(false)}
                  >
                    Đăng ký
                  </Link>
                </div>
              )}
            </div>
          )}
        </div>
      </div>
    </nav>
  );
}
