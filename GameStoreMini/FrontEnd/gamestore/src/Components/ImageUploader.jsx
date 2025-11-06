import React, { useState } from "react";

export default function ImageUploader({ onFileSelected }) {
  const [preview, setPreview] = useState(null);

  const handleFile = (e) => {
    const file = e.target.files && e.target.files[0];
    if (!file) return;
    onFileSelected && onFileSelected(file);
    const reader = new FileReader();
    reader.onload = () => setPreview(reader.result);
    reader.readAsDataURL(file);
  };

  return (
    <div className="image-uploader">
      <input type="file" accept="image/*" onChange={handleFile} />
      {preview && <img src={preview} alt="preview" style={{ maxWidth: 220 }} />}
    </div>
  );
}
