import { api } from "./ApiClient";

export async function createPayment(payload) {
  const res = await api.post("/payments/create", payload);
  return res.data;
}

export async function confirmPayment(payload) {
  const res = await api.post("/payments/confirm", payload);
  return res.data;
}

export default { createPayment, confirmPayment };
