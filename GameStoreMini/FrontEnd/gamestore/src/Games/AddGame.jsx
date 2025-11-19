import React, { useEffect, useState } from "react";
import { useNavigate, useParams, useLocation } from "react-router-dom";
import GameAPI from "../API/GameAPI";
import { useToast } from "../Components/Toast";
import "../Decorate/AdminForm.css";

export default function AddGame() {
  const params = useParams(); // optional edit id via route param
  const location = useLocation();
  const queryId = new URLSearchParams(location.search).get("id");
  const id = params.id ?? queryId; // support both /add-game/:id and /add-game?id=...
  const [title, setTitle] = useState("");
  const [description, setDescription] = useState("");
  const [price, setPrice] = useState(0);
  const [stock, setStock] = useState(0);
  const [categoryId, setCategoryId] = useState("");
  const [imageFile, setImageFile] = useState(null);
  const [imagePreviewUrl, setImagePreviewUrl] = useState("");
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
        // backend may return CategoryId or categoryId
        setCategoryId(g.categoryId ?? g.CategoryId ?? "");
        // show existing image if present
        setImagePreviewUrl(g.imageUrl || g.ImageUrl || "");
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

  // handle file selection + preview
  const onFileChange = (file) => {
    setImageFile(file ?? null);
    if (file) {
      try {
        const url = URL.createObjectURL(file);
        setImagePreviewUrl(url);
      } catch {
        setImagePreviewUrl("");
      }
    }
  };

  return (
    <main className="admin-form">
      <h2>{id ? "Sửa game" : "Thêm game mới"}</h2>
      <div className="card">
        <form onSubmit={onSubmit}>
          <div className="row">
            <div className="col-2">
              <label>Tiêu đề</label>
              <input
                value={title}
                onChange={(e) => setTitle(e.target.value)}
                required
              />
            </div>
          </div>

          <div className="row">
            <div className="col-2">
              <label>Mô tả</label>
              <textarea
                value={description}
                onChange={(e) => setDescription(e.target.value)}
              />
            </div>
          </div>

          <div className="row">
            <div className="col-1">
              <label>Giá</label>
              <input
                type="number"
                value={price}
                onChange={(e) => setPrice(Number(e.target.value))}
                required
              />
            </div>
            <div className="col-1">
              <label>Kho</label>
              <input
                type="number"
                value={stock}
                onChange={(e) => setStock(Number(e.target.value))}
                required
              />
            </div>
          </div>

          <div className="row">
            <div className="col-2">
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
          </div>

          <div className="row file-row">
            <div className="col-1">
              <label>Ảnh</label>
              <input
                type="file"
                accept="image/*"
                onChange={(e) => onFileChange(e.target.files?.[0] ?? null)}
              />
            </div>
            <div>
              <div className="img-preview">
                {imagePreviewUrl ? (
                  <img src={imagePreviewUrl} alt="preview" />
                ) : (
                  <div style={{ color: "#94a3b8", fontSize: 13 }}>No image</div>
                )}
              </div>
            </div>
          </div>

          <div className="actions">
            <button type="submit" className="primary" disabled={loading}>
              {loading ? "Đang lưu..." : "Lưu"}
            </button>
            <button type="button" onClick={() => navigate("/admin/games")}>
              Hủy
            </button>
          </div>
        </form>
      </div>
    </main>
  );
}
