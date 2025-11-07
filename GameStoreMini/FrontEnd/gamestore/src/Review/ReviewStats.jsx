import React from "react";
import StarRating from "./StarRating";
import "../Decorate/ReviewStats.css";

const ReviewStats = ({ statistics }) => {
  if (!statistics) return null;

  const { averageRating, totalReviews, ratingDistribution } = statistics;

  const getPercentage = (count) => {
    if (totalReviews === 0) return 0;
    return Math.round((count / totalReviews) * 100);
  };

  return (
    <div className="review-stats">
      <div className="stats-summary">
        <div className="average-rating">
          <h2>{averageRating.toFixed(1)}</h2>
          <StarRating rating={averageRating} size={24} showNumber={false} />
          <p>{totalReviews} đánh giá</p>
        </div>

        <div className="rating-distribution">
          {[5, 4, 3, 2, 1].map((star) => (
            <div key={star} className="rating-bar-row">
              <span className="star-label">{star} ⭐</span>
              <div className="rating-bar">
                <div
                  className="rating-bar-fill"
                  style={{
                    width: `${getPercentage(ratingDistribution[star] || 0)}%`,
                  }}
                />
              </div>
              <span className="rating-count">
                {ratingDistribution[star] || 0}
              </span>
            </div>
          ))}
        </div>
      </div>
    </div>
  );
};

export default ReviewStats;
