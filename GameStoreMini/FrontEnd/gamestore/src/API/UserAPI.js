import axios from "axios";
import OrderAPI from "./OrderAPI";
import { API_BASE, api } from "./ApiClient";

// Vite exposes import.meta.env.DEV in development builds; use it to enable
// development-only fallback behavior (refresh-dev) when cookies are unreliable.
const IS_DEV =
  typeof import.meta !== "undefined" && import.meta.env && import.meta.env.DEV;

// Attach JWT token from localStorage (if present)
api.interceptors.request.use((config) => {
  try {
    const token = localStorage.getItem("token");
    if (token)
      config.headers = {
        ...(config.headers || {}),
        Authorization: `Bearer ${token}`,
      };
  } catch (e) {
    // ignore
  }
  // Ensure an anonymous cart id is present and sent with requests
  try {
    config.headers = config.headers || {};
    let anon = localStorage.getItem("anonCartId");
    if (!anon) {
      try {
        anon = crypto.randomUUID();
      } catch (err) {
        anon = "anon-" + Math.random().toString(36).slice(2, 12);
      }
      localStorage.setItem("anonCartId", anon);
    }
    config.headers["X-Anonymous-Id"] = anon;
  } catch (e) {
    // ignore
  }
  return config;
});

// Response interceptor: try to refresh once on 401 then retry the request
let isRefreshing = false;
let failedRefresh = false; // global guard to avoid retry storms when refresh permanently fails
let refreshQueue = [];
let lastRefreshAttempt = 0; // timestamp (ms) of last refresh attempt to throttle retries
let justLoggedIn = false; // flag to prevent redirect immediately after login

function processQueue(error, token = null) {
  refreshQueue.forEach((p) => (error ? p.reject(error) : p.resolve(token)));
  refreshQueue = [];
}

api.interceptors.response.use(
  (res) => res,
  async (error) => {
    const original = error.config;

    // If we just logged in, don't redirect on 401 for 2 seconds
    if (justLoggedIn) {
      return Promise.reject(error);
    }

    // If we've already determined refresh fails, immediately force logout/redirect
    if (failedRefresh) {
      console.error("Refresh failed previously, redirecting to login");
      window.location.href = "/login";
      return Promise.reject(error);
    }

    if (error.response && error.response.status === 401 && !original._retry) {
      // If this request is the refresh endpoint itself or an auth endpoint, don't try to refresh
      const url = original.url || "";
      if (
        url.includes("/auth/refresh") ||
        url.includes("/auth/login") ||
        url.includes("/auth/register")
      ) {
        return Promise.reject(error);
      }

      // If we never logged-in / never expected a refresh cookie, skip refresh attempts
      // This prevents calling /auth/refresh repeatedly when server doesn't set a cookie
      const refreshExpected = localStorage.getItem("refreshExpected");
      if (!refreshExpected) {
        // TẠM THỜI KHÔNG redirect - chỉ log để debug
        console.error("No refresh expected, token expired");
        try {
          setAuthToken(null);
        } catch {}
        // KHÔNG redirect nữa - để user tiếp tục sử dụng
        // window.location.href = "/login";
        return Promise.reject(error);
      }
      // throttle refresh attempts: if we tried within last 5s, avoid starting another
      const now = Date.now();
      if (now - lastRefreshAttempt < 5000) {
        // quickly reject to avoid retry storm
        return Promise.reject(error);
      }
      // mark request as retrying
      original._retry = true;

      if (isRefreshing) {
        // queue the request until refresh completes
        return new Promise((resolve, reject) => {
          refreshQueue.push({ resolve, reject });
        })
          .then((token) => {
            original.headers = original.headers || {};
            original.headers.Authorization = `Bearer ${token}`;
            return api(original);
          })
          .catch((e) => Promise.reject(e));
      }

      isRefreshing = true;
      lastRefreshAttempt = Date.now();
      try {
        // use raw axios to call refresh and bypass this instance's interceptors
        const refreshResp = await axios.post(
          `${API_BASE}/auth/refresh`,
          {},
          { withCredentials: true }
        );
        const newToken =
          refreshResp?.data?.token ||
          refreshResp?.data?.Token ||
          refreshResp?.data;
        if (newToken) {
          setAuthToken(newToken);
          processQueue(null, newToken);
          original.headers = original.headers || {};
          original.headers.Authorization = `Bearer ${newToken}`;
          return api(original);
        }
      } catch (e) {
        // try a development-only fallback: if the backend echoed the refresh
        // token on login (refreshDebug) we can call /auth/refresh-dev with it
        // when cookies are not getting through in local setups.
        try {
          if (IS_DEV) {
            const dbg = localStorage.getItem("refreshDebug");
            if (dbg) {
              try {
                const devResp = await axios.post(
                  `${API_BASE}/auth/refresh-dev`,
                  { Token: dbg },
                  { withCredentials: true }
                );
                const devToken =
                  devResp?.data?.token || devResp?.data?.Token || devResp?.data;
                if (devToken) {
                  setAuthToken(devToken);
                  processQueue(null, devToken);
                  original.headers = original.headers || {};
                  original.headers.Authorization = `Bearer ${devToken}`;
                  return api(original);
                }
              } catch (devErr) {
                // dev fallback failed — fall through to normal failure handling
                console.warn("Dev refresh fallback failed", devErr);
              }
            }
          }
        } catch (fallbackErr) {
          // ignore errors from fallback attempt
          console.warn("Error during dev refresh fallback", fallbackErr);
        }

        // refresh failed — set guard so we don't keep retrying and redirect user to login
        failedRefresh = true;
        processQueue(e, null);
        console.warn("Refresh failed in interceptor", e);
        window.location.href = "/login";
        return Promise.reject(e);
      } finally {
        isRefreshing = false;
      }
    }

    return Promise.reject(error);
  }
);

// Login: returns { token: '...' } (AuthResultDto)
// Accept either an email or a username as the identifier.
export async function login(identifier, password) {
  // Backend now accepts a flexible LoginDto with an 'Identifier' field.
  // Send the identifier in the Identifier property for simplicity.
  const payload = { Identifier: identifier, Password: password };
  const res = await api.post("/auth/login", payload);
  return res.data;
}

// Register: server expects multipart/form-data (RegisterDto from form)
// `values` should be an object with keys: email, userName, fullName, phoneNumber, password, confirmPassword, termsAccepted, avatar (optional File)
export async function register(values) {
  const fd = new FormData();
  if (values.email) fd.append("Email", values.email);
  if (values.userName) fd.append("UserName", values.userName);
  if (values.fullName) fd.append("FullName", values.fullName);
  if (values.phoneNumber) fd.append("PhoneNumber", values.phoneNumber);
  if (values.password) fd.append("Password", values.password);
  if (values.confirmPassword)
    fd.append("ConfirmPassword", values.confirmPassword);
  // TermsAccepted is a boolean — backend expects true/false
  if (typeof values.termsAccepted !== "undefined")
    fd.append("TermsAccepted", values.termsAccepted ? "true" : "false");
  if (values.avatar) fd.append("Avatar", values.avatar);

  const res = await api.post("/auth/register", fd, {
    headers: { "Content-Type": "multipart/form-data" },
  });
  return res.data;
}

export function setAuthToken(token) {
  try {
    if (token) {
      localStorage.setItem("token", token);
      console.log("[UserAPI] Token saved, length:", token.length);
      // Set flag to prevent interceptor redirect for 2 seconds
      justLoggedIn = true;
      setTimeout(() => {
        justLoggedIn = false;
      }, 2000);
      // KHÔNG set refreshExpected nữa vì refresh token không hoạt động đúng
      // localStorage.setItem("refreshExpected", "1");
    } else {
      localStorage.removeItem("token");
      localStorage.removeItem("refreshExpected");
    }
    // Mark that a refresh cookie is expected after a successful login/set-token.
    // Backend sets HttpOnly refresh cookie on login; we cannot read it from JS,
    // so remember that login happened and refresh should be attempted when 401 occurs.
    if (token) localStorage.setItem("refreshExpected", "1");
    else localStorage.removeItem("refreshExpected");
    // Development helper: backend may echo the refresh token in the response
    // body when running in Development mode (RefreshTokenDebug). Persist it so
    // we can use the /auth/refresh-dev endpoint if normal cookie-based refresh
    // is not working in the local environment.
    try {
      const dbg = res?.data?.refreshTokenDebug || res?.data?.RefreshTokenDebug;
      if (dbg) localStorage.setItem("refreshDebug", dbg);
    } catch (e) {
      /* ignore */
    }
    // notify other parts of the app that auth state changed
    try {
      window.dispatchEvent(new Event("authChanged"));
    } catch (e) {
      /* ignore */
    }
    // if we just set a token and an anonymous cart exists, try to merge it into user cart
    try {
      if (token) {
        const anon = localStorage.getItem("anonCartId");
        if (anon) {
          // fire-and-forget merge request; api will attach Authorization from localStorage
          (async () => {
            try {
              // send anonymous id as raw text/plain to match backend MergeCart([FromBody] string anonymousId)
              await api.post(`/cart/merge`, anon, {
                headers: { "Content-Type": "text/plain" },
              });
              // remove anon id after successful merge
              localStorage.removeItem("anonCartId");
            } catch (e) {
              // swallow merge errors; merge is best-effort
              // eslint-disable-next-line no-console
              console.warn("Cart merge failed:", e);
            }
          })();
        }
      }
    } catch (e) {
      /* ignore */
    }
    // After setting token, fetch user profile to get fields like EmailConfirmed/LockoutEnd
    try {
      if (token) {
        (async () => {
          try {
            const res = await api.get(`/users/me`);
            if (res && res.data) {
              localStorage.setItem("userProfile", JSON.stringify(res.data));
              try {
                window.dispatchEvent(new Event("authChanged"));
              } catch {}
            }
          } catch (err) {
            // ignore profile fetch errors
            console.warn("Failed to fetch user profile after login", err);
          }
        })();
      } else {
        localStorage.removeItem("userProfile");
        try {
          window.dispatchEvent(new Event("authChanged"));
        } catch {}
      }
    } catch (e) {}

    // If we just logged in and there is a pending order (saved when token expired),
    // try to submit it automatically and redirect to success page on success.
    try {
      if (token) {
        const pending = localStorage.getItem("pendingOrder");
        if (pending) {
          (async () => {
            try {
              const payload = JSON.parse(pending);
              const res = await OrderAPI.createOrder(payload);
              // clear pending and redirect to success page
              localStorage.removeItem("pendingOrder");
              const orderNumber =
                res?.orderNumber || res?.OrderNumber || res?.order?.orderNumber;
              if (orderNumber) {
                // redirect to order success with query params
                window.location.href = `/order/success?orderNumber=${encodeURIComponent(
                  orderNumber
                )}&email=${encodeURIComponent(
                  payload.CustomerEmail || payload.customerEmail || ""
                )}`;
              }
            } catch (err) {
              // ignore failures for the auto-retry; user can retry manually
              console.warn("Auto-submit pending order failed", err);
            }
          })();
        }
      }
    } catch (e) {
      /* ignore */
      // Clear development debug token when logging out / clearing auth
      try {
        localStorage.removeItem("refreshDebug");
      } catch (e) {}
    }
  } catch (e) {
    // ignore storage errors
  }
}

export function logout() {
  // Call backend to clear the refresh cookie (HttpOnly) and invalidate server token.
  (async () => {
    try {
      await api.post(`/auth/logout`, {}, { withCredentials: true });
    } catch (e) {
      // ignore logout errors
    }
    setAuthToken(null);
  })();
}

// Fetch current authenticated user's profile
export async function getProfile() {
  const res = await api.get(`/users/me`);
  return res.data;
}

// Update current user's profile. `payload` should be an object with writable fields
// e.g. { fullName: 'New Name', phoneNumber: '0123...' }
export async function updateProfile(payload) {
  try {
    const res = await api.put(`/users/profile`, payload);
    // Update localStorage user profile
    if (res.data) {
      const currentProfile = JSON.parse(
        localStorage.getItem("userProfile") || "{}"
      );
      const updatedProfile = { ...currentProfile, ...res.data };
      localStorage.setItem("userProfile", JSON.stringify(updatedProfile));
      window.dispatchEvent(new Event("authChanged"));
    }
    return res.data;
  } catch (err) {
    throw err;
  }
}

export async function changePassword(passwordData) {
  try {
    const res = await api.post(`/users/change-password`, passwordData);
    return res.data;
  } catch (err) {
    throw err;
  }
}

export async function refreshToken() {
  try {
    // backend phải trả { token: '...' } và đọc refresh token từ httpOnly cookie
    const res = await api.post("/auth/refresh");
    const token = res?.data?.token || res?.data?.Token || res?.data;
    if (token) {
      // setAuthToken đã có trong file: lưu token + fetch profile
      setAuthToken(token, res);
      return token;
    }
  } catch (e) {
    console.warn("Refresh token failed", e);
  }
  return null;
}
export default {
  login,
  register,
  setAuthToken,
  logout,
  getProfile,
  updateProfile,
  changePassword,
  refreshToken,
};
