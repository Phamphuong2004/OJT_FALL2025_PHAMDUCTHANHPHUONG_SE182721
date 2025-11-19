import React, { useEffect, useState, useRef } from "react";
import { useNavigate } from "react-router-dom";
import GameAPI from "../API/GameAPI";
import { useToast } from "../Components/Toast";
import "../Decorate/AdminGames.css";
import { getUserRole } from "../Auth/useAuth";

export default function GamesList() {
  const [games, setGames] = useState([]);
  const [loading, setLoading] = useState(true);
  const [search, setSearch] = useState("");
  const [page, setPage] = useState(1);
  const [pageSize, setPageSize] = useState(10);
  const [total, setTotal] = useState(0);
  const navigate = useNavigate();
  const toast = useToast();
  const searchRef = useRef();
  const userRole = getUserRole();

  // debounce search
  useEffect(() => {
    const t = setTimeout(() => {
      setPage(1);
      fetchGames();
    }, 400);
    return () => clearTimeout(t);
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [search, pageSize]);

  useEffect(() => {
    // initial load
    fetchGames();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [page]);

  async function fetchGames() {
    setLoading(true);
    try {
      const params = { q: search || undefined, page, pageSize };
      const res = await GameAPI.getAll(params);
      let list = [];
      let tot = 0;
      if (Array.isArray(res)) {
        list = res;
        tot = res.length;
      } else if (res?.items) {
        list = res.items;
        tot = res.total ?? list.length;
      } else {
        list = res?.items ?? [];
        tot = list.length;
      }
      setGames(list);
      setTotal(Number(tot || 0));
    } catch (e) {
      console.error(e);
      setGames([]);
      setTotal(0);
    } finally {
      setLoading(false);
    }
  }

  const onDelete = async (id) => {
    if (!confirm("Bạn có chắc muốn xóa game này?")) return;
    try {
      await GameAPI.deleteGame(id);
      toast?.success?.("Đã xóa");
      // refetch current page
      fetchGames();
    } catch (e) {
      console.error(e);
      toast?.error?.("Xóa thất bại");
    }
  };

  const onAddToCart = async (game) => {
    toast?.success?.(`Đã thêm ${game.title} vào giỏ hàng!`);
  };

  const totalPages = Math.max(1, Math.ceil((total || 0) / pageSize));

  if (loading) return <div style={{ padding: 20 }}>Loading...</div>;

  return (
    <main className="admin-games">
      <h2>Quản lý Games</h2>

      <div className="admin-games-header">
        <button className="btn-add" onClick={() => navigate("/admin/add-game")}>
          Thêm game
        </button>
        <div className="controls">
          <input
            ref={searchRef}
            className="admin-games-search"
            placeholder="Tìm theo tiêu đề..."
            value={search}
            onChange={(e) => setSearch(e.target.value)}
          />
        </div>

        <div style={{ marginLeft: "auto" }}>
          <label style={{ marginRight: 8 }}>Hiển thị: </label>
          <select
            className="page-size"
            value={pageSize}
            onChange={(e) => setPageSize(Number(e.target.value))}
          >
            <option value={5}>5</option>
            <option value={10}>10</option>
            <option value={20}>20</option>
          </select>
        </div>
      </div>

      <div>
        {games.length === 0 && (
          <div style={{ color: "#6b7280" }}>Không tìm thấy game nào.</div>
        )}
        <ul className="game-list">
          {games.map((g) => (
            <li
              key={g.id ?? g.Id ?? `${g.title}-${Math.random()}`}
              className="game-item"
            >
              <div className="game-thumb">
                <img
                  src={g.imageUrl || g.ImageUrl || ""}
                  alt={g.title || g.Title}
                  onError={(e) => {
                    // prevent infinite loop
                    e.target.onerror = null;
                    e.target.src =
                      'data:image/svg+xml;utf8,<svg xmlns="http://www.w3.org/2000/svg" width="72" height="72"><rect width="100%" height="100%" fill="%23f3f4f6"/><text x="50%" y="50%" font-size="10" fill="%239ca3af" text-anchor="middle" dy=".3em">No image</text></svg>';
                  }}
                />
              </div>
              <div className="game-meta">
                <div className="game-title">{g.title || g.Title}</div>
                <div className="game-price">
                  {Number(g.price ?? g.Price ?? 0).toLocaleString()} ₫
                </div>
              </div>
              <div className="game-actions">
                <button
                  onClick={() => navigate(`/admin/add-game?id=${g.id ?? g.Id}`)}
                >
                  Sửa
                </button>
                <button onClick={() => onDelete(g.id ?? g.Id)}>Xóa</button>
                {userRole !== "Admin" && (
                  <button className="primary" onClick={() => onAddToCart(g)}>
                    Thêm vào giỏ
                  </button>
                )}
              </div>
            </li>
          ))}
        </ul>
      </div>

      <div className="pagination">
        <button
          disabled={page <= 1}
          onClick={() => setPage((p) => Math.max(1, p - 1))}
        >
          ← Prev
        </button>
        <div>
          Trang {page} / {totalPages} — {total} items
        </div>
        <button
          disabled={page >= totalPages}
          onClick={() => setPage((p) => p + 1)}
        >
          Next →
        </button>
      </div>
    </main>
  );
}
