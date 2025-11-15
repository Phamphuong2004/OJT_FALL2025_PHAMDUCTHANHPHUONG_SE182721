export function getToken() {
  try {
    return localStorage.getItem("token");
  } catch (e) {
    return null;
  }
}

// Lightweight JWT payload decoder (no verification) to read claims
export function decodeToken(token) {
  if (!token) return null;
  try {
    const parts = token.split(".");
    if (parts.length < 2) return null;
    const payload = parts[1];
    const json = atob(payload.replace(/-/g, "+").replace(/_/g, "/"));
    return JSON.parse(decodeURIComponent(escape(json)));
  } catch (e) {
    try {
      // fallback simple base64
      const b = atob(token.split(".")[1]);
      return JSON.parse(b);
    } catch (err) {
      return null;
    }
  }
}

export function getUserRole() {
  const token = getToken();
  const data = decodeToken(token);
  if (!data) return null;

  // Thử nhiều format role claim
  const role =
    data.role ||
    data.Role ||
    data["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"] ||
    data["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/role"] ||
    null;

  return role;
}

export function getUserEmail() {
  const token = getToken();
  const data = decodeToken(token);
  if (!data) return null;

  // Thử nhiều format email claim
  const email =
    data.email ||
    data.Email ||
    data[
      "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"
    ] ||
    null;

  return email;
}

export function isAuthenticated() {
  return !!getToken();
}

export default {
  getToken,
  decodeToken,
  getUserRole,
  getUserEmail,
  isAuthenticated,
};
