import React, { useState, useEffect } from "react";
import { useReviews } from "./useReviews";
import ReviewStats from "./ReviewStats";
import ReviewFilter from "./ReviewFilter";
import ReviewForm from "./ReviewForm";
import ReviewItem from "./ReviewItem";
import Pagination from "../Components/Pagination";
import reviewService from "../API/ReviewAPI";
import "../Decorate/ReviewList.css";

const ReviewList = ({ gameId }) => {
  // Lấy thông tin user từ localStorage
  const [isAuthenticated, setIsAuthenticated] = useState(false);
  const [userId, setUserId] = useState(null);
  const [token, setToken] = useState(null);

  const [canReview, setCanReview] = useState(false);
  const [showReviewForm, setShowReviewForm] = useState(false);
  const [checkingEligibility, setCheckingEligibility] = useState(false);

  // Kiểm tra authentication
  useEffect(() => {
    const storedToken = localStorage.getItem("token");
    const storedUser = localStorage.getItem("user");

    if (storedToken) {
      setIsAuthenticated(true);
      setToken(storedToken);

      if (storedUser) {
        try {
          const user = JSON.parse(storedUser);
          setUserId(user.id || user.userId);
        } catch (err) {
          console.error("Error parsing user:", err);
        }
      }
    }
  }, []);

  const {
    reviews,
    statistics,
    loading,
    error,
    pagination,
    filters,
    createReview,
    updateReview,
    deleteReview,
    toggleHelpful,
    updateFilters,
    goToPage,
  } = useReviews(gameId);

  // Kiểm tra user có thể review không
  const checkReviewEligibility = async () => {
    if (!isAuthenticated) {
      alert("Vui lòng đăng nhập để viết đánh giá");
      return;
    }

    setCheckingEligibility(true);
    try {
      const data = await reviewService.canUserReviewGame(gameId, token);

      if (data.canReview) {
        setCanReview(true);
        setShowReviewForm(true);
      } else {
        alert("Bạn cần mua game này trước khi có thể đánh giá");
      }
    } catch (err) {
      console.error("Error checking eligibility:", err);
      alert("Có lỗi xảy ra khi kiểm tra quyền đánh giá");
    } finally {
      setCheckingEligibility(false);
    }
  };

  const handleCreateReview = async (reviewData) => {
    try {
      await createReview(reviewData);
      setShowReviewForm(false);
      setCanReview(false);
      alert("Đánh giá của bạn đã được gửi thành công!");
    } catch (err) {
      console.error("Error creating review:", err);
      alert(err.message || "Có lỗi xảy ra khi gửi đánh giá");
    }
  };

  // Hiển thị lỗi chi tiết hơn
  if (error) {
    return (
      <div className="review-section">
        <div
          className="error-message"
          style={{
            padding: "20px",
            background: "#fee2e2",
            borderRadius: "8px",
            color: "#dc2626",
          }}
        >
          <h4>⚠️ Không thể tải reviews</h4>
          <p>
            <strong>Lỗi:</strong> {error}
          </p>
          <details style={{ marginTop: "10px" }}>
            <summary style={{ cursor: "pointer" }}>Chi tiết lỗi</summary>
            <pre
              style={{
                marginTop: "10px",
                padding: "10px",
                background: "#fff",
                borderRadius: "4px",
                fontSize: "12px",
                overflow: "auto",
              }}
            >
              {typeof error === "string"
                ? error
                : JSON.stringify(error, null, 2)}
            </pre>
          </details>
          <div style={{ marginTop: "10px" }}>
            <strong>Kiểm tra:</strong>
            <ul style={{ marginTop: "5px", paddingLeft: "20px" }}>
              <li>Backend API có đang chạy không? (http://localhost:5000)</li>
              <li>CORS đã được cấu hình chưa?</li>
              <li>GameId có hợp lệ không? (GameId: {gameId})</li>
            </ul>
          </div>
          <button
            onClick={() => window.location.reload()}
            style={{
              marginTop: "10px",
              padding: "8px 16px",
              background: "#dc2626",
              color: "white",
              border: "none",
              borderRadius: "6px",
              cursor: "pointer",
            }}
          >
            Thử lại
          </button>
        </div>
      </div>
    );
  }

  return (
    <div className="review-list-container">
      <h2 className="review-list-title">Đánh giá từ khách hàng</h2>

      {/* Statistics */}
      {statistics && statistics.totalReviews > 0 && (
        <ReviewStats statistics={statistics} />
      )}

      {/* Write Review Button */}
      {isAuthenticated && !showReviewForm && (
        <div className="write-review-section">
          <button
            onClick={checkReviewEligibility}
            className="btn-write-review"
            disabled={checkingEligibility}
          >
            {checkingEligibility ? "Đang kiểm tra..." : "✍️ Viết đánh giá"}
          </button>
        </div>
      )}

      {/* Review Form */}
      {showReviewForm && (
        <ReviewForm
          gameId={gameId}
          onSubmit={handleCreateReview}
          onCancel={() => {
            setShowReviewForm(false);
            setCanReview(false);
          }}
        />
      )}

      {/* Filter */}
      {reviews && reviews.length > 0 && (
        <ReviewFilter filters={filters} onFilterChange={updateFilters} />
      )}

      {/* Loading */}
      {loading && (
        <div className="loading-state">
          <div className="spinner"></div>
          <p>Đang tải đánh giá...</p>
        </div>
      )}

      {/* Reviews List */}
      {!loading && !error && (
        <>
          {!reviews || reviews.length === 0 ? (
            <div className="empty-state">
              <p>Chưa có đánh giá nào cho sản phẩm này.</p>
              {isAuthenticated && (
                <button
                  onClick={checkReviewEligibility}
                  className="btn-write-review"
                  disabled={checkingEligibility}
                >
                  {checkingEligibility
                    ? "Đang kiểm tra..."
                    : "Hãy là người đầu tiên đánh giá!"}
                </button>
              )}
            </div>
          ) : (
            <>
              <div className="reviews-container">
                {reviews.map((review) => (
                  <ReviewItem
                    key={review.reviewId}
                    review={review}
                    onUpdate={updateReview}
                    onDelete={deleteReview}
                    onToggleHelpful={toggleHelpful}
                  />
                ))}
              </div>

              {/* Pagination */}
              {pagination && pagination.totalPages > 1 && (
                <div className="pagination-container">
                  <Pagination
                    currentPage={pagination.page}
                    totalPages={pagination.totalPages}
                    onPageChange={goToPage}
                  />
                </div>
              )}
            </>
          )}
        </>
      )}
    </div>
  );
};

export default ReviewList;
