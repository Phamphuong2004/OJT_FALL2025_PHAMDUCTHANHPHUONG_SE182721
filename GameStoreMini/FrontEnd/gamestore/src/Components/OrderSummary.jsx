import React from "react";
import formatCurrency from "../Utils/formatCurrency";

export default function OrderSummary({
  items = [],
  count = 0,
  subtotal = 0,
  shipping = 0,
  tax = 0,
  total = 0,
}) {
  const fmt = (v) => formatCurrency(v);

  return (
    <aside className="order-summary">
      <h3>Đơn hàng của bạn</h3>
      <ul className="summary-items">
        {items.map((it) => (
          <li key={it.gameId || it.id} className="summary-item">
            <div>
              <strong>{it.title || it.game?.title}</strong>
              <div style={{ fontSize: 12, color: "#666" }}>
                Số lượng: {it.qty || it.quantity}
              </div>
            </div>
            <div>
              {fmt(
                (it.price || it.game?.price || 0) * (it.qty || it.quantity || 1)
              )}
            </div>
          </li>
        ))}
      </ul>

      <div className="summary-lines">
        <div className="line">
          <span>Tạm tính ({count}):</span>
          <span>{fmt(subtotal)}</span>
        </div>
        <div className="line">
          <span>Phí vận chuyển:</span>
          <span>{fmt(shipping)}</span>
        </div>
        <div className="line">
          <span>VAT:</span>
          <span>{fmt(tax)}</span>
        </div>
        <hr />
        <div className="line total">
          <span>Tổng cộng:</span>
          <span>{fmt(total)}</span>
        </div>
      </div>
    </aside>
  );
}
