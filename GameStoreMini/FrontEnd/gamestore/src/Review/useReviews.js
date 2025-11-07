import { useState, useEffect } from "react";
import reviewService from "../API/ReviewAPI";
import { getToken, decodeToken } from "../Auth/useAuth";

export const useReviews = (gameId, initialFilters = {}) => {
  const token = getToken();
  const userData = token ? decodeToken(token) : null;
  const userId = userData ? parseInt(userData.nameid || userData.sub) : null;

  const [reviews, setReviews] = useState([]);
  const [statistics, setStatistics] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [pagination, setPagination] = useState({
    page: 1,
    pageSize: 10,
    totalCount: 0,
    totalPages: 0,
  });

  const [filters, setFilters] = useState({
    gameId,
    sortBy: "date",
    sortOrder: "desc",
    page: 1,
    pageSize: 10,
    ...initialFilters,
  });

  // Fetch reviews
  const fetchReviews = async () => {
    try {
      setLoading(true);
      console.log("Fetching reviews with filters:", filters);
      const data = await reviewService.getReviews(filters);

      console.log("Reviews fetched successfully:", data);
      setReviews(data.data);
      setPagination({
        page: data.page,
        pageSize: data.pageSize,
        totalCount: data.totalCount,
        totalPages: data.totalPages,
      });
      setError(null);
    } catch (err) {
      console.error("❌ Error fetching reviews:", err); // Debug log
      console.error("Error details:", {
        message: err.message,
        response: err.response,
        request: err.request,
        config: err.config,
      });

      setError(err.response?.data?.message || "Không thể tải reviews");
    } finally {
      setLoading(false);
    }
  };

  // Fetch statistics
  const fetchStatistics = async () => {
    if (!gameId) return;

    try {
      const data = await reviewService.getGameRatingStatistics(gameId);
      setStatistics(data);
    } catch (err) {
      console.error("Không thể tải thống kê:", err);
    }
  };

  useEffect(() => {
    fetchReviews();
    if (gameId) {
      fetchStatistics();
    }
  }, [filters]);

  // Tạo review mới
  const createReview = async (reviewData) => {
    if (!token) throw new Error("Vui lòng đăng nhập");

    try {
      const newReview = await reviewService.createReview(reviewData, token);
      await fetchReviews();
      await fetchStatistics();
      return newReview;
    } catch (err) {
      const message = err.response?.data?.message || "Không thể tạo review";
      throw new Error(message);
    }
  };

  // Cập nhật review
  const updateReview = async (id, reviewData) => {
    if (!token) throw new Error("Vui lòng đăng nhập");

    try {
      const updatedReview = await reviewService.updateReview(
        id,
        reviewData,
        token
      );
      await fetchReviews();
      await fetchStatistics();
      return updatedReview;
    } catch (err) {
      const message =
        err.response?.data?.message || "Không thể cập nhật review";
      throw new Error(message);
    }
  };

  // Xóa review
  const deleteReview = async (id) => {
    if (!token) throw new Error("Vui lòng đăng nhập");

    try {
      await reviewService.deleteReview(id, token);
      await fetchReviews();
      await fetchStatistics();
    } catch (err) {
      const message = err.response?.data?.message || "Không thể xóa review";
      throw new Error(message);
    }
  };

  // Toggle helpful
  const toggleHelpful = async (id) => {
    if (!token) throw new Error("Vui lòng đăng nhập");

    try {
      await reviewService.toggleHelpful(id, token);
      await fetchReviews();
    } catch (err) {
      const message = err.response?.data?.message || "Có lỗi xảy ra";
      throw new Error(message);
    }
  };

  // Thay đổi filter
  const updateFilters = (newFilters) => {
    setFilters((prev) => ({ ...prev, ...newFilters, page: 1 }));
  };

  // Chuyển trang
  const goToPage = (page) => {
    setFilters((prev) => ({ ...prev, page }));
  };

  return {
    reviews,
    statistics,
    loading,
    error,
    pagination,
    filters,
    userId,
    isAuthenticated: !!token,
    createReview,
    updateReview,
    deleteReview,
    toggleHelpful,
    updateFilters,
    goToPage,
    refreshReviews: fetchReviews,
  };
};
