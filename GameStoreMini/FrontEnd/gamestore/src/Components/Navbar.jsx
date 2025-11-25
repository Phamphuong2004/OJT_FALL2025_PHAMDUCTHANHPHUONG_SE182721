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
import OrderTrackingButton from "../Order/OrderTrackingButton";
import Clock from "./Clock";

export default function Navbar({ onSearch }) {
  const { count } = useCart(); // get count from context
  const [open, setOpen] = useState(false);
  const [query, setQuery] = useState("");
  const [auth, setAuth] = useState(() => isAuthenticated());
  const [role, setRole] = useState(() => getUserRole());
  const [accountOpen, setAccountOpen] = useState(false);
  const [userDropdownOpen, setUserDropdownOpen] = useState(false);
  const [adminDropdownOpen, setAdminDropdownOpen] = useState(false);

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

  // Close dropdown when clicking outside
  useEffect(() => {
    const handleClickOutside = (event) => {
      if (userDropdownOpen && !event.target.closest(".gs-user-menu")) {
        setUserDropdownOpen(false);
      }
      if (
        adminDropdownOpen &&
        !event.target.closest(".admin-dropdown-wrapper")
      ) {
        setAdminDropdownOpen(false);
      }
    };

    document.addEventListener("mousedown", handleClickOutside);
    return () => {
      document.removeEventListener("mousedown", handleClickOutside);
    };
  }, [userDropdownOpen, adminDropdownOpen]);

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

  function handleSearch() {
    // If a parent provided an onSearch handler, use it.
    if (onSearch) {
      onSearch(query);
      return;
    }

    // Fallback: navigate to a simple search route so the button isn't "dead".
    // This avoids requiring every Navbar consumer to pass `onSearch`.
    const q = (query || "").trim();
    if (q.length > 0) navigate(`/search?q=${encodeURIComponent(q)}`);
    else navigate(`/search`);
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
            {/* Menu dành cho Customer - Ẩn khi Admin login */}
            {getUserRole() !== "Admin" && (
              <>
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
                {/* Nút theo dõi đơn hàng:
                   - Guest (chưa đăng nhập): mở modal tra cứu (OrderTrackingButton)
                   - Customer (đã đăng nhập, không phải Admin): dẫn thẳng tới trang 'Đơn hàng của tôi' (/orders)
                */}
                {getUserRole() !== "Admin" &&
                  (auth ? (
                    <li>
                      <Link
                        to="/orders"
                        className="order-track-link"
                        title="Xem đơn hàng của tôi"
                      >
                        <svg
                          width="18"
                          height="18"
                          viewBox="0 0 24 24"
                          fill="none"
                        >
                          <path
                            d="M3 6h18"
                            stroke="currentColor"
                            strokeWidth="1.5"
                            strokeLinecap="round"
                            strokeLinejoin="round"
                          />
                          <path
                            d="M3 6l1.5 14h15L21 6"
                            stroke="currentColor"
                            strokeWidth="1.5"
                            strokeLinecap="round"
                            strokeLinejoin="round"
                          />
                        </svg>
                        <span className="order-track-label">Đơn hàng</span>
                      </Link>
                    </li>
                  ) : (
                    <li>
                      <OrderTrackingButton />
                    </li>
                  ))}
                <li className="mobile-only">
                  <NavLink to="/contact">Liên hệ</NavLink>
                </li>
              </>
            )}

            {/* Menu dành cho Admin - Link đơn giản đến trang quản trị */}
            {auth && getUserRole() === "Admin" && (
              <li>
                <NavLink to="/admin/system">
                  <i className="fas fa-cog"></i> Quản trị hệ thống
                </NavLink>
              </li>
            )}
          </ul>
        </div>

        <div className="gs-actions">
          {getUserRole() !== "Admin" && (
            <div style={{ marginRight: 8 }}>
              <Clock
                showSeconds={true}
                showTZ={true}
                className="navbar-clock"
              />
            </div>
          )}
          {/* Search box - CHỈ cho Customer */}
          {getUserRole() !== "Admin" && (
            <div className="gs-search">
              <input
                type="text"
                placeholder="Search games..."
                value={query}
                onChange={(e) => setQuery(e.target.value)}
                onKeyDown={(e) => {
                  if (e.key === "Enter") handleSearch();
                }}
              />
              <button
                className="search-btn"
                onClick={() => handleSearch()}
                aria-label="Search"
              >
                🔍
              </button>
            </div>
          )}

          {/* Wishlist Link - CHỈ cho Customer */}
          {auth && getUserRole() !== "Admin" && (
            <Link className="gs-cart" to="/wishlist" title="Yêu thích">
              <svg width="20" height="20" viewBox="0 0 24 24" fill="none">
                <path
                  d="M12 21.35l-1.45-1.32C5.4 15.36 2 12.28 2 8.5 2 5.42 4.42 3 7.5 3c1.74 0 3.41.81 4.5 2.09C13.09 3.81 14.76 3 16.5 3 19.58 3 22 5.42 22 8.5c0 3.78-3.4 6.86-8.55 11.54L12 21.35z"
                  stroke="currentColor"
                  strokeWidth="1.5"
                  fill="none"
                />
              </svg>
            </Link>
          )}

          {/* View History Link - CHỈ cho Customer */}
          {auth && getUserRole() !== "Admin" && (
            <Link className="gs-cart" to="/history" title="Lịch sử xem">
              <svg width="20" height="20" viewBox="0 0 24 24" fill="none">
                <path
                  d="M12 8V12L15 15"
                  stroke="currentColor"
                  strokeWidth="1.5"
                  strokeLinecap="round"
                  strokeLinejoin="round"
                />
                <path
                  d="M3.05 11A9 9 0 1 1 3.05 13"
                  stroke="currentColor"
                  strokeWidth="1.5"
                  strokeLinecap="round"
                  strokeLinejoin="round"
                />
                <path
                  d="M3 4V10H9"
                  stroke="currentColor"
                  strokeWidth="1.5"
                  strokeLinecap="round"
                  strokeLinejoin="round"
                />
              </svg>
            </Link>
          )}

          {/* Cart - CHỈ cho Customer */}
          {getUserRole() !== "Admin" && (
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
          )}

          {/* Nút theo dõi đơn hàng - CHỈ cho Customer */}

          {auth ? (
            <div
              className="gs-user-menu"
              onClick={(e) => {
                e.stopPropagation();
                setUserDropdownOpen(!userDropdownOpen);
              }}
            >
              <div className="gs-user-info">
                <div className="gs-user-avatar">
                  {userInfo?.avatar ? (
                    <img
                      src={userInfo.avatar}
                      alt="Avatar"
                      className="avatar-img"
                    />
                  ) : (
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
                  )}
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
                </div>
                <i className="fas fa-chevron-down dropdown-icon"></i>
              </div>

              {userDropdownOpen && (
                <div className="gs-user-dropdown">
                  <Link to="/profile" className="dropdown-item">
                    <i className="fas fa-user"></i>
                    <span>Thông tin cá nhân</span>
                  </Link>

                  {getUserRole() !== "Admin" && (
                    <>
                      <Link to="/orders" className="dropdown-item">
                        <i className="fas fa-box"></i>
                        <span>Đơn hàng của tôi</span>
                      </Link>

                      <Link to="/wishlist" className="dropdown-item">
                        <i className="fas fa-heart"></i>
                        <span>Danh sách yêu thích</span>
                      </Link>

                      <Link to="/addresses" className="dropdown-item">
                        <i className="fas fa-map-marker-alt"></i>
                        <span>Địa chỉ giao hàng</span>
                      </Link>
                    </>
                  )}

                  {getUserRole() === "Admin" && (
                    <Link to="/admin/system" className="dropdown-item">
                      <i className="fas fa-cog"></i>
                      <span>Quản trị hệ thống</span>
                    </Link>
                  )}

                  <div className="dropdown-divider"></div>

                  <button
                    className="dropdown-item logout-item"
                    onClick={() => {
                      apiLogout();
                      setAuth(false);
                      setRole(null);
                      setUserInfo(null);
                      setUserDropdownOpen(false);
                      navigate("/");
                    }}
                  >
                    <i className="fas fa-sign-out-alt"></i>
                    <span>Đăng xuất</span>
                  </button>
                </div>
              )}
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
