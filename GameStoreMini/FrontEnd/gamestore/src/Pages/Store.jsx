import React, { useEffect, useState, useMemo } from "react";
import { useSearchParams, useNavigate } from "react-router-dom";
import GameAPI from "../API/GameAPI";
import { useCart } from "../Cart/CartProvider";
import { useToast } from "../Components/Toast";
import Pagination from "../Components/Pagination";
import ReviewSummary from "../Review/ReviewSummary";
import WishlistButton from "../Wishlist/WishlistButton";
import "../Decorate/Pages.css";

export default function Store() {
  const [games, setGames] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [totalCount, setTotalCount] = useState(0);
  const [serverPaged, setServerPaged] = useState(false);

  // Pagination state (declare early because fetch useEffect references them)
  const [currentPage, setCurrentPage] = useState(1);
  const pageSize = 9; // change to show more/less items per page

  const [searchParams, setSearchParams] = useSearchParams();
  const navigate = useNavigate();
  const categoryId = searchParams.get("categoryId") || "";
  const q = searchParams.get("q") || "";
  const minRating = searchParams.get("minRating") || "";
  const sortBy = searchParams.get("sortBy") || "createdAt";
  const sortOrder = searchParams.get("sortOrder") || "desc";

  useEffect(() => {
    let mounted = true;

    const fetchPage = async () => {
      setLoading(true);
      setError(null);
      try {
        const params = {
          page: currentPage,
          pageSize,
          sortBy,
          sortOrder,
        };
        if (categoryId) params.categoryId = categoryId;
        if (q) params.q = q;
        if (minRating) params.minRating = minRating;

        const res = await GameAPI.getAll(params);

        // Normalize response shapes
        if (Array.isArray(res)) {
          // backend returned full array (no server pagination)
          if (mounted) {
            setServerPaged(false);
            setGames(res);
            setTotalCount(res.length);
          }
        } else {
          // try common paged shapes: { items: [...], total: 48 } or { data: [...], totalCount: 48 }
          const items = Array.isArray(res.items)
            ? res.items
            : Array.isArray(res.data)
            ? res.data
            : Array.isArray(res.results)
            ? res.results
            : null;

          const total =
            res.total ??
            res.totalCount ??
            res.totalItems ??
            res.count ??
            res.meta?.total ??
            null;

          if (items) {
            // server-side paged
            if (mounted) {
              setServerPaged(true);
              setGames(items);
              setTotalCount(typeof total === "number" ? total : items.length);
            }
          } else {
            // fallback: try to find any array inside response
            const anyArray = Object.values(res).find((v) => Array.isArray(v));
            if (anyArray) {
              if (mounted) {
                setServerPaged(false);
                setGames(anyArray);
                setTotalCount(anyArray.length);
              }
            } else {
              // unknown shape — try to treat as single item object
              if (mounted) {
                setServerPaged(false);
                setGames([]);
                setTotalCount(0);
              }
            }
          }
        }
      } catch (err) {
        console.error("[Store] API error:", err);
        if (mounted) setError(err);
      } finally {
        if (mounted) setLoading(false);
      }
    };

    fetchPage();

    return () => {
      mounted = false;
    };
  }, [categoryId, q, minRating, sortBy, sortOrder, currentPage]);

  const [categories, setCategories] = useState([]);
  const [addingMap, setAddingMap] = useState({}); // track adding per game id
  const { addToCart: addToCartCtx } = useCart();
  const toast = useToast();

  // Helper function to update query params
  const updateFilter = (key, value) => {
    const newParams = new URLSearchParams(searchParams);
    if (value) {
      newParams.set(key, value);
    } else {
      newParams.delete(key);
    }
    setSearchParams(newParams);
  };

  // Pagination
  const totalPages = Math.max(1, Math.ceil((totalCount || 0) / pageSize));

  // currentGames depends on whether backend is server-paged or we have full list
  const currentGames = useMemo(() => {
    if (serverPaged) {
      // games already contains the items for current page
      return games || [];
    }
    // client-side paging from full list
    const start = (currentPage - 1) * pageSize;
    return (games || []).slice(start, start + pageSize);
  }, [games, currentPage, pageSize, serverPaged]);

  useEffect(() => {
    if (currentPage > totalPages) setCurrentPage(1);
  }, [totalPages]);

  // Reset to first page when filters/search change
  useEffect(() => {
    setCurrentPage(1);
  }, [categoryId, q, minRating, sortBy]);

  const currencyFormatter = new Intl.NumberFormat("vi-VN", {
    style: "currency",
    currency: "VND",
    maximumFractionDigits: 0,
  });

  // base URL để prefix nếu API trả đường dẫn tương đối (set VITE_API_BASE=https://localhost:7154)
  const API_BASE = (import.meta.env.VITE_API_BASE || "").replace(/\/$/, "");
  const PLACEHOLDER = "/placeholder-game.png"; // fallback placeholder in public folder

  useEffect(() => {
    (async () => {
      try {
        const c = await GameAPI.getCategories();
        const list = Array.isArray(c) ? c : c?.items ?? c?.data ?? [];
        setCategories(list);
      } catch (e) {
        // ignore
      }
    })();
  }, []);

  if (loading) return <div style={{ padding: 20 }}>Loading...</div>;
  if (error)
    return (
      <div style={{ padding: 20, color: "red" }}>
        Lỗi tải game: {error.message ?? String(error)}
      </div>
    );

  if (!loading && (totalCount === 0 || totalCount == null))
    return <div style={{ padding: 20 }}>Không tìm thấy game.</div>;

  return (
    <main className="page-container">
      <div className="page-hero">
        <div>
          <h1>Game Store</h1>
          <div className="muted">Tìm và mua những trò chơi hay nhất</div>
        </div>

        <div
          style={{
            display: "flex",
            gap: 12,
            alignItems: "center",
            flexWrap: "wrap",
          }}
        >
          {/* Rating Filter */}
          <div style={{ display: "flex", flexDirection: "column", gap: 4 }}>
            <label style={{ fontSize: 12, color: "#6b7280", fontWeight: 500 }}>
              Đánh giá
            </label>
            <select
              value={minRating}
              onChange={(e) => updateFilter("minRating", e.target.value)}
              style={{
                padding: "8px 32px 8px 12px",
                border: "1px solid #e5e7eb",
                borderRadius: 8,
                fontSize: 14,
                backgroundColor: "white",
                cursor: "pointer",
                outline: "none",
                minWidth: 160,
              }}
            >
              <option value="">Tất cả đánh giá</option>
              <option value="4.5">⭐ 4.5+ sao</option>
              <option value="4">⭐ 4+ sao</option>
              <option value="3">⭐ 3+ sao</option>
              <option value="2">⭐ 2+ sao</option>
            </select>
          </div>

          {/* Sort Options */}
          <div style={{ display: "flex", flexDirection: "column", gap: 4 }}>
            <label style={{ fontSize: 12, color: "#6b7280", fontWeight: 500 }}>
              Sắp xếp theo
            </label>
            <div style={{ display: "flex", gap: 8 }}>
              <select
                value={sortBy}
                onChange={(e) => updateFilter("sortBy", e.target.value)}
                style={{
                  padding: "8px 32px 8px 12px",
                  border: "1px solid #e5e7eb",
                  borderRadius: 8,
                  fontSize: 14,
                  backgroundColor: "white",
                  cursor: "pointer",
                  outline: "none",
                  minWidth: 160,
                }}
              >
                <option value="createdAt">Mới nhất</option>
                <option value="rating">Đánh giá cao</option>
                <option value="reviews">Nhiều review</option>
                <option value="price">Giá</option>
                <option value="title">Tên A-Z</option>
              </select>

              {/* Sort Order Toggle */}
              <button
                onClick={() =>
                  updateFilter(
                    "sortOrder",
                    sortOrder === "asc" ? "desc" : "asc"
                  )
                }
                style={{
                  padding: "8px 12px",
                  border: "1px solid #e5e7eb",
                  borderRadius: 8,
                  backgroundColor: "white",
                  cursor: "pointer",
                  fontSize: 16,
                  display: "flex",
                  alignItems: "center",
                  justifyContent: "center",
                  minWidth: 42,
                  transition: "all 0.2s",
                }}
                title={sortOrder === "asc" ? "Tăng dần" : "Giảm dần"}
                onMouseEnter={(e) =>
                  (e.currentTarget.style.backgroundColor = "#f3f4f6")
                }
                onMouseLeave={(e) =>
                  (e.currentTarget.style.backgroundColor = "white")
                }
              >
                {sortOrder === "asc" ? "↑" : "↓"}
              </button>
            </div>
          </div>
        </div>
      </div>

      <div
        style={{ display: "grid", gridTemplateColumns: "220px 1fr", gap: 16 }}
      >
        <aside className="card" style={{ height: "fit-content" }}>
          <h3>Danh mục</h3>
          <ul style={{ listStyle: "none", padding: 0, margin: 0 }}>
            <li>
              <button
                className="btn ghost small"
                onClick={() => navigate(`/store`)}
                style={{ width: "100%", textAlign: "left", marginBottom: 8 }}
              >
                Tất cả
              </button>
            </li>
            {categories.map((c) => (
              <li key={c.id} style={{ marginBottom: 6 }}>
                <button
                  className="btn ghost small"
                  onClick={() => updateFilter("categoryId", c.id)}
                  style={{
                    width: "100%",
                    textAlign: "left",
                    background: categoryId == c.id ? "#e0e7ff" : "transparent",
                  }}
                >
                  {c.name}
                </button>
              </li>
            ))}
          </ul>
        </aside>

        <section>
          <div className="page-grid">
            {currentGames.map((g) => {
              const title = g.title ?? g.name ?? g.gameName ?? "Untitled";
              const price =
                g.price != null
                  ? currencyFormatter.format(g.price)
                  : g.priceText ?? "";
              let imgSrc =
                g.imageUrl ??
                g.ImageUrl ?? // some backends use PascalCase
                g.thumbnailUrl ??
                g.thumbnail ??
                g.coverUrl ??
                g.imagePath ??
                "";

              // nếu là đường dẫn tương đối, prefix bằng API base
              if (imgSrc && !/^https?:\/\//i.test(imgSrc) && API_BASE) {
                imgSrc =
                  API_BASE + (imgSrc.startsWith("/") ? "" : "/") + imgSrc;
              }
              // fallback to placeholder
              const finalImg = imgSrc || PLACEHOLDER;

              return (
                <article key={g.id ?? g.gameId ?? title} className="card">
                  <div
                    style={{
                      height: 160,
                      overflow: "hidden",
                      borderRadius: 8,
                      marginBottom: 8,
                    }}
                  >
                    <img
                      src={finalImg}
                      alt={title}
                      style={{
                        width: "100%",
                        height: "100%",
                        objectFit: "cover",
                        display: "block",
                      }}
                      onError={(e) => (e.currentTarget.src = PLACEHOLDER)}
                    />
                  </div>

                  <h3>{title}</h3>
                  <div className="muted small" style={{ minHeight: 36 }}>
                    {g.shortDescription ?? g.description?.slice?.(0, 80) ?? ""}
                  </div>

                  {/* Review Summary */}
                  <ReviewSummary
                    averageRating={g.averageRating || 0}
                    totalReviews={g.reviewCount || 0}
                  />

                  <div className="card-actions" style={{ marginTop: 12 }}>
                    <div style={{ fontWeight: 800 }}>{price}</div>
                    <div style={{ display: "flex", gap: 8, flexWrap: "wrap" }}>
                      <WishlistButton gameId={g.id ?? g.gameId} />
                      <button
                        className="btn ghost"
                        onClick={() => navigate(`/games/${g.id ?? g.gameId}`)}
                      >
                        Chi tiết
                      </button>
                      <button
                        className="btn"
                        disabled={!!addingMap[g.id ?? g.gameId]}
                        onClick={async () => {
                          const gid = g.id ?? g.gameId;
                          try {
                            setAddingMap((m) => ({ ...m, [gid]: true }));
                            // use provider's addToCart for optimistic update and title mapping
                            await addToCartCtx({
                              id: gid,
                              qty: 1,
                              title: title,
                              unitPrice: g.price ?? 0,
                            });
                          } catch (err) {
                            console.error(err);
                            try {
                              toast.error(
                                err?.response?.data || "Thêm thất bại"
                              );
                            } catch {}
                          } finally {
                            setAddingMap((m) => ({ ...m, [gid]: false }));
                          }
                        }}
                      >
                        Thêm
                      </button>
                    </div>
                  </div>
                </article>
              );
            })}
          </div>

          {/* pagination controls */}
          <Pagination
            currentPage={currentPage}
            totalPages={totalPages}
            onPageChange={(p) => setCurrentPage(p)}
          />
        </section>
      </div>
    </main>
  );
}
