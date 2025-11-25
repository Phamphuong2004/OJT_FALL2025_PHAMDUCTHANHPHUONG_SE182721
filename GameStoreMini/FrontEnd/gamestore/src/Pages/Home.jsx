import React, { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import Hero from "../Components/Hero";
import Footer from "../Components/Footer";
import { getAll as getGames } from "../API/GameAPI";
import PromotionAPI from "../API/PromotionAPI";
import { getAll as getCategories } from "../API/CategoryAPI";

const sampleGames = [
  {
    id: 1,
    title: "Cyber Runner",
    price: "$19.99",
    thumb: "https://picsum.photos/seed/game1/400/240",
  },
  {
    id: 2,
    title: "Mystic Quest",
    price: "$29.99",
    thumb: "https://picsum.photos/seed/game2/400/240",
  },
  {
    id: 3,
    title: "Sky Armada",
    price: "$14.99",
    thumb: "https://picsum.photos/seed/game3/400/240",
  },
  {
    id: 4,
    title: "Dungeon Siege",
    price: "$24.99",
    thumb: "https://picsum.photos/seed/game4/400/240",
  },
];

const sampleCategories = [
  { id: "rpg", name: "RPG" },
  { id: "fps", name: "FPS" },
  { id: "strategy", name: "Strategy" },
  { id: "indie", name: "Indie" },
];

export default function Home() {
  const [featured, setFeatured] = useState([]);
  const [cats, setCats] = useState([]);
  const [promos, setPromos] = useState([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    let mounted = true;

    async function load() {
      setLoading(true);
      try {
        const [gRes, cRes, pRes] = await Promise.all([
          getGames({ page: 1, pageSize: 4 }),
          getCategories(),
          PromotionAPI.getFeatured(3),
        ]);

        if (!mounted) return;

        // normalize responses (API shapes vary)
        setFeatured(gRes?.items || gRes || []);
        // categories may return array or paged object
        setCats(cRes?.items || cRes?.Data || cRes || []);
        // PromotionAPI.getFeatured returns an array
        setPromos(pRes || []);
      } catch (err) {
        console.error("Failed to load home data", err);
      } finally {
        if (mounted) setLoading(false);
      }
    }

    load();
    return () => (mounted = false);
  }, []);

  return (
    <>
      <Hero />

      <main style={{ padding: 20, maxWidth: 1200, margin: "0 auto" }}>
        <section style={{ margin: "28px 0" }}>
          <div
            style={{
              display: "flex",
              justifyContent: "space-between",
              alignItems: "center",
              marginBottom: 12,
            }}
          >
            <div>
              <h2 style={{ margin: 0 }}>Featured Games</h2>
              <p className="muted" style={{ margin: 0 }}>
                Được tuyển chọn bởi đội ngũ của chúng tôi
              </p>
            </div>
            <Link to="/store" className="btn btn-outline">
              Xem tất cả
            </Link>
          </div>

          <div className="store-grid">
            {loading
              ? // simple loading placeholders
                Array.from({ length: 4 }).map((_, i) => (
                  <article key={i} className="card">
                    <div
                      className="thumb"
                      style={{ background: "#eee", minHeight: 140 }}
                    />
                    <h3 style={{ height: 18, background: "#f3f4f6" }} />
                    <p className="muted">&nbsp;</p>
                  </article>
                ))
              : (featured.length ? featured : sampleGames).map((g) => {
                  const id = g.Id ?? g.id ?? g.Id ?? g.Id;
                  const title = g.Title ?? g.title;
                  const priceRaw = g.Price ?? g.price;
                  const price =
                    typeof priceRaw === "number"
                      ? `$${priceRaw.toFixed(2)}`
                      : priceRaw;
                  const thumb = g.ImageUrl ?? g.thumb;

                  return (
                    <article key={id || title} className="card">
                      <div className="thumb">
                        {thumb ? (
                          <img src={thumb} alt={title} />
                        ) : (
                          <div style={{ height: 140, background: "#eee" }} />
                        )}
                      </div>
                      <h3>{title}</h3>
                      <p className="muted">
                        {(g.CategoryNames && g.CategoryNames[0]) ||
                          "Short tagline or genre"}
                      </p>
                      <div
                        style={{
                          display: "flex",
                          justifyContent: "space-between",
                          alignItems: "center",
                          marginTop: 12,
                        }}
                      >
                        <div className="price">{price}</div>
                        <Link to={`/games/${id}`} className="btn btn-outline">
                          Details
                        </Link>
                      </div>
                    </article>
                  );
                })}
          </div>
        </section>

        <section style={{ margin: "36px 0" }}>
          <div
            style={{
              display: "flex",
              justifyContent: "space-between",
              alignItems: "center",
              marginBottom: 12,
            }}
          >
            <div>
              <h2 style={{ margin: 0 }}>Categories</h2>
              <p className="muted" style={{ margin: 0 }}>
                Duyệt theo thể loại
              </p>
            </div>
            <Link to="/categories" className="btn btn-outline">
              Tất cả thể loại
            </Link>
          </div>

          <div
            style={{
              display: "grid",
              gridTemplateColumns: "repeat(4, 1fr)",
              gap: 12,
            }}
          >
            {(cats.length ? cats : sampleCategories).map((c) => (
              <Link
                key={c.id ?? c.Id ?? c.slug ?? c.name}
                to={`/categories#${c.id ?? c.Id ?? c.slug ?? c.name}`}
                className="card"
                style={{
                  display: "flex",
                  alignItems: "center",
                  justifyContent: "center",
                  padding: 18,
                  textAlign: "center",
                }}
              >
                <div>
                  <div
                    style={{
                      width: 56,
                      height: 56,
                      borderRadius: 12,
                      margin: "0 auto 8px",
                      background:
                        "linear-gradient(90deg, var(--primary, var(--gs-accent)), #60a5fa)",
                    }}
                  />
                  <div style={{ fontWeight: 700 }}>{c.name ?? c.Name}</div>
                </div>
              </Link>
            ))}
          </div>
        </section>

        <section style={{ margin: "36px 0" }}>
          <div
            style={{
              display: "flex",
              justifyContent: "space-between",
              alignItems: "center",
              marginBottom: 12,
            }}
          >
            <div>
              <h2 style={{ margin: 0 }}>Promotions</h2>
              <p className="muted" style={{ margin: 0 }}>
                Ưu đãi đang chạy
              </p>
            </div>
            <Link to="/promotions" className="btn btn-outline">
              Xem tất cả
            </Link>
          </div>

          <div
            style={{
              display: "grid",
              gridTemplateColumns: "repeat(3, 1fr)",
              gap: 12,
            }}
          >
            {loading
              ? Array.from({ length: 3 }).map((_, i) => (
                  <div
                    key={i}
                    className="card"
                    style={{ padding: 18, minHeight: 120 }}
                  />
                ))
              : (promos.length
                  ? promos
                  : [
                      {
                        Title: "Khuyến mãi tuần này",
                        Summary: "Ưu đãi có hạn, mua ngay khi thấy thích.",
                      },
                      { Title: "Flash Sale", Summary: "Chỉ trong hôm nay." },
                      {
                        Title: "Bundle",
                        Summary: "Mua theo gói để tiết kiệm hơn.",
                      },
                    ]
                ).map((p, i) => (
                  <div
                    key={p.Id ?? p.id ?? p.Title ?? i}
                    className="card"
                    style={{ padding: 18 }}
                  >
                    <div
                      style={{
                        fontSize: 14,
                        fontWeight: 700,
                        color:
                          i === 1
                            ? "#ef4444"
                            : i === 2
                            ? "#10b981"
                            : "var(--primary, var(--gs-accent))",
                        marginBottom: 8,
                      }}
                    >
                      {p.Title || p.title}
                    </div>
                    <h3 style={{ margin: 0 }}>
                      {p.Summary || p.Summary || p.Slug || ""}
                    </h3>
                    <p className="muted">{p.Summary ?? p.summary ?? ""}</p>
                    {p.Id && (
                      <div style={{ marginTop: 12 }}>
                        <Link
                          to={`/promotions/${p.Id}`}
                          className="btn btn-primary"
                        >
                          Xem
                        </Link>
                      </div>
                    )}
                  </div>
                ))}
          </div>
        </section>
      </main>

      <Footer />
    </>
  );
}
