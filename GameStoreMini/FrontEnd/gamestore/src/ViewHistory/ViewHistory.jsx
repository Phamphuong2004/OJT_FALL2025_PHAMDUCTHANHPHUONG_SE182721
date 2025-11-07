import React, { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { History, ShoppingCart, Trash2, Star, Clock } from "lucide-react";
import viewHistoryAPI from "../API/ViewHistoryAPI";
import { useCart } from "../Cart/CartProvider";
import Pagination from "../Components/Pagination";
import "../Decorate/ViewHistory.css";

const ViewHistory = () => {
  const navigate = useNavigate();
  const { addToCart } = useCart();
  const [viewHistory, setViewHistory] = useState([]);
  const [loading, setLoading] = useState(true);
  const [removing, setRemoving] = useState({});
  const [pagination, setPagination] = useState({
    page: 1,
    pageSize: 20,
    totalCount: 0,
    totalPages: 1,
  });

  useEffect(() => {
    fetchViewHistory(1);
  }, []);

  const fetchViewHistory = async (page) => {
    try {
      setLoading(true);
      const data = await viewHistoryAPI.getViewHistory(page, 20);
      setViewHistory(data.data || []);
      setPagination({
        page: data.page,
        pageSize: data.pageSize,
        totalCount: data.totalCount,
        totalPages: data.totalPages,
      });
    } catch (error) {
      console.error("Error fetching view history:", error);
      if (error.response?.status === 401) {
        alert("Vui lòng đăng nhập");
        navigate("/login");
      }
    } finally {
      setLoading(false);
    }
  };

  const handleRemove = async (gameId) => {
    if (!window.confirm("Bạn có chắc muốn xóa game này khỏi lịch sử?")) return;

    try {
      setRemoving((prev) => ({ ...prev, [gameId]: true }));
      await viewHistoryAPI.removeFromHistory(gameId);
      setViewHistory((prev) => prev.filter((item) => item.gameId !== gameId));
    } catch (error) {
      console.error("Error removing from history:", error);
      alert("Không thể xóa game");
    } finally {
      setRemoving((prev) => ({ ...prev, [gameId]: false }));
    }
  };

  const handleClearAll = async () => {
    if (!window.confirm("Bạn có chắc muốn xóa toàn bộ lịch sử xem?")) return;

    try {
      await viewHistoryAPI.clearHistory();
      setViewHistory([]);
    } catch (error) {
      console.error("Error clearing history:", error);
      alert("Không thể xóa lịch sử");
    }
  };

  const handleAddToCart = async (item) => {
    try {
      await addToCart({
        id: item.gameId,
        qty: 1,
        title: item.gameTitle,
        unitPrice: item.gamePrice,
      });
      alert("Đã thêm vào giỏ hàng!");
    } catch (error) {
      console.error("Error adding to cart:", error);
      alert("Không thể thêm vào giỏ hàng");
    }
  };

  const goToPage = (page) => {
    fetchViewHistory(page);
  };

  const currencyFormatter = new Intl.NumberFormat("vi-VN", {
    style: "currency",
    currency: "VND",
    maximumFractionDigits: 0,
  });

  const API_BASE = (import.meta.env.VITE_API_BASE || "").replace(/\/$/, "");

  const formatViewTime = (viewedAt, lastViewedAt) => {
    const date = new Date(lastViewedAt || viewedAt);
    const now = new Date();
    const diffMs = now - date;
    const diffMins = Math.floor(diffMs / 60000);
    const diffHours = Math.floor(diffMs / 3600000);
    const diffDays = Math.floor(diffMs / 86400000);

    if (diffMins < 1) return "Vừa xem";
    if (diffMins < 60) return `${diffMins} phút trước`;
    if (diffHours < 24) return `${diffHours} giờ trước`;
    if (diffDays < 7) return `${diffDays} ngày trước`;
    return date.toLocaleDateString("vi-VN");
  };

  if (loading && viewHistory.length === 0) {
    return (
      <div className="view-history-container">
        <div className="loading-state">Đang tải...</div>
      </div>
    );
  }

  return (
    <div className="view-history-container">
      <div className="view-history-header">
        <div>
          <h1>
            <History size={32} stroke="#3b82f6" />
            Lịch sử xem sản phẩm
          </h1>
          <p className="muted">{pagination.totalCount} game đã xem</p>
        </div>
        {viewHistory.length > 0 && (
          <button onClick={handleClearAll} className="btn ghost">
            <Trash2 size={18} />
            Xóa tất cả lịch sử
          </button>
        )}
      </div>

      {viewHistory.length === 0 ? (
        <div className="empty-history card">
          <History size={64} stroke="#d1d5db" />
          <h3>Chưa có lịch sử xem</h3>
          <p>Bạn chưa xem game nào gần đây</p>
          <button onClick={() => navigate("/store")} className="btn primary">
            Khám phá Games
          </button>
        </div>
      ) : (
        <>
          <div className="view-history-grid">
            {viewHistory.map((item) => {
              let imgSrc = item.gameImageUrl || "";
              if (imgSrc && !/^https?:\/\//i.test(imgSrc) && API_BASE) {
                imgSrc =
                  API_BASE + (imgSrc.startsWith("/") ? "" : "/") + imgSrc;
              }

              return (
                <div key={item.viewHistoryId} className="history-card card">
                  <div className="history-image">
                    <img
                      src={imgSrc || "/placeholder-game.png"}
                      alt={item.gameTitle}
                      onError={(e) =>
                        (e.currentTarget.src = "/placeholder-game.png")
                      }
                      onClick={() => navigate(`/games/${item.gameId}`)}
                      style={{ cursor: "pointer" }}
                    />
                    <button
                      onClick={() => handleRemove(item.gameId)}
                      disabled={removing[item.gameId]}
                      className="btn-remove"
                      title="Xóa khỏi lịch sử"
                    >
                      <Trash2 size={16} />
                    </button>
                    <div className="view-badge">
                      <Clock size={14} />
                      <span>
                        {formatViewTime(item.viewedAt, item.lastViewedAt)}
                      </span>
                    </div>
                  </div>

                  <div className="history-content">
                    <h3
                      onClick={() => navigate(`/games/${item.gameId}`)}
                      style={{ cursor: "pointer" }}
                    >
                      {item.gameTitle}
                    </h3>

                    {item.categoryName && (
                      <div className="category-badge">{item.categoryName}</div>
                    )}

                    <p className="description">
                      {item.gameDescription?.slice(0, 100)}
                      {item.gameDescription?.length > 100 && "..."}
                    </p>

                    <div className="rating">
                      <Star size={16} fill="#fbbf24" stroke="#fbbf24" />
                      <span>
                        {item.averageRating > 0
                          ? item.averageRating.toFixed(1)
                          : "N/A"}
                      </span>
                      <span className="muted">
                        ({item.reviewCount} reviews)
                      </span>
                    </div>

                    <div className="history-footer">
                      <div className="price">
                        {currencyFormatter.format(item.gamePrice)}
                      </div>
                      <div className="actions">
                        <button
                          onClick={() => navigate(`/games/${item.gameId}`)}
                          className="btn ghost small"
                        >
                          Chi tiết
                        </button>
                        <button
                          onClick={() => handleAddToCart(item)}
                          className="btn primary small"
                        >
                          <ShoppingCart size={16} />
                          Thêm vào giỏ
                        </button>
                      </div>
                    </div>

                    <div className="view-stats muted small">
                      Đã xem {item.viewCount} lần
                    </div>
                  </div>
                </div>
              );
            })}
          </div>

          {pagination.totalPages > 1 && (
            <Pagination
              currentPage={pagination.page}
              totalPages={pagination.totalPages}
              onPageChange={goToPage}
            />
          )}
        </>
      )}
    </div>
  );
};

export default ViewHistory;
