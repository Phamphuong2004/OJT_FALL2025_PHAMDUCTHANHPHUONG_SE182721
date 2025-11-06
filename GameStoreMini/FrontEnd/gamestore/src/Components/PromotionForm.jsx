import React, { useState, useEffect } from "react";
import AdminPromotionAPI from "../API/AdminPromotionAPI";
import ImageUploader from "./ImageUploader";

export default function PromotionForm({ initialValues = null, onSaved }) {
  const [values, setValues] = useState(
    initialValues || {
      title: "",
      summary: "",
      content: "",
      slug: "",
      startDate: "",
      endDate: "",
      promotionType: "PERCENTAGE",
      discountPercentage: 0,
      fixedDiscountAmount: 0,
      eventType: "",
      isActive: true,
      isFeatured: false,
      gameIds: [],
      imageFile: null,
    }
  );
  const [saving, setSaving] = useState(false);

  useEffect(() => {
    if (initialValues) setValues({ ...initialValues, imageFile: null });
  }, [initialValues]);

  const handleChange = (e) => {
    const { name, value, type, checked } = e.target;
    setValues((v) => ({ ...v, [name]: type === "checkbox" ? checked : value }));
  };

  const handleImage = (file) => setValues((v) => ({ ...v, imageFile: file }));

  const handleSubmit = async (e) => {
    e.preventDefault();
    setSaving(true);
    try {
      const form = new FormData();
      form.append("Title", values.title);
      form.append("Content", values.content);
      form.append("Summary", values.summary);
      form.append("Slug", values.slug || "");
      form.append("StartDate", values.startDate);
      form.append("EndDate", values.endDate);
      form.append("PromotionType", values.promotionType);
      form.append("EventType", values.eventType);
      form.append("DiscountPercentage", values.discountPercentage);
      if (values.fixedDiscountAmount)
        form.append("FixedDiscountAmount", values.fixedDiscountAmount);
      form.append("IsActive", values.isActive);
      form.append("IsFeatured", values.isFeatured);
      if (values.gameIds && values.gameIds.length) {
        values.gameIds.forEach((id) => form.append("GameIds", id));
      }
      if (values.imageFile) form.append("Image", values.imageFile);

      if (initialValues && initialValues.id) {
        await AdminPromotionAPI.updatePromotion(initialValues.id, form);
      } else {
        await AdminPromotionAPI.createPromotion(form);
      }

      onSaved && onSaved();
    } catch (err) {
      console.error(err);
      alert("Lỗi khi lưu chương trình");
    } finally {
      setSaving(false);
    }
  };

  return (
    <form
      className="promotion-form"
      onSubmit={handleSubmit}
      encType="multipart/form-data"
    >
      <div>
        <label>Tiêu đề</label>
        <input
          name="title"
          value={values.title}
          onChange={handleChange}
          required
        />
      </div>

      <div>
        <label>Slug</label>
        <input name="slug" value={values.slug} onChange={handleChange} />
      </div>

      <div>
        <label>Bắt đầu</label>
        <input
          name="startDate"
          type="datetime-local"
          value={values.startDate}
          onChange={handleChange}
          required
        />
      </div>

      <div>
        <label>Kết thúc</label>
        <input
          name="endDate"
          type="datetime-local"
          value={values.endDate}
          onChange={handleChange}
          required
        />
      </div>

      <div>
        <label>Loại khuyến mãi</label>
        <select
          name="promotionType"
          value={values.promotionType}
          onChange={handleChange}
        >
          <option value="PERCENTAGE">Phần trăm</option>
          <option value="FIXED">Tiền cố định</option>
          <option value="SPECIAL">Đặc biệt</option>
        </select>
      </div>

      <div>
        <label>Giảm (%)</label>
        <input
          name="discountPercentage"
          type="number"
          value={values.discountPercentage}
          onChange={handleChange}
        />
      </div>

      <div>
        <label>Giảm cố định</label>
        <input
          name="fixedDiscountAmount"
          type="number"
          value={values.fixedDiscountAmount}
          onChange={handleChange}
        />
      </div>

      <div>
        <label>Ảnh đại diện</label>
        <ImageUploader onFileSelected={handleImage} />
      </div>

      <div>
        <label>Nội dung</label>
        <textarea
          name="content"
          value={values.content}
          onChange={handleChange}
          rows={6}
        />
      </div>

      <div>
        <label>Is Active</label>
        <input
          name="isActive"
          type="checkbox"
          checked={values.isActive}
          onChange={handleChange}
        />
      </div>

      <div>
        <label>Is Featured</label>
        <input
          name="isFeatured"
          type="checkbox"
          checked={values.isFeatured}
          onChange={handleChange}
        />
      </div>

      <button type="submit" disabled={saving}>
        {saving ? "Đang lưu..." : "Lưu"}
      </button>
    </form>
  );
}
