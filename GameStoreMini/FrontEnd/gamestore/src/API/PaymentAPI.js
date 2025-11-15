import { api } from "./ApiClient";

// Tự động chọn URL dựa trên môi trường
const API_BASE_URL = import.meta.env.PROD
  ? "https://api.dammegame.com/api" // Production
  : "http://localhost:5179/api"; // Development

/**
 * Tạo payment URL cho VNPay
 * @param {string} orderNumber - Mã đơn hàng
 * @returns {Promise<{paymentUrl: string}>}
 */
export async function createPaymentUrl(orderNumber) {
  try {
    console.log("PaymentAPI: Creating payment URL for order:", orderNumber);

    const response = await fetch(
      `${API_BASE_URL}/payments/create-payment-url`,
      {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
          Accept: "application/json",
        },
        body: JSON.stringify({ orderNumber }),
        credentials: "include",
      }
    );

    console.log("PaymentAPI: Response status:", response.status);

    if (!response.ok) {
      const errorText = await response.text();
      console.error("PaymentAPI: Error response:", errorText);
      throw new Error(`Payment API error: ${response.status} - ${errorText}`);
    }

    const data = await response.json();
    console.log("PaymentAPI: Payment URL created:", data);

    return data;
  } catch (error) {
    console.error("PaymentAPI: createPaymentUrl error:", error);
    throw error;
  }
}

/**
 * Xử lý callback từ VNPay
 * @param {Object} params - Query parameters từ VNPay
 */
export function handleVnpayReturn(params) {
  try {
    console.log("PaymentAPI: Handling VNPay return:", params);

    return {
      success: params.success === "true",
      orderNumber: params.orderNumber,
      message: params.message,
      code: params.code,
    };
  } catch (error) {
    console.error("PaymentAPI: handleVnpayReturn error:", error);
    throw error;
  }
}

// Legacy functions (giữ để không break code cũ)
export async function createPayment(payload) {
  const res = await api.post("/payments/create", payload);
  return res.data;
}

export async function confirmPayment(payload) {
  const res = await api.post("/payments/confirm", payload);
  return res.data;
}

export default {
  createPaymentUrl,
  handleVnpayReturn,
  createPayment,
  confirmPayment,
};
