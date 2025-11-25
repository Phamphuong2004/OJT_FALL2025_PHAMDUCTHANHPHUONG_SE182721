import React from "react";
import { Link } from "react-router-dom";
import Navbar from "./Navbar";
import "../Decorate/Navbar.css";
import "../App.css";

export default function Header() {
  return (
    <header>
      <Navbar />

      <section className="hero">
        <div className="container">
          <div
            style={{
              display: "flex",
              gap: "2rem",
              alignItems: "center",
              justifyContent: "space-between",
              flexWrap: "wrap",
            }}
          >
            <div style={{ maxWidth: 760 }}>
              <div style={{ display: "inline-block", marginBottom: 12 }}>
                <span
                  style={{
                    display: "inline-flex",
                    alignItems: "center",
                    gap: 8,
                    padding: "6px 12px",
                    borderRadius: 999,
                    background:
                      "linear-gradient(90deg, rgba(43,110,246,0.12), rgba(43,110,246,0.06))",
                    color: "#1e3a8a",
                    fontWeight: 700,
                  }}
                >
                  🔥 Khuyến mãi tuần này
                </span>
              </div>

              <h1 className="title" style={{ marginBottom: 12 }}>
                Giảm đến{" "}
                <span style={{ color: "var(--primary, var(--gs-accent))" }}>
                  70%
                </span>{" "}
                cho game chọn lọc
              </h1>
              <p className="subtitle" style={{ marginBottom: 18 }}>
                Ưu đãi hạn chế — Tìm game theo thể loại, đánh giá và chọn mua
                ngay.
              </p>

              <div
                style={{
                  display: "flex",
                  gap: 12,
                  alignItems: "center",
                  flexWrap: "wrap",
                }}
              >
                <Link to="/promotions" className="btn btn-primary">
                  Xem ưu đãi →
                </Link>

                <Link to="/categories" className="btn btn-outline">
                  Theo thể loại
                </Link>
              </div>
            </div>

            <div
              style={{
                width: 220,
                height: 140,
                borderRadius: 14,
                background:
                  "linear-gradient(180deg, rgba(43,110,246,0.06), rgba(255,122,89,0.04))",
                display: "flex",
                alignItems: "center",
                justifyContent: "center",
              }}
            >
              <div
                style={{
                  width: 68,
                  height: 32,
                  borderRadius: 8,
                  background: "linear-gradient(90deg,#6EE7F7,#60A5FA)",
                  boxShadow: "0 6px 18px rgba(15,23,42,0.06)",
                }}
              />
            </div>
          </div>
        </div>
      </section>
    </header>
  );
}
