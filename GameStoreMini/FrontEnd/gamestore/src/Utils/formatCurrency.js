export default function formatCurrency(value, options = {}) {
  // Giá trị có thể là number hoặc string; cố gắng chuyển về number nếu có thể
  if (value == null) return "";
  const num = typeof value === "number" ? value : Number(value);
  if (Number.isNaN(num)) return String(value);

  // Mặc định dùng locale Việt Nam và VND. Nếu backend thay đổi đơn vị,
  // có thể truyền { locale, currency } vào options hoặc backend nên trả
  // metadata `currency` cùng với giá.
  const locale = options.locale || "vi-VN";
  const currency = options.currency || "VND";

  return new Intl.NumberFormat(locale, { style: "currency", currency }).format(
    num
  );
}
