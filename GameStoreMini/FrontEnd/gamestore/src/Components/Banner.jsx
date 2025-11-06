import React from "react";
import { Link } from "react-router-dom";
import "../Decorate/Banner.css";

export default function Banner() {
  return (
    <section className="banner-section" aria-label="Promotions banner">
      <div className="banner-container">
        <div className="banner-content">
          <div className="banner-badge">Khuyến mãi tuần này</div>

          <h2 className="banner-title">
            Giảm đến <span className="banner-highlight">70%</span> cho game chọn
            lọc
          </h2>

          <p className="banner-description">
            Ưu đãi hạn chế — Tìm game theo thể loại, đánh giá và chọn mua ngay.
          </p>

          <div className="banner-actions">
            <Link
              to="/promotions"
              className="banner-btn-primary"
              aria-label="Xem ưu đãi"
            >
              Xem ưu đãi
            </Link>

            <Link to="/categories" className="banner-btn-secondary">
              📚 Theo thể loại
            </Link>
          </div>
        </div>

        <div className="banner-illustration" aria-hidden="true">
          {/* simple illustrative SVG */}
          <svg
            width="100"
            height="100"
            viewBox="0 0 24 24"
            fill="none"
            xmlns="http://www.w3.org/2000/svg"
          >
            <path
              d="M6 12h12"
              stroke="#0f172a"
              strokeWidth="1.5"
              strokeLinecap="round"
              strokeLinejoin="round"
            />
            <path d="M8 16a2 2 0 1 0 0-4 2 2 0 0 0 0 4z" fill="#06b6d4" />
            <path d="M16 16a2 2 0 1 0 0-4 2 2 0 0 0 0 4z" fill="#6366f1" />
          </svg>
        </div>
      </div>
    </section>
  );
}
