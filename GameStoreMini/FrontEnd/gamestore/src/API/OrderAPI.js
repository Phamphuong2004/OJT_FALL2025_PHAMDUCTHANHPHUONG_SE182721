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
  const res = await api.get("/orders");
  return res.data;
}

export async function getOrder(id) {
  const res = await api.get(`/orders/${id}`);
  return res.data;
}

export default { createOrder, getMyOrders, getOrder };
