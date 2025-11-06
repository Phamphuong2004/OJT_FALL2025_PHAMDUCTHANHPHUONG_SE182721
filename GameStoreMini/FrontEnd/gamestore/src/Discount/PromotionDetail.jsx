import React, { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import PromotionAPI from "../API/PromotionAPI";
import { isAuthenticated } from "../Auth/useAuth";

export default function PromotionDetail() {
  const { slug } = useParams();
  const [promotion, setPromotion] = useState(null);
  const [claiming, setClaiming] = useState(false);
  const [claimMessage, setClaimMessage] = useState(null);

  useEffect(() => {
    const load = async () => {
      try {
        const res = await PromotionAPI.getPromotionBySlug(slug);
        setPromotion(res);
      } catch (err) {
        console.error(err);
      }
    };
    load();
  }, [slug]);

  if (!promotion) return <div>Không tìm thấy chương trình.</div>;

  return (
    <div className="promotion-detail">
      <h1>{promotion.title}</h1>
      {promotion.imageUrl && (
        <img src={promotion.imageUrl} alt={promotion.title} />
      )}
      {isAuthenticated() && (
        <div style={{ margin: "12px 0" }}>
          <button
            onClick={async () => {
              if (claiming) return;
              setClaiming(true);
              setClaimMessage(null);
              try {
                const res = await PromotionAPI.claimPromotion(promotion.id);
                setClaimMessage({
                  type: "success",
                  text: "Đã nhận khuyến mãi!",
                });
                console.log("Claim result:", res);
              } catch (err) {
                console.error(err);
                const msg =
                  err?.response?.data?.message || "Không thể nhận khuyến mãi.";
                setClaimMessage({ type: "error", text: msg });
              } finally {
                setClaiming(false);
              }
            }}
            disabled={claiming}
            className="btn btn-primary"
          >
            {claiming ? "Đang xử lý..." : "Nhận khuyến mãi"}
          </button>
          {claimMessage && (
            <div
              style={{
                marginTop: 8,
                color: claimMessage.type === "error" ? "#c00" : "#080",
              }}
            >
              {claimMessage.text}
            </div>
          )}
        </div>
      )}
      <div dangerouslySetInnerHTML={{ __html: promotion.content }} />
      <h3>Games trong chương trình</h3>
      <ul>
        {promotion.games.map((g) => (
          <li key={g.id}>
            {g.title} — {g.price} đ
          </li>
        ))}
      </ul>
    </div>
  );
}
