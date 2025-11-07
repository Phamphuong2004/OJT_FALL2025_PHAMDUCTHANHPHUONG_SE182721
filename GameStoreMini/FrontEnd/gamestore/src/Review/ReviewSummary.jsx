import React from "react";
import StarRating from "./StarRating";
import "../Decorate/ReviewSummary.css";

/**
 * Component hiển thị tóm tắt rating của game
 * Dùng để embed vào GameCard hoặc GameDetails
 */
const ReviewSummary = ({ averageRating, totalReviews }) => {
  if (!totalReviews || totalReviews === 0) {
    return (
      <div className="review-summary no-reviews">
        <span className="no-reviews-text">Chưa có đánh giá</span>
      </div>
    );
  }

  return (
    <div className="review-summary">
      <StarRating rating={averageRating} size={16} showNumber={false} />
      <span className="review-count">
        {averageRating.toFixed(1)} ({totalReviews}{" "}
        {totalReviews === 1 ? "đánh giá" : "đánh giá"})
      </span>
    </div>
  );
};

export default ReviewSummary;
