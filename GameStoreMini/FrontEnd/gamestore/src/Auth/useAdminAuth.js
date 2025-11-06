import { useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { isAuthenticated, getUserRole } from "../Auth/useAuth";
import { useToast } from "../Components/Toast";

export const useAdminAuth = () => {
  const navigate = useNavigate();
  const toast = useToast();

  useEffect(() => {
    const checkAdminPermission = () => {
      if (!isAuthenticated()) {
        toast.error("Vui lòng đăng nhập để tiếp tục");
        navigate("/login");
        return false;
      }

      if (getUserRole() !== "Admin") {
        toast.error("Bạn không có quyền truy cập vào khu vực này");
        navigate("/");
        return false;
      }

      return true;
    };

    checkAdminPermission();
  }, [navigate, toast]);

  return {
    isAdmin: isAuthenticated() && getUserRole() === "Admin",
  };
};
