import React, { useState } from "react";
import "../Decorate/StarRating.css";

const StarRating = ({
  rating = 0,
  maxStars = 5,
  size = 20,
  interactive = false,
  onChange = null,
  showNumber = true,
}) => {
  const [hoverRating, setHoverRating] = useState(0);
  const displayRating = interactive && hoverRating > 0 ? hoverRating : rating;

  const handleClick = (star) => {
    if (interactive && onChange) {
      onChange(star);
    }
  };

  const handleMouseEnter = (star) => {
    if (interactive) {
      setHoverRating(star);
    }
  };

  const handleMouseLeave = () => {
    if (interactive) {
      setHoverRating(0);
    }
  };

  const renderStar = (index) => {
    const starValue = index + 1;
    const filled = displayRating >= starValue;
    const halfFilled =
      displayRating >= starValue - 0.5 && displayRating < starValue;

    return (
      <span
        key={index}
        onClick={() => handleClick(starValue)}
        onMouseEnter={() => handleMouseEnter(starValue)}
        onMouseLeave={handleMouseLeave}
        className={`star ${interactive ? "interactive" : ""} ${
          filled ? "filled" : ""
        } ${halfFilled ? "half-filled" : ""}`}
        style={{ fontSize: `${size}px` }}
      >
        {halfFilled ? "⯨" : filled ? "★" : "☆"}
      </span>
    );
  };

  return (
    <div className="star-rating">
      <div className="stars">
        {Array.from({ length: maxStars }).map((_, index) => renderStar(index))}
      </div>
      {showNumber && (
        <span className="rating-number">{displayRating.toFixed(1)}</span>
      )}
    </div>
  );
};

export default StarRating;
