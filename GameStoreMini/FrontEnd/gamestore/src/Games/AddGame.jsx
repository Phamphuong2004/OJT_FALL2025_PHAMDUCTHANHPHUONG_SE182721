import React, { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import GameAPI from "../API/GameAPI";
import { useToast } from "../Components/Toast";

export default function AddGame() {
  const { id } = useParams(); // optional edit id
  const [title, setTitle] = useState("");
  const [description, setDescription] = useState("");
  const [price, setPrice] = useState(0);
  const [stock, setStock] = useState(0);
  const [categoryId, setCategoryId] = useState("");
  const [imageFile, setImageFile] = useState(null);
  const [categories, setCategories] = useState([]);
  const [loading, setLoading] = useState(false);
  const navigate = useNavigate();
  const toast = useToast();

  useEffect(() => {
    (async () => {
      try {
        const cats = await GameAPI.getCategories();
        setCategories(Array.isArray(cats) ? cats : cats?.items ?? []);
      } catch (e) {
        console.error(e);
      }
    })();
  }, []);

  useEffect(() => {
    if (!id) return;
    (async () => {
      try {
        const g = await GameAPI.getOne(id);
        setTitle(g.title || "");
        setDescription(g.description || "");
        setPrice(g.price ?? 0);
        setStock(g.stock ?? 0);
        setCategoryId(g.categoryId ?? "");
      } catch (e) {
        console.error(e);
      }
    })();
  }, [id]);

  const onSubmit = async (e) => {
    e.preventDefault();
    setLoading(true);
    try {
      if (id) {
        await GameAPI.updateGame(id, {
          title,
          description,
          price,
          stock,
          categoryIds: categoryId ? [Number(categoryId)] : undefined,
          imageFile,
        });
        try {
          toast.success("Cập nhật game thành công");
        } catch {}
      } else {
        await GameAPI.addGame({
          title,
          description,
          price,
          stock,
          categoryIds: categoryId ? [Number(categoryId)] : undefined,
          imageFile,
        });
        try {
          toast.success("Thêm game thành công");
        } catch {}
      }
      navigate("/admin/games");
    } catch (err) {
      console.error(err);
      try {
        toast.error(err.response?.data?.message || "Lỗi khi lưu game");
      } catch {}
    } finally {
      setLoading(false);
    }
  };

  return (
    <main style={{ padding: 20 }}>
      <h2>{id ? "Sửa game" : "Thêm game mới"}</h2>
      <form onSubmit={onSubmit}>
        <div>
          <label>Tiêu đề</label>
          <input
            value={title}
            onChange={(e) => setTitle(e.target.value)}
            required
          />
        </div>
        <div>
          <label>Mô tả</label>
          <textarea
            value={description}
            onChange={(e) => setDescription(e.target.value)}
          />
        </div>
        <div>
          <label>Giá</label>
          <input
            type="number"
            value={price}
            onChange={(e) => setPrice(Number(e.target.value))}
            required
          />
        </div>
        <div>
          <label>Kho</label>
          <input
            type="number"
            value={stock}
            onChange={(e) => setStock(Number(e.target.value))}
            required
          />
        </div>
        <div>
          <label>Danh mục</label>
          <select
            value={categoryId}
            onChange={(e) => setCategoryId(e.target.value)}
          >
            <option value="">-- Chọn --</option>
            {categories.map((c) => (
              <option key={c.id} value={c.id}>
                {c.name}
              </option>
            ))}
          </select>
        </div>
        <div>
          <label>Ảnh</label>
          <input
            type="file"
            accept="image/*"
            onChange={(e) => setImageFile(e.target.files?.[0] ?? null)}
          />
        </div>
        <div style={{ marginTop: 12 }}>
          <button type="submit" disabled={loading}>
            {loading ? "Đang lưu..." : "Lưu"}
          </button>
        </div>
      </form>
    </main>
  );
}
