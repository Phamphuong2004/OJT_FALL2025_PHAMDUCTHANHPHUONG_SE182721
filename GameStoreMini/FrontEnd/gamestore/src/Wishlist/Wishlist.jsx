import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import wishlistAPI from "../API/WishlistAPI";
import { useCart } from "../Cart/CartProvider";
import { Trash2, ShoppingCart } from "lucide-react";
import ReviewSummary from "../Review/ReviewSummary";
import formatCurrency from "../Utils/formatCurrency";
import { useToast } from "../Components/Toast";
import "../Decorate/Pages.css";

export default function Wishlist() {
  const [wishlist, setWishlist] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const navigate = useNavigate();
  const { addToCart } = useCart();
  const toast = useToast();

  useEffect(() => {
    fetchWishlist();
  }, []);

  const fetchWishlist = async () => {
    try {
      setLoading(true);
      const data = await wishlistAPI.getWishlist();
      setWishlist(data);
    } catch (err) {
      console.error("Error fetching wishlist:", err);
      if (err.response?.status === 401) {
        setError("Vui lòng đăng nhập để xem wishlist");
      } else {
        setError("Không thể tải wishlist");
      }
    } finally {
      setLoading(false);
    }
  };

  const removeFromWishlist = async (gameId) => {
    try {
      await wishlistAPI.removeFromWishlist(gameId);
      setWishlist((prev) => prev.filter((item) => item.gameId !== gameId));
    } catch (err) {
      console.error("Error removing from wishlist:", err);
      try {
        toast.error("Không thể xóa khỏi wishlist");
      } catch {}
    }
  };

  const addToCartAndRemove = async (item) => {
    try {
      await addToCart({
        id: item.gameId,
        qty: 1,
        title: item.gameTitle,
        unitPrice: item.gamePrice,
      });
      await removeFromWishlist(item.gameId);
    } catch (err) {
      console.error("Error adding to cart:", err);
      try {
        toast.error("Không thể thêm vào giỏ hàng");
      } catch {}
    }
  };

  const API_BASE = (import.meta.env.VITE_API_BASE || "").replace(/\/$/, "");
  const PLACEHOLDER = "/placeholder-game.png";

  if (loading) return <div style={{ padding: 20 }}>Đang tải...</div>;

  if (error)
    return (
      <div style={{ padding: 20, color: "red" }}>
        {error}
        <button
          onClick={() => navigate("/login")}
          className="btn"
          style={{ marginLeft: 10 }}
        >
          Đăng nhập
        </button>
      </div>
    );

  if (wishlist.length === 0) {
    return (
      <main className="page-container">
        <div className="page-hero">
          <h1>💖 Danh sách yêu thích</h1>
          <div className="muted">Bạn chưa có game nào trong wishlist</div>
        </div>
        <div className="card" style={{ textAlign: "center", padding: 40 }}>
          <p style={{ marginBottom: 20 }}>
            Thêm các game yêu thích vào wishlist để dễ dàng theo dõi!
          </p>
          <button onClick={() => navigate("/store")} className="btn">
            Khám phá Game Store
          </button>
        </div>
      </main>
    );
  }

  return (
    <main className="page-container">
      <div className="page-hero">
        <h1>💖 Danh sách yêu thích</h1>
        <div className="muted">{wishlist.length} game trong wishlist</div>
      </div>

      <div className="page-grid">
        {wishlist.map((item) => {
          let imgSrc = item.gameImageUrl || "";
          if (imgSrc && !/^https?:\/\//i.test(imgSrc) && API_BASE) {
            imgSrc = API_BASE + (imgSrc.startsWith("/") ? "" : "/") + imgSrc;
          }
          const finalImg = imgSrc || PLACEHOLDER;

          return (
            <article key={item.wishlistId} className="card">
              <div
                style={{
                  height: 160,
                  overflow: "hidden",
                  borderRadius: 8,
                  marginBottom: 8,
                  cursor: "pointer",
                }}
                onClick={() => navigate(`/games/${item.gameId}`)}
              >
                <img
                  src={finalImg}
                  alt={item.gameTitle}
                  style={{
                    width: "100%",
                    height: "100%",
                    objectFit: "cover",
                    display: "block",
                  }}
                  onError={(e) => (e.currentTarget.src = PLACEHOLDER)}
                />
              </div>

              <h3
                onClick={() => navigate(`/games/${item.gameId}`)}
                style={{ cursor: "pointer" }}
              >
                {item.gameTitle}
              </h3>

              <div className="muted small" style={{ minHeight: 36 }}>
                {item.gameDescription?.slice(0, 80)}
              </div>

              <ReviewSummary
                averageRating={item.averageRating || 0}
                totalReviews={item.reviewCount || 0}
              />

              <div
                style={{
                  display: "flex",
                  justifyContent: "space-between",
                  alignItems: "center",
                  marginTop: 12,
                }}
              >
                <div style={{ fontWeight: 800 }}>
                  {formatCurrency(item.gamePrice)}
                </div>
              </div>

              <div style={{ display: "flex", gap: 8, marginTop: 12 }}>
                <button
                  onClick={() => addToCartAndRemove(item)}
                  className="btn"
                  style={{
                    flex: 1,
                    display: "flex",
                    alignItems: "center",
                    justifyContent: "center",
                    gap: 4,
                  }}
                >
                  <ShoppingCart size={16} />
                  Thêm vào giỏ
                </button>
                <button
                  onClick={() => removeFromWishlist(item.gameId)}
                  className="btn ghost"
                  style={{ padding: "0 12px" }}
                  title="Xóa khỏi wishlist"
                >
                  <Trash2 size={16} />
                </button>
              </div>

              <div className="muted small" style={{ marginTop: 8 }}>
                Đã thêm: {new Date(item.addedAt).toLocaleDateString("vi-VN")}
              </div>
            </article>
          );
        })}
      </div>
    </main>
  );
}
