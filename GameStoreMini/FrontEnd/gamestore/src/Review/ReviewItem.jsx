import React, { useState } from "react";
import StarRating from "./StarRating";
import ReviewForm from "./ReviewForm";
import { getUserRole } from "../Auth/useAuth";
import "../Decorate/ReviewItem.css";

const ReviewItem = ({
  review,
  userId,
  onUpdate,
  onDelete,
  onToggleHelpful,
  isAuthenticated,
}) => {
  const [isEditing, setIsEditing] = useState(false);
  const [isDeleting, setIsDeleting] = useState(false);
  const [isVoting, setIsVoting] = useState(false);

  const userRole = getUserRole();
  const isAdmin = userRole === "Admin";
  const isOwner = userId && userId === review.userId;
  const canEdit = isOwner;
  const canDelete = isOwner || isAdmin;
  const canVoteHelpful = isAuthenticated && !isOwner;

  const formatDate = (dateString) => {
    const date = new Date(dateString);
    return date.toLocaleDateString("vi-VN", {
      year: "numeric",
      month: "long",
      day: "numeric",
    });
  };

  const handleUpdate = async (data) => {
    try {
      await onUpdate(review.reviewId, data);
      setIsEditing(false);
    } catch (error) {
      console.error("Update failed:", error);
      alert(error.message || "Có lỗi xảy ra khi cập nhật");
    }
  };

  const handleDelete = async () => {
    if (!window.confirm("Bạn có chắc muốn xóa đánh giá này?")) {
      return;
    }

    setIsDeleting(true);
    try {
      await onDelete(review.reviewId);
    } catch (error) {
      console.error("Delete failed:", error);
      alert(error.message || "Có lỗi xảy ra khi xóa");
      setIsDeleting(false);
    }
  };

  const handleToggleHelpful = async () => {
    if (!isAuthenticated) {
      alert("Vui lòng đăng nhập để vote");
      return;
    }

    setIsVoting(true);
    try {
      await onToggleHelpful(review.reviewId);
    } catch (error) {
      console.error("Toggle helpful failed:", error);
      alert(error.message || "Có lỗi xảy ra");
    } finally {
      setIsVoting(false);
    }
  };

  if (isEditing) {
    return (
      <ReviewForm
        gameId={review.gameId}
        initialData={review}
        onSubmit={handleUpdate}
        onCancel={() => setIsEditing(false)}
      />
    );
  }

  return (
    <div className={`review-item ${isDeleting ? "deleting" : ""}`}>
      <div className="review-header">
        <div className="reviewer-info">
          <div className="reviewer-avatar">
            {review.userName?.charAt(0).toUpperCase()}
          </div>
          <div>
            <div className="reviewer-name">
              {review.userName || "Ẩn danh"}
              {review.isVerifiedPurchase && (
                <span className="verified-badge" title="Đã mua sản phẩm">
                  ✓ Đã mua
                </span>
              )}
            </div>
            <div className="review-date">
              {formatDate(review.createdAt)}
              {review.updatedAt && " (đã chỉnh sửa)"}
            </div>
          </div>
        </div>

        {(canEdit || canDelete) && (
          <div className="review-actions">
            {canEdit && (
              <button
                onClick={() => setIsEditing(true)}
                className="btn-icon"
                title="Sửa"
              >
                ✏️
              </button>
            )}
            {canDelete && (
              <button
                onClick={handleDelete}
                className="btn-icon btn-danger"
                title="Xóa"
                disabled={isDeleting}
              >
                🗑️
              </button>
            )}
          </div>
        )}
      </div>

      <div className="review-rating">
        <StarRating rating={review.rating} size={18} showNumber={false} />
      </div>

      {review.comment && <div className="review-comment">{review.comment}</div>}

      <div className="review-footer">
        <button
          onClick={handleToggleHelpful}
          className={`btn-helpful ${
            review.isHelpfulByCurrentUser ? "active" : ""
          }`}
          disabled={!canVoteHelpful || isVoting}
          title={
            !isAuthenticated
              ? "Đăng nhập để vote"
              : isOwner
              ? "Không thể vote cho review của mình"
              : "Đánh dấu hữu ích"
          }
        >
          <span className="helpful-icon">👍</span>
          <span>Hữu ích ({review.helpfulCount})</span>
        </button>
      </div>
    </div>
  );
};

export default ReviewItem;
