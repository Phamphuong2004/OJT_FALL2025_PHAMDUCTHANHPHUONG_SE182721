import React from "react";
import { Link } from "react-router-dom";

export default function PromotionCard({ p }) {
  return (
    <div className="promotion-card">
      <Link to={`/promotions/${p.slug}`}>
        <img src={p.imageUrl || "/api/placeholder/320/180"} alt={p.title} />
      </Link>
      <div className="promotion-card-body">
        <h3>{p.title}</h3>
        <p className="summary">{p.summary}</p>
        <div className="meta">
          <span>{new Date(p.startDate).toLocaleDateString()}</span>
          <span>→</span>
          <span>{new Date(p.endDate).toLocaleDateString()}</span>
        </div>
        <Link to={`/promotions/${p.slug}`} className="view-link">
          Xem chi tiết
        </Link>
      </div>
    </div>
  );
}
