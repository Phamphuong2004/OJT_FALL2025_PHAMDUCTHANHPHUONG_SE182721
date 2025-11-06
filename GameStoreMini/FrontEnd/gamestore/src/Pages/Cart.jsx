import React from "react";
import { useCart } from "../Cart/CartProvider";
import "../Decorate/Cart.css";
import { useNavigate } from "react-router-dom"; // <-- thêm import

export default function Cart() {
  const {
    items = [],
    updateQty = () => {},
    removeFromCart = () => {},
    clearCart = () => {},
    count = 0,
    loadingMap = {},
  } = useCart();

  const navigate = useNavigate(); // <-- thêm navigate

  const fmtCurrency = (v) => new Intl.NumberFormat("vi-VN").format(v) + " đ";

  return (
    <div className="cart-page">
      <div className="cart-container">
        <h1 className="cart-title">Giỏ hàng</h1>

        {items.length === 0 ? (
          <div className="cart-empty">
            <div className="empty-cart-icon">
              <svg
                width="120"
                height="120"
                viewBox="0 0 24 24"
                fill="none"
                stroke="currentColor"
                strokeWidth="1.5"
                strokeLinecap="round"
                strokeLinejoin="round"
              >
                <circle cx="9" cy="21" r="1" />
                <circle cx="20" cy="21" r="1" />
                <path d="M1 1h4l2.68 13.39a2 2 0 0 0 2 1.61h9.72a2 2 0 0 0 2-1.61L23 6H6" />
                <line x1="10" y1="11" x2="10" y2="11" />
              </svg>
            </div>
            <h2 className="empty-cart-title">Giỏ hàng đang trống</h2>
            <p className="empty-cart-message">
              Vui lòng thêm sản phẩm vào giỏ hàng để tiếp tục mua sắm
            </p>
            <button
              className="btn btn-primary empty-cart-btn"
              onClick={() => navigate("/store")}
            >
              🎮 Khám phá game ngay
            </button>
          </div>
        ) : (
          <div className="cart-grid">
            <div className="cart-list">
              {items.map((item) => {
                const loading = !!loadingMap[item.gameId];
                const title = item.title || item.game?.title || "Untitled";
                const price = Number(item.price || item.game?.price || 0) || 0;
                const img =
                  item.imageUrl ||
                  item.game?.imageUrl ||
                  "/placeholder-game.png";
                const qty = Number(item.qty) || 0;
                const lineTotal = price * qty;
                return (
                  <div className="cart-item" key={item.gameId}>
                    <div className="item-left">
                      <div className="thumb-wrap">
                        <img
                          className="item-thumb"
                          src={img}
                          alt={title}
                          onError={(e) =>
                            (e.currentTarget.src = "/placeholder-game.png")
                          }
                        />
                      </div>
                      <div className="item-meta">
                        <div className="item-title">{title}</div>
                        <div className="item-sub">
                          <div className="item-chip">Game</div>
                          <div style={{ fontWeight: 800 }}>
                            {fmtCurrency(price)}
                          </div>
                        </div>
                      </div>
                    </div>

                    <div className="item-actions">
                      <div className="qty-control">
                        <button
                          onClick={() => updateQty(item.gameId, item.qty - 1)}
                          disabled={
                            loadingMap[item.gameId] ||
                            (Number(item.qty) || 0) <= 1
                          }
                        >
                          -
                        </button>
                        <span className="qty-value">{qty}</span>
                        <button
                          onClick={() =>
                            updateQty(item.gameId, (Number(item.qty) || 0) + 1)
                          }
                          disabled={loadingMap[item.gameId]}
                        >
                          +
                        </button>
                      </div>

                      <div className="line-total">{fmtCurrency(lineTotal)}</div>

                      <button
                        className="remove-btn"
                        disabled={loading}
                        onClick={() => removeFromCart(item.gameId)}
                      >
                        Xóa
                      </button>
                    </div>
                  </div>
                );
              })}
            </div>

            <aside className="cart-summary">
              <div className="summary-card">
                <h2>Tóm tắt đơn hàng</h2>
                <div className="summary-row">
                  <div>Tổng số sản phẩm</div>
                  <div>{count}</div>
                </div>
                <div className="summary-row">
                  <div>Tạm tính</div>
                  <div className="total-amount">
                    {fmtCurrency(
                      items.reduce(
                        (s, it) =>
                          s +
                          (Number(it.price || it.game?.price || 0) || 0) *
                            (Number(it.qty) || 0),
                        0
                      )
                    )}
                  </div>
                </div>
                <div className="summary-row" style={{ marginTop: 8 }}>
                  <div style={{ fontWeight: 800 }}>Tổng cộng</div>
                  <div style={{ fontWeight: 900, fontSize: 18 }}>
                    {fmtCurrency(
                      items.reduce(
                        (s, it) =>
                          s +
                          (Number(it.price || it.game?.price || 0) || 0) *
                            (Number(it.qty) || 0),
                        0
                      )
                    )}
                  </div>
                </div>

                <div className="summary-actions">
                  <button
                    className="btn btn-primary"
                    onClick={() => navigate("/checkout")}
                  >
                    Thanh toán
                  </button>
                  <button className="btn btn-outline" onClick={clearCart}>
                    Xóa hết
                  </button>
                </div>
              </div>
            </aside>
          </div>
        )}

        {/* ...existing summary/footer markup with clearCart / checkout buttons... */}
      </div>
    </div>
  );
}
