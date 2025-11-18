import { api } from "./ApiClient";

export async function createOrder(payload) {
  const token = localStorage.getItem("token");
  console.log("[OrderAPI] createOrder called");
  console.log("[OrderAPI] Token exists:", !!token);
  console.log("[OrderAPI] Token length:", token?.length || 0);
  console.log("[OrderAPI] Payload:", payload);

  if (token) {
    // Authenticated checkout: send shipping info to backend
    console.log(
      "[OrderAPI] Using authenticated checkout: POST /orders/checkout"
    );

    // GỬI shipping info cho backend
    const checkoutPayload = {
      customerEmail: payload.customerEmail,
      shippingAddress: payload.shippingAddress,
      shippingPhone: payload.customerPhone,
      orderNotes: payload.orderNotes || "",
    };

    const res = await api.post("/orders/checkout", checkoutPayload);
    return res.data;
  } else {
    // Guest checkout: send guest info and items
    console.log("[OrderAPI] Using guest checkout: POST /orders/guest-checkout");
    const guestPayload = {
      AnonymousId: localStorage.getItem("anonCartId"),
      CustomerName: payload.customerName || payload.name || "Guest",
      CustomerEmail: payload.customerEmail || payload.email || "",
      Items: payload.items || [],
      Total: payload.total || 0,
      Shipping: {
        FullName: payload.customerName,
        Phone: payload.customerPhone || payload.shippingPhone || "",
        Address: payload.shippingAddress || "",
        City: payload.shippingCity || "",
        State: payload.shippingDistrict || "",
      },
    };
    const res = await api.post("/orders/guest-checkout", guestPayload);
    return res.data;
  }
}

export async function getMyOrders() {
  try {
    console.log("[OrderAPI] getMyOrders called");
    const token = localStorage.getItem("token");
    console.log("[OrderAPI] Token exists:", !!token);

    const res = await api.get("/orders/myorders");
    console.log("[OrderAPI] Orders received:", res.data);
    return res.data;
  } catch (error) {
    console.error("[OrderAPI] Error getting orders:", error);
    console.error("[OrderAPI] Error response:", error.response);
    throw error;
  }
}

export async function getOrder(id) {
  const res = await api.get(`/orders/${id}`);
  return res.data;
}

// ===== ADMIN APIs =====

export async function getAllOrdersAdmin(
  status = null,
  page = 1,
  pageSize = 20
) {
  const params = new URLSearchParams();
  if (status) params.set("status", status);
  params.set("page", page);
  params.set("pageSize", pageSize);

  const res = await api.get(`/orders/admin/all?${params.toString()}`);
  return res.data;
}

export async function getOrderAdmin(id) {
  const res = await api.get(`/orders/admin/${id}`);
  return res.data;
}

export async function updateOrderStatus(id, status, paymentStatus = null) {
  const payload = { status };
  if (paymentStatus) payload.paymentStatus = paymentStatus;

  const res = await api.put(`/orders/admin/${id}/status`, payload);
  return res.data;
}

export async function getOrderStatistics() {
  const res = await api.get("/orders/admin/statistics");
  return res.data;
}

export async function trackOrder({ orderCode, email }) {
  // Đúng với backend: OrderNumber và Email
  const res = await api.post("/orders/track", {
    OrderNumber: orderCode,
    Email: email,
  });
  // Trả về dữ liệu đơn hàng hoặc lỗi
  return res.data;
}

export default {
  createOrder,
  getMyOrders,
  getOrder,
  // Admin APIs
  getAllOrdersAdmin,
  getOrderAdmin,
  updateOrderStatus,
  getOrderStatistics,
  trackOrder,
};
