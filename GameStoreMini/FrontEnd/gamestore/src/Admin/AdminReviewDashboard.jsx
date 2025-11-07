import React, { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import adminReviewAPI from "../API/AdminReviewAPI";
import Pagination from "../Components/Pagination";
import { Star, Trash2, Search } from "lucide-react";
import "../Decorate/AdminReview.css";

const AdminReviewDashboard = () => {
  const navigate = useNavigate();
  const [reviews, setReviews] = useState([]);
  const [statistics, setStatistics] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [deleting, setDeleting] = useState({});

  const [filters, setFilters] = useState({
    search: "",
    minRating: "",
    verifiedPurchaseOnly: false,
    sortBy: "date",
    sortOrder: "desc",
    page: 1,
    pageSize: 20,
  });

  const [pagination, setPagination] = useState({
    page: 1,
    pageSize: 20,
    totalCount: 0,
    totalPages: 1,
  });

  // Fetch reviews
  useEffect(() => {
    fetchReviews();
  }, [filters]);

  // Fetch statistics
  useEffect(() => {
    fetchStatistics();
  }, []);

  const fetchReviews = async () => {
    try {
      setLoading(true);
      const data = await adminReviewAPI.getAllReviews(filters);
      setReviews(data.data || []);
      setPagination({
        page: data.page,
        pageSize: data.pageSize,
        totalCount: data.totalCount,
        totalPages: data.totalPages,
      });
      setError(null);
    } catch (err) {
      console.error("Error fetching reviews:", err);
      setError("Không thể tải danh sách reviews");
    } finally {
      setLoading(false);
    }
  };

  const fetchStatistics = async () => {
    try {
      const stats = await adminReviewAPI.getStatistics();
      setStatistics(stats);
    } catch (err) {
      console.error("Error fetching statistics:", err);
    }
  };

  const handleDeleteReview = async (reviewId) => {
    const reason = prompt("Lý do xóa review (không bắt buộc):");
    if (reason === null) return; // User cancelled

    const confirmed = window.confirm("Bạn có chắc muốn xóa review này?");
    if (!confirmed) return;

    try {
      setDeleting((prev) => ({ ...prev, [reviewId]: true }));
      await adminReviewAPI.deleteReview(reviewId, reason);
      await fetchReviews();
      await fetchStatistics();
      alert("Đã xóa review thành công!");
    } catch (err) {
      console.error("Error deleting review:", err);
      alert(
        "Lỗi khi xóa review: " + (err.response?.data?.message || err.message)
      );
    } finally {
      setDeleting((prev) => ({ ...prev, [reviewId]: false }));
    }
  };

  const updateFilter = (key, value) => {
    setFilters((prev) => ({ ...prev, [key]: value, page: 1 }));
  };

  const goToPage = (page) => {
    setFilters((prev) => ({ ...prev, page }));
  };

  const renderStars = (rating) => {
    return (
      <div style={{ display: "flex", gap: 2 }}>
        {[...Array(5)].map((_, i) => (
          <Star
            key={i}
            size={16}
            fill={i < rating ? "#fbbf24" : "none"}
            stroke={i < rating ? "#fbbf24" : "#d1d5db"}
          />
        ))}
      </div>
    );
  };

  if (loading && !reviews.length) {
    return (
      <div className="admin-container">
        <div className="loading-state">Đang tải...</div>
      </div>
    );
  }

  return (
    <div className="admin-container">
      <div className="admin-header">
        <h1>Quản lý Reviews</h1>
        <p className="muted">Quản lý tất cả đánh giá từ khách hàng</p>
      </div>

      {/* Statistics */}
      {statistics && (
        <div className="stats-grid">
          <div className="stat-card">
            <div className="stat-icon">📊</div>
            <div>
              <div className="stat-label">Tổng Reviews</div>
              <div className="stat-value">{statistics.totalReviews}</div>
            </div>
          </div>

          <div className="stat-card">
            <div className="stat-icon">⭐</div>
            <div>
              <div className="stat-label">Đánh giá TB</div>
              <div className="stat-value">
                {statistics.averageRating.toFixed(1)}
              </div>
            </div>
          </div>

          <div className="stat-card">
            <div className="stat-icon">✅</div>
            <div>
              <div className="stat-label">Verified Reviews</div>
              <div className="stat-value">
                {statistics.verifiedReviewsCount}{" "}
                <span className="stat-sub">
                  ({statistics.verifiedReviewsPercentage}%)
                </span>
              </div>
            </div>
          </div>

          <div className="stat-card">
            <div className="stat-icon">📈</div>
            <div>
              <div className="stat-label">Top Game</div>
              <div className="stat-value small">
                {statistics.topReviewedGames[0]?.gameTitle || "N/A"}
              </div>
            </div>
          </div>
        </div>
      )}

      {/* Filters */}
      <div className="filters-section card">
        <div className="filters-row">
          <div className="search-box">
            <Search size={20} />
            <input
              type="text"
              placeholder="Tìm theo game, user, nội dung..."
              value={filters.search}
              onChange={(e) => updateFilter("search", e.target.value)}
              className="search-input"
            />
          </div>

          <select
            value={filters.minRating}
            onChange={(e) => updateFilter("minRating", e.target.value)}
            className="filter-select"
          >
            <option value="">Tất cả đánh giá</option>
            <option value="5">⭐ 5 sao</option>
            <option value="4">⭐ 4+ sao</option>
            <option value="3">⭐ 3+ sao</option>
            <option value="2">⭐ 2+ sao</option>
            <option value="1">⭐ 1+ sao</option>
          </select>

          <select
            value={filters.sortBy}
            onChange={(e) => updateFilter("sortBy", e.target.value)}
            className="filter-select"
          >
            <option value="date">Mới nhất</option>
            <option value="rating">Rating</option>
            <option value="helpful">Helpful</option>
            <option value="game">Game</option>
            <option value="user">User</option>
          </select>

          <label className="checkbox-label">
            <input
              type="checkbox"
              checked={filters.verifiedPurchaseOnly}
              onChange={(e) =>
                updateFilter("verifiedPurchaseOnly", e.target.checked)
              }
            />
            Chỉ Verified
          </label>
        </div>
      </div>

      {/* Reviews Table */}
      {error && <div className="error-message">{error}</div>}

      {reviews.length === 0 ? (
        <div className="empty-state card">
          <p>Không tìm thấy review nào</p>
        </div>
      ) : (
        <>
          <div className="table-container card">
            <table className="admin-table">
              <thead>
                <tr>
                  <th>ID</th>
                  <th>Game</th>
                  <th>User</th>
                  <th>Rating</th>
                  <th>Comment</th>
                  <th>Verified</th>
                  <th>Helpful</th>
                  <th>Ngày tạo</th>
                  <th>Actions</th>
                </tr>
              </thead>
              <tbody>
                {reviews.map((review) => (
                  <tr key={review.reviewId}>
                    <td>{review.reviewId}</td>
                    <td>
                      <div className="game-cell">
                        {review.gameImageUrl && (
                          <img
                            src={review.gameImageUrl}
                            alt={review.gameTitle}
                            className="game-thumb"
                          />
                        )}
                        <span className="game-title">{review.gameTitle}</span>
                      </div>
                    </td>
                    <td>
                      <div className="user-cell">
                        <div className="user-name">{review.username}</div>
                        <div className="user-email">{review.userEmail}</div>
                      </div>
                    </td>
                    <td>{renderStars(review.rating)}</td>
                    <td>
                      <div className="comment-cell" title={review.comment}>
                        {review.comment?.slice(0, 60)}
                        {review.comment?.length > 60 && "..."}
                      </div>
                    </td>
                    <td>
                      {review.verifiedPurchase ? (
                        <span className="badge success">✓ Verified</span>
                      ) : (
                        <span className="badge">Regular</span>
                      )}
                    </td>
                    <td>{review.helpfulCount}</td>
                    <td>
                      {new Date(review.createdAt).toLocaleDateString("vi-VN")}
                    </td>
                    <td>
                      <div className="action-buttons">
                        <button
                          onClick={() => handleDeleteReview(review.reviewId)}
                          disabled={deleting[review.reviewId]}
                          className="btn-icon danger"
                          title="Xóa review"
                        >
                          <Trash2 size={16} />
                        </button>
                      </div>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>

          <Pagination
            currentPage={pagination.page}
            totalPages={pagination.totalPages}
            onPageChange={goToPage}
          />
        </>
      )}

      {/* Top Reviewed Games */}
      {statistics && statistics.topReviewedGames.length > 0 && (
        <div className="card" style={{ marginTop: 24 }}>
          <h3>Top 5 Games được review nhiều nhất</h3>
          <div className="top-games-list">
            {statistics.topReviewedGames.map((game, index) => (
              <div key={game.gameId} className="top-game-item">
                <div className="rank">#{index + 1}</div>
                <div className="game-info">
                  <div className="game-name">{game.gameTitle}</div>
                  <div className="game-stats">
                    {game.reviewCount} reviews • ⭐{" "}
                    {game.averageRating.toFixed(1)}
                  </div>
                </div>
              </div>
            ))}
          </div>
        </div>
      )}
    </div>
  );
};

export default AdminReviewDashboard;
