import React, { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { getOrder } from "../API/OrderAPI";
import { useToast } from "../Components/Toast";
import "../Decorate/OrderDetails.css";

export default function OrderDetails() {
  const { id } = useParams();
  const [order, setOrder] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const navigate = useNavigate();
  const toast = useToast();

  useEffect(() => {
    if (!id) return;
    let mounted = true;
    const fetchOrder = async () => {
      setLoading(true);
      setError(null);
      try {
        const data = await getOrder(id);

        // Backend may return { success, order, statusLabel } or the order directly
        const fetchedOrder = data?.order ?? data;
        if (!fetchedOrder) {
          setError("Không tìm thấy đơn hàng.");
          return;
        }

        // normalize status label
        const statusLabel =
          data?.statusLabel ?? fetchedOrder.status ?? fetchedOrder.Status;
        const normalized = { ...fetchedOrder, statusLabel };
        if (mounted) setOrder(normalized);
      } catch (err) {
        const status = err?.response?.status;
        if (status === 401) {
          toast?.error?.("Vui lòng đăng nhập để xem chi tiết đơn hàng.");
          navigate("/login");
          return;
        }

        if (status === 404) {
          setError("Không tìm thấy đơn hàng.");
        } else {
          const message =
            err?.response?.data?.message ??
            err.message ??
            "Đã xảy ra lỗi khi tải đơn hàng.";
          toast?.error?.(message);
          setError(message);
        }
      } finally {
        if (mounted) setLoading(false);
      }
    };

    fetchOrder();
    return () => (mounted = false);
  }, [id, navigate, toast]);

  if (loading) return <div className="od-loading">Loading order...</div>;
  if (error) return <div className="od-error">{error}</div>;
  if (!order) return <div className="od-empty">Không có dữ liệu đơn hàng.</div>;

  const items = order.items ?? order.Items ?? [];
  const orderNumber =
    order.orderNumber ?? order.OrderNumber ?? order.OrderId ?? order.id;
  const createdAt = order.createdAt ?? order.CreatedAt;
  const displayDate = createdAt ? new Date(createdAt).toLocaleString() : "";

  const currency = (v) =>
    typeof v === "number"
      ? v.toLocaleString("vi-VN", { style: "currency", currency: "VND" })
      : v;

  return (
    <div className="order-details">
      <div className="od-card">
        <h2 className="od-title">Chi tiết đơn hàng</h2>

        <div className="od-meta">
          <p>
            <strong>Mã đơn:</strong>{" "}
            <span className="od-code">{orderNumber}</span>
          </p>
          <p>
            <strong>Trạng thái:</strong>{" "}
            <span className="od-status">
              {order.statusLabel ?? order.status ?? order.Status}
            </span>
          </p>
          <p>
            <strong>Ngày đặt:</strong> <span>{displayDate}</span>
          </p>
          <p>
            <strong>Tổng tiền:</strong>{" "}
            <span className="od-total">
              {currency(order.total ?? order.Total ?? 0)}
            </span>
          </p>
        </div>

        <h3 className="od-subtitle">Sản phẩm</h3>

        <div className="table-responsive">
          <table className="order-items-table">
            <thead>
              <tr>
                <th className="col-product">Sản phẩm</th>
                <th className="col-qty">Số lượng</th>
                <th className="col-price">Đơn giá</th>
                <th className="col-sub">Thành tiền</th>
              </tr>
            </thead>
            <tbody>
              {items.length === 0 && (
                <tr>
                  <td colSpan={4} className="no-items">
                    Không có sản phẩm.
                  </td>
                </tr>
              )}
              {items.map((it) => {
                const title =
                  it.game?.title ??
                  it.Game?.title ??
                  it.title ??
                  `#${it.gameId ?? it.GameId ?? it.id}`;
                const qty = it.quantity ?? it.Quantity ?? 0;
                const unit = it.unitPrice ?? it.UnitPrice ?? 0;
                const subtotal = unit * qty;
                return (
                  <tr key={it.id ?? `${it.gameId}-${Math.random()}`}>
                    <td className="product-cell">{title}</td>
                    <td className="qty-cell">{qty}</td>
                    <td className="price-cell">{currency(unit)}</td>
                    <td className="sub-cell">{currency(subtotal)}</td>
                  </tr>
                );
              })}
            </tbody>
          </table>
        </div>

        <div className="od-actions">
          <button className="btn-back" onClick={() => navigate("/orders")}>
            Quay lại Đơn hàng của tôi
          </button>
        </div>
      </div>
    </div>
  );
}
