import React, {
  createContext,
  useContext,
  useReducer,
  useEffect,
  useMemo,
  useRef,
} from "react";
import CartAPI from "../api/cartApi";
import * as signalR from "@microsoft/signalr";
import { getToken, decodeToken } from "../Auth/useAuth";
import { useToast } from "../Components/Toast";

const CartContext = createContext();
const initialState = { items: [], loadingMap: {} };

function cartReducer(state, action) {
  switch (action.type) {
    case "SET_ITEMS":
      return { ...state, items: action.payload };
    case "ADD_OPTIMISTIC": {
      const items = [...state.items];
      const idx = items.findIndex((i) => i.gameId === action.payload.gameId);
      if (idx === -1) items.push(action.payload);
      else
        items[idx] = {
          ...items[idx],
          qty: items[idx].qty + action.payload.qty,
        };
      return { ...state, items };
    }
    case "UPDATE_QTY": {
      const { gameId, qty } = action.payload;
      const items = state.items.map((it) =>
        it.gameId === gameId ? { ...it, qty } : it
      );
      return { ...state, items };
    }
    case "REMOVE_ITEM": {
      const items = state.items.filter(
        (it) => it.gameId !== action.payload.gameId
      );
      return { ...state, items };
    }
    case "SET_LOADING": {
      const { id, loading } = action.payload;
      const loadingMap = { ...state.loadingMap };
      if (loading) loadingMap[id] = true;
      else delete loadingMap[id];
      return { ...state, loadingMap };
    }
    case "CLEAR":
      return { ...state, items: [] };
    default:
      return state;
  }
}

export function CartProvider({ children }) {
  const [state, dispatch] = useReducer(cartReducer, initialState);
  const toast = useToast();
  const connRef = useRef(null);

  // helper to ensure anon id exists
  const ensureAnonId = () => {
    let anon = localStorage.getItem("anonCartId");
    if (!anon) {
      try {
        anon =
          (crypto && crypto.randomUUID && crypto.randomUUID()) ||
          `anon-${Date.now()}`;
      } catch {
        anon = `anon-${Date.now()}`;
      }
      localStorage.setItem("anonCartId", anon);
    }
    return anon;
  };

  // initial load
  // normalize server items shape to provider shape
  const normalizeItems = (items) => {
    if (!Array.isArray(items)) return [];
    return items.map((i) => {
      // server may return { gameId, quantity, game: { title, price, imageUrl } }
      const game = i.game || i.Game || {};
      const gid = i.gameId ?? i.GameId ?? game?.id ?? i.id;
      const qty = Number(i.quantity ?? i.Quantity ?? i.qty ?? i.Qty ?? 0) || 0;
      const title = game?.title ?? game?.Title ?? i.title ?? i.Name ?? "";
      const price =
        Number(game?.price ?? game?.Price ?? i.price ?? i.unitPrice ?? 0) || 0;
      const imageUrl =
        game?.imageUrl ?? game?.ImageUrl ?? i.imageUrl ?? i.image ?? null;
      return { gameId: gid, qty, title, price, imageUrl };
    });
  };

  // initial load
  useEffect(() => {
    ensureAnonId();
    let mounted = true;
    const load = async () => {
      try {
        const res = await CartAPI.getCart();
        if (!mounted) return;
        if (res?.items)
          dispatch({ type: "SET_ITEMS", payload: normalizeItems(res.items) });
      } catch (e) {
        // ignore
      }
    };
    load();
    return () => {
      mounted = false;
    };
  }, []);

  // expose derived count
  const count = useMemo(
    () => state.items.reduce((s, it) => s + (Number(it.qty) || 0), 0),
    [state.items]
  );

  // addToCart: optimistic add, then sync with server response
  const addToCart = async ({
    gameId,
    id,
    qty = 1,
    quantity,
    title,
    price,
    unitPrice,
    imageUrl,
  }) => {
    // be tolerant of different call sites: they may pass `id` or `gameId`, `qty` or `quantity`, `unitPrice` or `price`
    const gid = gameId ?? id;
    const q = Number(qty ?? quantity ?? 1) || 1;
    const p = price ?? unitPrice ?? 0;
    const optimisticItem = { gameId: gid, qty: q, title, price: p, imageUrl };
    // debug: log what we're about to send
    try {
      console.debug("CartProvider.addToCart -> payload", { gameId, qty });
    } catch {}
    // optimistic update
    dispatch({ type: "ADD_OPTIMISTIC", payload: optimisticItem });

    try {
      const res = await CartAPI.addToCart({ gameId: gid, quantity: q });
      console.debug("CartAPI.addToCart response", res);
      // if server returns items, replace; otherwise keep optimistic
      if (res?.items && res.items.length) {
        dispatch({ type: "SET_ITEMS", payload: normalizeItems(res.items) });
      }
      // provider triggers toast once (avoid duplicate toasts in callers)
      toast.success("Đã thêm vào giỏ hàng");
    } catch (err) {
      console.error("CartProvider.addToCart error", err?.response ?? err);
      // rollback: fetch fresh from server or remove optimistic qty
      toast.error("Thêm vào giỏ hàng thất bại");
      try {
        const r = await CartAPI.getCart();
        if (r?.items)
          dispatch({ type: "SET_ITEMS", payload: normalizeItems(r.items) });
      } catch {}
    }
  };

  // updateQty: set to absolute qty (safe), with optimistic update and rollback
  const updateQty = async (gameId, newQty) => {
    // prevent useless requests and disallow qty < 1 (minimum 1)
    if (newQty < 1) return;
    const prev = state.items.find((i) => i.gameId === gameId);
    const prevItems = state.items;

    dispatch({ type: "SET_LOADING", payload: { id: gameId, loading: true } });
    // optimistic update locally
    if (newQty === 0) dispatch({ type: "REMOVE_ITEM", payload: { gameId } });
    else dispatch({ type: "UPDATE_QTY", payload: { gameId, qty: newQty } });

    try {
      // prefer updateQuantity API if available
      if (CartAPI.updateQuantity) {
        const res = await CartAPI.updateQuantity(gameId, newQty);
        if (res?.items)
          dispatch({ type: "SET_ITEMS", payload: normalizeItems(res.items) });
      } else {
        // fallback: if increasing use addToCart, if decreasing and newQty ===0 use remove
        if (prev) {
          const delta = newQty - (prev.qty || 0);
          if (delta > 0) {
            await CartAPI.addToCart({ gameId, quantity: delta });
          } else if (delta < 0) {
            const remaining = newQty;
            if (remaining <= 0) await CartAPI.removeFromCart(gameId);
            else {
              // no direct API to set qty down, call remove then add remaining - simple fallback:
              await CartAPI.removeFromCart(gameId);
              if (remaining > 0)
                await CartAPI.addToCart({ gameId, quantity: remaining });
            }
          }
          // after server calls, try to sync cart
          const r = await CartAPI.getCart();
          if (r?.items)
            dispatch({ type: "SET_ITEMS", payload: normalizeItems(r.items) });
        } else {
          // item not found locally, just call add
          await CartAPI.addToCart({ gameId, quantity: newQty });
          const r = await CartAPI.getCart();
          if (r?.items)
            dispatch({ type: "SET_ITEMS", payload: normalizeItems(r.items) });
        }
      }
    } catch (e) {
      toast.error("Cập nhật số lượng thất bại");
      // rollback to previous state
      dispatch({ type: "SET_ITEMS", payload: prevItems });
    } finally {
      dispatch({
        type: "SET_LOADING",
        payload: { id: gameId, loading: false },
      });
    }
  };

  // removeFromCart helper
  const removeFromCart = async (gameId) => {
    const prevItems = state.items;
    dispatch({ type: "REMOVE_ITEM", payload: { gameId } });
    dispatch({ type: "SET_LOADING", payload: { id: gameId, loading: true } });
    try {
      const res = await CartAPI.removeFromCart(gameId);
      if (res?.items)
        dispatch({ type: "SET_ITEMS", payload: normalizeItems(res.items) });
      toast.success("Đã xóa khỏi giỏ hàng");
    } catch (e) {
      toast.error("Xóa khỏi giỏ hàng thất bại");
      dispatch({ type: "SET_ITEMS", payload: prevItems });
    } finally {
      dispatch({
        type: "SET_LOADING",
        payload: { id: gameId, loading: false },
      });
    }
  };

  const clearCart = async () => {
    const prevItems = state.items;
    dispatch({ type: "CLEAR" });
    try {
      await CartAPI.clearCart();
      toast.success("Đã xóa hết giỏ hàng");
    } catch {
      toast.error("Xóa giỏ hàng thất bại");
      dispatch({ type: "SET_ITEMS", payload: prevItems });
    }
  };

  // SignalR connection for CartUpdated -> sync
  useEffect(() => {
    let conn = null;
    let joined = null;
    let stopped = false;
    let retryCount = 0;
    let retryTimer = null;

    const buildHubUrl = () => {
      const envHub = import.meta.env.VITE_API_HUB ?? null;
      const apiBase = import.meta.env.VITE_API_BASE ?? null;
      let hubBase = envHub;
      if (!hubBase && apiBase) hubBase = apiBase.replace(/\/api\/?$/i, "");
      if (!hubBase) hubBase = window.location.origin;
      return hubBase.replace(/\/$/, "") + "/hubs/cart";
    };

    const start = async () => {
      try {
        const hubUrl = buildHubUrl();
        // Preflight negotiate to get clearer failure modes (negotiate is a POST)
        try {
          const negotiateUrl = hubUrl.replace(/\/$/, "") + "/negotiate";
          const headers = { "Content-Type": "application/json" };
          const token = getToken();
          if (token) headers["Authorization"] = `Bearer ${token}`;

          const negRes = await fetch(negotiateUrl, { method: "POST", headers });
          if (!negRes.ok) {
            const txt = await negRes.text().catch(() => "(no body)");
            throw new Error(`Negotiate failed ${negRes.status}: ${txt}`);
          }
        } catch (neErr) {
          console.warn("SignalR negotiate preflight failed:", neErr);
          throw neErr; // propagate to outer catch to trigger backoff retry
        }

        conn = new signalR.HubConnectionBuilder()
          .withUrl(hubUrl, {
            accessTokenFactory: () => getToken() || undefined,
          })
          .withAutomaticReconnect()
          .configureLogging(signalR.LogLevel.Information)
          .build();

        conn.on("CartUpdated", async () => {
          try {
            const r = await CartAPI.getCart();
            if (r?.items)
              dispatch({ type: "SET_ITEMS", payload: normalizeItems(r.items) });
          } catch (e) {
            console.warn("CartUpdated handler failed to sync cart:", e);
          }
        });

        conn.onclose((err) => {
          if (err) console.warn("SignalR connection closed with error:", err);
          if (!stopped) {
            // schedule reconnect attempt (automatic reconnect also helps, but we backoff start for initial connect)
            retryCount = Math.min(10, retryCount + 1);
            const backoff = Math.min(30000, 500 * 2 ** retryCount);
            retryTimer = setTimeout(() => start(), backoff);
          }
        });

        await conn.start();
        console.info("SignalR connected to", buildHubUrl());
        connRef.current = conn;
        retryCount = 0;

        const token = getToken();
        const dec = decodeToken(token);
        const userId = dec?.sub ?? dec?.nameid ?? null;
        if (userId) {
          joined = `user:${userId}`;
          try {
            await conn.invoke("JoinGroup", joined);
          } catch (e) {
            console.warn("JoinGroup failed:", e);
          }
        } else {
          const anon = ensureAnonId();
          joined = `anon:${anon}`;
          try {
            await conn.invoke("JoinGroup", joined);
          } catch (e) {
            console.warn("JoinGroup (anon) failed:", e);
          }
        }
      } catch (err) {
        console.warn("SignalR start failed:", err);
        // exponential backoff retry for initial connect
        retryCount = Math.min(10, retryCount + 1);
        const backoff = Math.min(30000, 500 * 2 ** retryCount);
        if (!stopped) retryTimer = setTimeout(() => start(), backoff);
      }
    };

    start();

    return () => {
      stopped = true;
      if (retryTimer) clearTimeout(retryTimer);
      (async () => {
        try {
          if (conn) {
            if (joined) {
              try {
                await conn.invoke("LeaveGroup", joined);
              } catch {}
            }
            try {
              await conn.stop();
            } catch {}
          }
        } catch (e) {
          // swallow
        }
      })();
    };
  }, []);

  return (
    <CartContext.Provider
      value={{
        items: state.items,
        count,
        loadingMap: state.loadingMap,
        addToCart,
        updateQty,
        removeFromCart,
        clearCart,
        syncWithServer: async () => {
          try {
            const r = await CartAPI.getCart();
            if (r?.items)
              dispatch({ type: "SET_ITEMS", payload: normalizeItems(r.items) });
          } catch {}
        },
      }}
    >
      {children}
    </CartContext.Provider>
  );
}

export function useCart() {
  return useContext(CartContext);
}

// ensure stable module shape for Fast Refresh
export default CartProvider;
