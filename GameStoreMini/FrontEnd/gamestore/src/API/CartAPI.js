import { api } from "../API/ApiClient";

function makeAnonId() {
  try {
    return ([1e7] + -1e3 + -4e3 + -8e3 + -1e11).replace(/[018]/g, (c) =>
      (
        c ^
        (crypto.getRandomValues(new Uint8Array(1))[0] & (15 >> (c / 4)))
      ).toString(16)
    );
  } catch {
    return "anon-" + Math.random().toString(36).slice(2, 12);
  }
}

// ApiClient already attaches Authorization from localStorage via its request
// interceptor and sets withCredentials=true. We only need to attach the
// anonymous cart id header here.
api.interceptors.request.use(
  (config) => {
    config.headers = config.headers || {};
    let anon = localStorage.getItem("anonCartId");
    if (!anon) {
      anon = makeAnonId();
      localStorage.setItem("anonCartId", anon);
    }
    config.headers["X-Anonymous-Id"] = anon;
    return config;
  },
  (err) => Promise.reject(err)
);

api.interceptors.response.use(
  (r) => r,
  (err) => {
    if (err?.response?.status === 401)
      window.dispatchEvent(new Event("unauthorized"));
    return Promise.reject(err);
  }
);

export async function getCart() {
  const r = await api.get("/cart");
  return r.data;
}

export async function addToCart(gameIdOrObj, quantity = 1) {
  // Accept either (gameId, quantity) or ({ gameId, quantity }) to be
  // tolerant of different call sites in the app.
  const payload =
    typeof gameIdOrObj === "object" && gameIdOrObj !== null
      ? gameIdOrObj
      : { gameId: gameIdOrObj, quantity };

  try {
    try {
      console.debug("CartAPI.addToCart -> payload", payload);
    } catch {}
    const r = await api.post("/cart/add", payload);
    try {
      console.debug("CartAPI.addToCart <- response", r?.data);
    } catch {}
    return r.data;
  } catch (err) {
    try {
      console.error("CartAPI.addToCart error", err?.response ?? err);
    } catch {}
    throw err;
  }
}

export async function removeFromCart(gameId) {
  const r = await api.delete(`/cart/remove/${gameId}`);
  return r.data;
}

export async function clearCart() {
  const r = await api.delete(`/cart/clear`);
  return r.data;
}

export default { getCart, addToCart, removeFromCart, clearCart };
