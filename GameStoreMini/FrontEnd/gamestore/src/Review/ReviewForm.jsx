import React, { useState } from "react";
import StarRating from "./StarRating";
import "../Decorate/ReviewForm.css";

const ReviewForm = ({
  gameId,
  initialData = null,
  onSubmit,
  onCancel,
  isLoading = false,
}) => {
  const [rating, setRating] = useState(initialData?.rating || 0);
  const [comment, setComment] = useState(initialData?.comment || "");
  const [error, setError] = useState("");

  const handleSubmit = async (e) => {
    e.preventDefault();

    if (rating === 0) {
      setError("Vui lòng chọn số sao");
      return;
    }

    if (comment.trim().length === 0) {
      setError("Vui lòng viết nhận xét");
      return;
    }

    if (comment.length > 1000) {
      setError("Nhận xét không được quá 1000 ký tự");
      return;
    }

    try {
      await onSubmit({
        gameId,
        rating,
        comment: comment.trim(),
      });

      // Reset form nếu là tạo mới
      if (!initialData) {
        setRating(0);
        setComment("");
      }
      setError("");
    } catch (err) {
      setError(err.message || "Có lỗi xảy ra");
    }
  };

  return (
    <form className="review-form" onSubmit={handleSubmit}>
      <h3>{initialData ? "Sửa đánh giá" : "Viết đánh giá"}</h3>

      {error && <div className="error-message">{error}</div>}

      <div className="form-group">
        <label>Đánh giá của bạn *</label>
        <StarRating
          rating={rating}
          interactive={true}
          onChange={setRating}
          size={32}
          showNumber={false}
        />
      </div>

      <div className="form-group">
        <label>Nhận xét *</label>
        <textarea
          value={comment}
          onChange={(e) => setComment(e.target.value)}
          placeholder="Chia sẻ trải nghiệm của bạn về game này..."
          rows={6}
          maxLength={1000}
          disabled={isLoading}
        />
        <div className="char-count">{comment.length}/1000</div>
      </div>

      <div className="form-actions">
        {onCancel && (
          <button
            type="button"
            onClick={onCancel}
            className="btn-cancel"
            disabled={isLoading}
          >
            Hủy
          </button>
        )}
        <button type="submit" className="btn-submit" disabled={isLoading}>
          {isLoading
            ? "Đang xử lý..."
            : initialData
            ? "Cập nhật"
            : "Gửi đánh giá"}
        </button>
      </div>
    </form>
  );
};

export default ReviewForm;
