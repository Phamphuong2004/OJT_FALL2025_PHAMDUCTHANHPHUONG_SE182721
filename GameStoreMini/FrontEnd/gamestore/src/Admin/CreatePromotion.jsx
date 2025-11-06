import React from "react";
import { useNavigate } from "react-router-dom";
import PromotionForm from "../Components/PromotionForm";
import { useToast } from "../Components/Toast";

export default function CreatePromotion() {
  const navigate = useNavigate();
  const toast = useToast();

  const handleSaved = () => {
    toast.success("Tạo chương trình thành công");
    navigate("/admin/promotion");
  };

  return (
    <div style={{ padding: 20 }}>
      <h2>Tạo chương trình khuyến mãi</h2>
      <PromotionForm onSaved={handleSaved} />
    </div>
  );
}
