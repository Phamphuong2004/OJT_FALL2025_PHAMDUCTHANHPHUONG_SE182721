import React from "react";
import "../Decorate/ReviewFilter.css";

const ReviewFilter = ({ filters, onFilterChange }) => {
  const handleSortChange = (sortBy) => {
    onFilterChange({
      sortBy,
      sortOrder:
        filters.sortBy === sortBy && filters.sortOrder === "desc"
          ? "asc"
          : "desc",
    });
  };

  const handleRatingFilter = (minRating) => {
    onFilterChange({
      minRating: filters.minRating === minRating ? null : minRating,
    });
  };

  const handleVerifiedFilter = () => {
    onFilterChange({
      verifiedPurchaseOnly: !filters.verifiedPurchaseOnly,
    });
  };

  return (
    <div className="review-filter">
      <div className="filter-group">
        <span className="filter-label">🔍 Sắp xếp:</span>
        <div className="filter-buttons">
          <button
            className={`filter-btn ${
              filters.sortBy === "date" ? "active" : ""
            }`}
            onClick={() => handleSortChange("date")}
          >
            Mới nhất{" "}
            {filters.sortBy === "date" &&
              (filters.sortOrder === "desc" ? "↓" : "↑")}
          </button>
          <button
            className={`filter-btn ${
              filters.sortBy === "rating" ? "active" : ""
            }`}
            onClick={() => handleSortChange("rating")}
          >
            Đánh giá{" "}
            {filters.sortBy === "rating" &&
              (filters.sortOrder === "desc" ? "↓" : "↑")}
          </button>
          <button
            className={`filter-btn ${
              filters.sortBy === "helpful" ? "active" : ""
            }`}
            onClick={() => handleSortChange("helpful")}
          >
            Hữu ích nhất{" "}
            {filters.sortBy === "helpful" &&
              (filters.sortOrder === "desc" ? "↓" : "↑")}
          </button>
        </div>
      </div>

      <div className="filter-group">
        <span className="filter-label">⭐ Lọc theo sao:</span>
        <div className="filter-buttons">
          {[5, 4, 3, 2, 1].map((rating) => (
            <button
              key={rating}
              className={`filter-btn ${
                filters.minRating === rating ? "active" : ""
              }`}
              onClick={() => handleRatingFilter(rating)}
            >
              {rating} ⭐
            </button>
          ))}
        </div>
      </div>

      <div className="filter-group">
        <label className="filter-checkbox">
          <input
            type="checkbox"
            checked={filters.verifiedPurchaseOnly || false}
            onChange={handleVerifiedFilter}
          />
          <span>Chỉ hiển thị người đã mua</span>
        </label>
      </div>
    </div>
  );
};

export default ReviewFilter;
