import React, { useState, useEffect } from "react";
import wishlistAPI from "../API/WishlistAPI";
import { Heart } from "lucide-react";
import { getUserRole } from "../Auth/useAuth";

export default function WishlistButton({ gameId, onToggle }) {
  const [inWishlist, setInWishlist] = useState(false);
  const [loading, setLoading] = useState(false);
  const [isAuthenticated, setIsAuthenticated] = useState(false);
  const [role, setRole] = useState(null);

  useEffect(() => {
    const token = localStorage.getItem("token");
    const userRole = getUserRole();
    setIsAuthenticated(!!token);
    setRole(userRole);

    if (token && gameId && userRole !== "Admin") {
      checkWishlistStatus();
    }
  }, [gameId]);

  const checkWishlistStatus = async () => {
    try {
      const result = await wishlistAPI.checkInWishlist(gameId);
      setInWishlist(result.inWishlist);
    } catch (error) {
      console.error("Error checking wishlist:", error);
    }
  };

  const toggleWishlist = async (e) => {
    e.stopPropagation(); // Prevent card click
    e.preventDefault();

    if (!isAuthenticated) {
      alert("Vui lòng đăng nhập để sử dụng wishlist");
      return;
    }

    if (role === "Admin") {
      alert("Admin không cần wishlist");
      return;
    }

    setLoading(true);
    try {
      if (inWishlist) {
        await wishlistAPI.removeFromWishlist(gameId);
        setInWishlist(false);
      } else {
        await wishlistAPI.addToWishlist(gameId);
        setInWishlist(true);
      }
      if (onToggle) onToggle();
    } catch (error) {
      console.error("Error toggling wishlist:", error);
      alert(error.response?.data?.message || "Có lỗi xảy ra");
    } finally {
      setLoading(false);
    }
  };

  // Ẩn button nếu chưa đăng nhập HOẶC là Admin
  if (!isAuthenticated || role === "Admin") return null;

  return (
    <button
      onClick={toggleWishlist}
      disabled={loading}
      className="btn ghost small"
      style={{
        padding: "6px 12px",
        display: "flex",
        alignItems: "center",
        gap: "4px",
        color: inWishlist ? "#ef4444" : "#6b7280",
        border: inWishlist ? "1px solid #ef4444" : "1px solid #e5e7eb",
      }}
      title={inWishlist ? "Xóa khỏi yêu thích" : "Thêm vào yêu thích"}
    >
      <Heart
        size={16}
        fill={inWishlist ? "#ef4444" : "none"}
        stroke={inWishlist ? "#ef4444" : "currentColor"}
      />
      {loading ? "..." : inWishlist ? "Đã thích" : "Yêu thích"}
    </button>
  );
}
