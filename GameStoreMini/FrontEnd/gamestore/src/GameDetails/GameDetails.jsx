import React, { useEffect, useState } from "react";
import { useParams, Link, useNavigate } from "react-router-dom";
import { getOne as getGame } from "../API/GameAPI";
import { useCart } from "../Cart/CartProvider";
import { useToast } from "../Components/Toast";
import { getUserRole } from "../Auth/useAuth";
import viewHistoryAPI from "../API/ViewHistoryAPI";
import "../Decorate/GameDetails.css"; // <-- imported stylesheet
import ReviewList from "../Review/ReviewList";
import formatCurrency from "../Utils/formatCurrency";

export default function GameDetails() {
  const { id } = useParams();
  const navigate = useNavigate();
  const [game, setGame] = useState(null);
  const [loading, setLoading] = useState(true);
  const [adding, setAdding] = useState(false);
  const API_BASE = import.meta.env.VITE_API_BASE ?? "/api";
  const PLACEHOLDER =
    "data:image/svg+xml,%3Csvg xmlns='http://www.w3.org/2000/svg' width='800' height='450'%3E%3Crect width='100%25' height='100%25' fill='%23222'/%3E%3C/svg%3E";

  useEffect(() => {
    if (!id) return;
    let cancelled = false;

    (async () => {
      try {
        setLoading(true);
        const res = await getGame(id);
        if (cancelled) return;
        // handle both: API returns the entity directly or wraps it in { data: ... }
        const payload =
          res && typeof res === "object" && "data" in res ? res.data : res;
        setGame(payload ?? null);

        // Tự động lưu vào lịch sử xem nếu user đã đăng nhập VÀ KHÔNG PHẢI ADMIN
        const token = localStorage.getItem("token");
        const userRole = getUserRole();
        if (token && payload && userRole !== "Admin") {
          try {
            await viewHistoryAPI.addViewHistory(
              payload.id ?? payload.gameId ?? id
            );
          } catch (err) {
            // Không hiển thị lỗi cho user, chỉ log
            console.log("Could not save view history:", err);
          }
        }
      } catch (e) {
        console.error("Failed to load game", e);
        setGame(null);
      } finally {
        if (!cancelled) setLoading(false);
      }
    })();

    return () => {
      cancelled = true;
    };
  }, [id]);

  const { addToCart: addToCartCtx } = useCart();
  const toast = useToast();
  const handleAdd = async () => {
    if (!game) return;
    const rawId = game.id ?? game.gameId ?? id;
    // normalize id: string -> number when possible
    const gameId =
      typeof rawId === "string" && /^\d+$/.test(rawId) ? Number(rawId) : rawId;
    try {
      setAdding(true);
      // call provider's addToCart (optimistic + sync with server)
      await addToCartCtx({
        id: gameId,
        qty: 1,
        title,
        unitPrice: game.price ?? game.priceAmount ?? 0,
      });
      navigate("/cart");
    } catch (e) {
      console.error("Add to cart failed", e);
      // If user is not authenticated, redirect to login so they can sign in
      const status = e?.response?.status;
      if (status === 401) {
        // optional: show a friendly message then redirect to login
        try {
          toast.info("Vui lòng đăng nhập để thêm vào giỏ hàng");
        } catch {}
        navigate("/login", { replace: true });
      } else {
        try {
          toast.error("Thêm vào giỏ hàng thất bại");
        } catch {}
      }
    } finally {
      setAdding(false);
    }
  };

  if (loading)
    return (
      <div style={{ padding: 20 }}>
        <strong>Đang tải...</strong>
      </div>
    );

  if (!game)
    return (
      <div style={{ padding: 20 }}>
        <p>Không tìm thấy trò chơi.</p>
        <Link to="/">Quay về cửa hàng</Link>
      </div>
    );

  const title = game.title ?? game.name ?? game.gameName ?? "Untitled";
  // Giá từ API có thể được trả về với nhiều field khác nhau: `price`, `priceAmount`,
  // hoặc `priceText`. Trường hợp phổ biến trong project này là lưu giá theo VND
  // (ví dụ 1399000). Trước đây code dùng currency: "USD" nên giá hiển thị là
  // "$1,399,000.00" — nhìn sai lệch về đơn vị.
  //
  // Sửa ở đây để hiển thị đúng theo locale Việt Nam và đơn vị VND. Nếu backend
  // thay đổi đơn vị (ví dụ trả về giá bằng USD), cần thay lại `currency` phù
  // hợp hoặc gửi thêm metadata từ API.
  const priceNum = game.price ?? game.priceAmount ?? null;
  const price =
    priceNum != null ? formatCurrency(priceNum) : game.priceText ?? "";

  let imgSrc =
    game.imageUrl ||
    game.ImageUrl ||
    game.thumbnailUrl ||
    game.thumbnail ||
    game.coverUrl ||
    game.imagePath ||
    "";

  if (imgSrc && !/^https?:\/\//i.test(imgSrc) && API_BASE) {
    imgSrc = API_BASE + (imgSrc.startsWith("/") ? "" : "/") + imgSrc;
  }
  const finalImg = imgSrc || PLACEHOLDER;

  return (
    <div style={{ padding: 20, maxWidth: 900, margin: "0 auto" }}>
      <div style={{ display: "flex", gap: 20, alignItems: "flex-start" }}>
        <div
          style={{
            width: 360,
            minHeight: 200,
            borderRadius: 6,
            overflow: "hidden",
            background: "#222",
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
          />
        </div>

        <div style={{ flex: 1 }}>
          <h1 style={{ marginTop: 0 }}>{title}</h1>
          <p style={{ fontSize: 20, fontWeight: 600 }}>{price}</p>
          <div style={{ margin: "12px 0" }}>
            <strong>Thể loại:</strong>{" "}
            {/*
                Backend có thể trả về thể loại dưới dạng nhiều kiểu:
                - `categoryName` (string)
                - `category` (string) hoặc `category` (object) như { id, name }
                Để tránh hiển thị 'Chưa xác định' khi thông tin vẫn có trong object,
                chúng ta thử `categoryName`, sau đó `category.name`, rồi `category`
                (fallback). Nếu backend thay đổi cấu trúc, chỉnh logic ở đây.
              */}
            {
              // 1) categoryNames (camelCase or PascalCase)
              (game.categoryNames &&
                game.categoryNames.length &&
                game.categoryNames[0]) ||
                (game.CategoryNames &&
                  game.CategoryNames.length &&
                  game.CategoryNames[0]) ||
                // 2) Categories array of objects
                (Array.isArray(game.Categories) &&
                  game.Categories[0] &&
                  (game.Categories[0].name || game.Categories[0].Name)) ||
                (Array.isArray(game.categories) &&
                  game.categories[0] &&
                  (game.categories[0].name || game.categories[0].Name)) ||
                // 3) single-field fallbacks
                game.categoryName ||
                game.category?.name ||
                game.category ||
                // 4) default
                "Chưa xác định"
            }
          </div>
          <div style={{ marginBottom: 16, color: "#444" }}>
            {game.description ||
              game.summary ||
              game.shortDescription ||
              "Không có mô tả."}
          </div>

          <div style={{ display: "flex", gap: 10 }}>
            <button
              className="btn btn-primary"
              onClick={handleAdd}
              disabled={adding}
            >
              {adding ? "Đang thêm..." : "Thêm vào giỏ"}
            </button>
            <Link to="/" className="btn btn-outline-secondary">
              Quay về cửa hàng
            </Link>
          </div>

          {game.stock != null && (
            <div style={{ marginTop: 12 }}>
              <small>Kho: {game.stock}</small>
            </div>
          )}
        </div>
      </div>

      {/* Reviews Section */}
      <div style={{ marginTop: 48 }}>
        <ReviewList gameId={id} />
      </div>
    </div>
  );
}
