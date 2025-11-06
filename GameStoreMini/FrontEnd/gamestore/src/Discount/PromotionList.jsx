import React, { useEffect, useState } from "react";
import PromotionAPI from "../API/PromotionAPI";
import PromotionCard from "../Components/PromotionCard";

export default function PromotionList() {
  const [promotions, setPromotions] = useState([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const load = async () => {
      try {
        setLoading(true);
        const res = await PromotionAPI.getActivePromotions({
          page: 1,
          pageSize: 20,
        });
        setPromotions(res.data || res);
      } catch (err) {
        console.error(err);
      } finally {
        setLoading(false);
      }
    };
    load();
  }, []);

  if (loading) return <div>Đang tải chương trình...</div>;
  if (!promotions || promotions.length === 0)
    return <div>Hiện không có chương trình khuyến mãi.</div>;

  return (
    <div className="promotion-list">
      {promotions.map((p) => (
        <PromotionCard key={p.id} p={p} />
      ))}
    </div>
  );
}
