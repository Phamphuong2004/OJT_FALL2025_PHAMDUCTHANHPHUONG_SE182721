import React, { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import PromotionForm from "../Components/PromotionForm";
import AdminPromotionAPI from "../API/AdminPromotionAPI";
import { useToast } from "../Components/Toast";

export default function EditPromotion() {
  const { id } = useParams();
  const [initial, setInitial] = useState(null);
  const toast = useToast();
  const navigate = useNavigate();

  useEffect(() => {
    const load = async () => {
      try {
        const data = await AdminPromotionAPI.getPromotionById(id);
        // map backend fields to initial values expected by PromotionForm
        setInitial({
          id: data.id,
          title: data.title,
          summary: data.summary,
          content: data.content,
          slug: data.slug,
          startDate: data.startDate?.slice(0, 16),
          endDate: data.endDate?.slice(0, 16),
          promotionType: data.promotionType,
          discountPercentage: data.discountPercentage,
          fixedDiscountAmount: data.fixedDiscountAmount,
          eventType: data.eventType,
          isActive: data.isActive,
          isFeatured: data.isFeatured,
          gameIds: (data.games || []).map((g) => g.id),
          imageUrl: data.imageUrl,
        });
      } catch (err) {
        toast.error("Không tải được chương trình");
      }
    };
    load();
  }, [id, toast]);

  const handleSaved = () => {
    toast.success("Cập nhật thành công");
    navigate("/admin/promotion");
  };

  if (!initial) return <div>Đang tải...</div>;

  return (
    <div style={{ padding: 20 }}>
      <h2>Chỉnh sửa chương trình</h2>
      <PromotionForm initialValues={initial} onSaved={handleSaved} />
    </div>
  );
}
