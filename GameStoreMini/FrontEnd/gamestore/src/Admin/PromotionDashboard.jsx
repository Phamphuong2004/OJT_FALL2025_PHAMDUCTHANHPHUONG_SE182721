import React, { useState, useEffect } from "react";
import { Link, useNavigate } from "react-router-dom";
import AdminPromotionAPI from "../API/AdminPromotionAPI";
import { useToast } from "../Components/Toast";
import { isAuthenticated, getUserRole } from "../Auth/useAuth";
import formatCurrency from "../Utils/formatCurrency";
import "../Decorate/AdminPromotion.css";

export default function PromotionDashboard() {
  const [promotions, setPromotions] = useState([]);
  const [statistics, setStatistics] = useState(null);
  const [loading, setLoading] = useState(true);
  const [currentPage, setCurrentPage] = useState(1);
  const [totalPages, setTotalPages] = useState(1);
  const [searchTerm, setSearchTerm] = useState("");
  const [activeFilter, setActiveFilter] = useState(null);
  const toast = useToast();
  const navigate = useNavigate();

  // Check admin permission
  useEffect(() => {
    if (!isAuthenticated() || getUserRole() !== "Admin") {
      toast.error("Bạn không có quyền truy cập trang này");
      navigate("/login");
      return;
    }
  }, [navigate, toast]);

  // Use shared currency formatter helper

  const formatDate = (dateString) => {
    return new Date(dateString).toLocaleDateString("vi-VN", {
      day: "2-digit",
      month: "2-digit",
      year: "numeric",
      hour: "2-digit",
      minute: "2-digit",
    });
  };

  const getStatusColor = (status) => {
    switch (status) {
      case "ACTIVE":
        return "#10b981";
      case "UPCOMING":
        return "#f59e0b";
      case "EXPIRED":
        return "#ef4444";
      default:
        return "#6b7280";
    }
  };

  const getStatusText = (status) => {
    switch (status) {
      case "ACTIVE":
        return "Đang diễn ra";
      case "UPCOMING":
        return "Sắp diễn ra";
      case "EXPIRED":
        return "Đã hết hạn";
      default:
        return "Không xác định";
    }
  };

  const loadPromotions = async (page = 1, search = "", isActive = null) => {
    try {
      setLoading(true);
      const response = await AdminPromotionAPI.getAllPromotions({
        page,
        limit: 20,
        search,
        isActive,
      });

      setPromotions(response.data);
      setTotalPages(response.totalPages);
      setCurrentPage(page);
    } catch (error) {
      toast.error("Không thể tải danh sách chương trình khuyến mãi");
      console.error("Error loading promotions:", error);
    } finally {
      setLoading(false);
    }
  };

  const loadStatistics = async () => {
    try {
      const stats = await AdminPromotionAPI.getStatistics();
      setStatistics(stats);
    } catch (error) {
      console.error("Error loading statistics:", error);
    }
  };

  useEffect(() => {
    loadPromotions(1, searchTerm, activeFilter);
  }, [searchTerm, activeFilter]);

  useEffect(() => {
    loadStatistics();
  }, []);

  const handleSearch = (e) => {
    setSearchTerm(e.target.value);
    setCurrentPage(1);
  };

  const handleFilterChange = (filter) => {
    setActiveFilter(filter);
    setCurrentPage(1);
  };

  const handleToggleStatus = async (id, currentStatus) => {
    try {
      const result = await AdminPromotionAPI.togglePromotionStatus(id);
      toast.success(result.message);

      // Refresh list
      loadPromotions(currentPage, searchTerm, activeFilter);
      loadStatistics();
    } catch (error) {
      toast.error("Không thể thay đổi trạng thái chương trình khuyến mãi");
    }
  };

  const handleDelete = async (id, title) => {
    if (!window.confirm(`Bạn có chắc chắn muốn xóa chương trình "${title}"?`)) {
      return;
    }

    try {
      await AdminPromotionAPI.deletePromotion(id);
      toast.success("Xóa chương trình khuyến mãi thành công");

      // Refresh list
      loadPromotions(currentPage, searchTerm, activeFilter);
      loadStatistics();
    } catch (error) {
      toast.error("Không thể xóa chương trình khuyến mãi");
    }
  };

  return (
    <div className="admin-promotion-dashboard">
      <div className="dashboard-header">
        <div className="header-content">
          <h1>🎉 Quản Lý Khuyến Mãi</h1>
          <Link to="/admin/promotion/create" className="create-btn">
            + Tạo chương trình mới
          </Link>
        </div>

        {/* Statistics Cards */}
        {statistics && (
          <div className="stats-grid">
            <div className="stat-card">
              <div className="stat-icon">📊</div>
              <div className="stat-content">
                <div className="stat-number">
                  {statistics.overview.totalPromotions}
                </div>
                <div className="stat-label">Tổng chương trình</div>
              </div>
            </div>

            <div className="stat-card active">
              <div className="stat-icon">🔥</div>
              <div className="stat-content">
                <div className="stat-number">
                  {statistics.overview.activePromotions}
                </div>
                <div className="stat-label">Đang diễn ra</div>
              </div>
            </div>

            <div className="stat-card upcoming">
              <div className="stat-icon">⏰</div>
              <div className="stat-content">
                <div className="stat-number">
                  {statistics.overview.upcomingPromotions}
                </div>
                <div className="stat-label">Sắp diễn ra</div>
              </div>
            </div>

            <div className="stat-card featured">
              <div className="stat-icon">⭐</div>
              <div className="stat-content">
                <div className="stat-number">
                  {statistics.overview.featuredPromotions}
                </div>
                <div className="stat-label">Nổi bật</div>
              </div>
            </div>
          </div>
        )}
      </div>

      {/* Filters and Search */}
      <div className="dashboard-controls">
        <div className="search-bar">
          <input
            type="text"
            placeholder="🔍 Tìm kiếm chương trình khuyến mãi..."
            value={searchTerm}
            onChange={handleSearch}
            className="search-input"
          />
        </div>

        <div className="filter-tabs">
          <button
            className={`filter-tab ${activeFilter === null ? "active" : ""}`}
            onClick={() => handleFilterChange(null)}
          >
            Tất cả
          </button>
          <button
            className={`filter-tab ${activeFilter === true ? "active" : ""}`}
            onClick={() => handleFilterChange(true)}
          >
            Đang hoạt động
          </button>
          <button
            className={`filter-tab ${activeFilter === false ? "active" : ""}`}
            onClick={() => handleFilterChange(false)}
          >
            Đã tắt
          </button>
        </div>
      </div>

      {/* Promotions Table */}
      <div className="promotions-table-container">
        {loading ? (
          <div className="loading-state">
            <div className="loading-spinner"></div>
            <p>Đang tải dữ liệu...</p>
          </div>
        ) : promotions.length > 0 ? (
          <table className="promotions-table">
            <thead>
              <tr>
                <th>Hình ảnh</th>
                <th>Thông tin</th>
                <th>Thời gian</th>
                <th>Giảm giá</th>
                <th>Trạng thái</th>
                <th>Games</th>
                <th>Hành động</th>
              </tr>
            </thead>
            <tbody>
              {promotions.map((promotion) => (
                <tr key={promotion.id}>
                  <td>
                    <div className="promotion-image">
                      <img
                        src={promotion.imageUrl || "/api/placeholder/80/60"}
                        alt={promotion.title}
                      />
                    </div>
                  </td>

                  <td>
                    <div className="promotion-info">
                      <h4>{promotion.title}</h4>
                      <p className="promotion-summary">{promotion.summary}</p>
                      <div className="promotion-badges">
                        <span className="event-badge">
                          {promotion.eventType}
                        </span>
                        {promotion.isFeatured && (
                          <span className="featured-badge">Nổi bật</span>
                        )}
                      </div>
                    </div>
                  </td>

                  <td>
                    <div className="promotion-dates">
                      <div>
                        <strong>Bắt đầu:</strong>{" "}
                        {formatDate(promotion.startDate)}
                      </div>
                      <div>
                        <strong>Kết thúc:</strong>{" "}
                        {formatDate(promotion.endDate)}
                      </div>
                    </div>
                  </td>

                  <td>
                    <div className="discount-info">
                      {promotion.promotionType === "PERCENTAGE" ? (
                        <span className="discount-percent">
                          -{promotion.discountPercentage}%
                        </span>
                      ) : promotion.promotionType === "FIXED" ? (
                        <span className="discount-fixed">
                          -{formatCurrency(promotion.fixedDiscountAmount)}
                        </span>
                      ) : (
                        <span className="discount-special">Đặc biệt</span>
                      )}
                    </div>
                  </td>

                  <td>
                    <div className="status-container">
                      <span
                        className="status-badge"
                        style={{
                          backgroundColor: getStatusColor(promotion.status),
                        }}
                      >
                        {getStatusText(promotion.status)}
                      </span>
                      <div className="active-toggle">
                        <label className="toggle-switch">
                          <input
                            type="checkbox"
                            checked={promotion.isActive}
                            onChange={() =>
                              handleToggleStatus(
                                promotion.id,
                                promotion.isActive
                              )
                            }
                          />
                          <span className="toggle-slider"></span>
                        </label>
                      </div>
                    </div>
                  </td>

                  <td>
                    <span className="games-count">
                      {promotion.gamesCount} games
                    </span>
                  </td>

                  <td>
                    <div className="action-buttons">
                      <Link
                        to={`/admin/promotion/${promotion.id}/edit`}
                        className="action-btn edit-btn"
                        title="Chỉnh sửa"
                      >
                        ✏️
                      </Link>
                      <Link
                        to={`/promotions/${promotion.slug}`}
                        className="action-btn view-btn"
                        title="Xem trước"
                        target="_blank"
                      >
                        👁️
                      </Link>
                      <button
                        onClick={() =>
                          handleDelete(promotion.id, promotion.title)
                        }
                        className="action-btn delete-btn"
                        title="Xóa"
                      >
                        🗑️
                      </button>
                    </div>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        ) : (
          <div className="empty-state">
            <h3>📋 Chưa có chương trình khuyến mãi nào</h3>
            <p>Hãy tạo chương trình khuyến mãi đầu tiên của bạn!</p>
            <Link to="/admin/promotion/create" className="create-btn">
              Tạo chương trình mới
            </Link>
          </div>
        )}
      </div>

      {/* Pagination */}
      {totalPages > 1 && (
        <div className="pagination">
          <button
            onClick={() =>
              loadPromotions(currentPage - 1, searchTerm, activeFilter)
            }
            disabled={currentPage === 1}
            className="pagination-btn"
          >
            ← Trước
          </button>

          <span className="pagination-info">
            Trang {currentPage} / {totalPages}
          </span>

          <button
            onClick={() =>
              loadPromotions(currentPage + 1, searchTerm, activeFilter)
            }
            disabled={currentPage === totalPages}
            className="pagination-btn"
          >
            Tiếp →
          </button>
        </div>
      )}
    </div>
  );
}
