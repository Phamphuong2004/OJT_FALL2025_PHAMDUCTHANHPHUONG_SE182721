import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import GameAPI from "../../API/GameAPI";
import { useToast } from "../Components/Toast";

export default function GamesList() {
  const [games, setGames] = useState([]);
  const [loading, setLoading] = useState(true);
  const navigate = useNavigate();
  const toast = useToast();

  useEffect(() => {
    (async () => {
      try {
        const res = await GameAPI.getAll();
        const list = Array.isArray(res) ? res : res?.items ?? [];
        setGames(list);
      } catch (e) {
        console.error(e);
        setGames([]);
      } finally {
        setLoading(false);
      }
    })();
  }, []);

  const onDelete = async (id) => {
    if (!confirm("Bạn có chắc muốn xóa game này?")) return;
    try {
      await GameAPI.deleteGame(id);
      setGames((g) => g.filter((x) => x.id !== id));
      try {
        toast.success("Đã xóa");
      } catch {}
    } catch (e) {
      console.error(e);
      try {
        toast.error("Xóa thất bại");
      } catch {}
    }
  };

  if (loading) return <div style={{ padding: 20 }}>Loading...</div>;

  return (
    <main style={{ padding: 20 }}>
      <h2>Quản lý Games</h2>
      <button onClick={() => navigate("/admin/add-game")}>Thêm game</button>
      <ul style={{ marginTop: 12 }}>
        {games.map((g) => (
          <li key={g.id} style={{ marginBottom: 8 }}>
            <strong>{g.title}</strong> — {Number(g.price || 0).toLocaleString()}
            ₫
            <div style={{ display: "inline-block", marginLeft: 12 }}>
              <button onClick={() => navigate(`/admin/add-game?id=${g.id}`)}>
                Sửa
              </button>
              <button onClick={() => onDelete(g.id)} style={{ marginLeft: 8 }}>
                Xóa
              </button>
            </div>
          </li>
        ))}
      </ul>
    </main>
  );
}
