import React, { Suspense } from "react";
import {
  BrowserRouter,
  Routes,
  Route,
  Navigate,
  useNavigate,
  Outlet,
} from "react-router-dom";
import Navbar from "../Components/Navbar";
import { CartProvider, useCart } from "../Cart/CartProvider";
import ToastProvider from "../Components/Toast";
import { getUserRole, isAuthenticated } from "../Auth/useAuth";

// Small, reusable loading UI
const Loading = ({ message = "Loading..." }) => (
  <div style={{ padding: 20 }}>{message}</div>
);

// Helper that lazily imports a module and falls back to a tiny component if the import fails.
const lazyPage = (importFn, fallbackText) =>
  React.lazy(() =>
    importFn().catch(() => ({
      default: () => <Loading message={fallbackText} />,
    }))
  );

const Home = lazyPage(() => import("../Pages/Home"), "Home (placeholder)");
const Store = lazyPage(() => import("../Pages/Store"), "Store (placeholder)");
const Categories = lazyPage(
  () => import("../Pages/Categories"),
  "Categories (placeholder)"
);
const Cart = lazyPage(() => import("../Pages/Cart"), "Cart (placeholder)");
const Login = lazyPage(() => import("../Form/Login"), "Login (placeholder)");
const Register = lazyPage(
  () => import("../Form/Register"),
  "Register (placeholder)"
);
const ForgotPassword = lazyPage(
  () => import("../Form/ForgotPassword"),
  "Forgot Password (placeholder)"
);
const AddGame = lazyPage(
  () => import("../Games/AddGame"),
  "AddGame (placeholder)"
);
const About = lazyPage(() => import("../Pages/About"), "About (placeholder)");
const Contact = lazyPage(
  () => import("../Pages/Contact"),
  "Contact (placeholder)"
);
const Checkout = lazyPage(
  () => import("../Components/Checkout"),
  "Checkout (placeholder)"
);
const OrderConfirmation = lazyPage(
  () => import("../Order/OrderConfirmation"),
  "OrderConfirmation (placeholder)"
);

// add GameDetails lazy import
const GameDetails = lazyPage(
  () => import("../GameDetails/GameDetails"),
  "GameDetails (placeholder)"
);

const Payment = lazyPage(
  () => import("../Payment/Payment"),
  "Payment (placeholder)"
);
const PromotionList = lazyPage(
  () => import("../Discount/PromotionList"),
  "PromotionList (placeholder)"
);
const PromotionDetail = lazyPage(
  () => import("../Discount/PromotionDetail"),
  "PromotionDetail (placeholder)"
);
const Account = lazyPage(
  () => import("../Pages/Account"),
  "Account (placeholder)"
);
const AdminPromotionDashboard = lazyPage(
  () => import("../Admin/PromotionDashboard"),
  "PromotionDashboard (placeholder)"
);
const UsersList = lazyPage(() => import("../Admin/UsersList"), "UsersList");
const CreatePromotion = lazyPage(
  () => import("../Admin/CreatePromotion"),
  "CreatePromotion (placeholder)"
);
const EditPromotion = lazyPage(
  () => import("../Admin/EditPromotion"),
  "EditPromotion (placeholder)"
);

const OrderSuccessPage = lazyPage(
  () => import("../Order/OrderSuccessPage"),
  "Order Success Page (placeholder)"
);
const OrderTracking = lazyPage(
  () => import("../Order/OrderTracking"),
  "Order Tracking (placeholder)"
);
const Wishlist = lazyPage(
  () => import("../Wishlist/Wishlist"),
  "Wishlist (placeholder)"
);
const ViewHistory = lazyPage(
  () => import("../ViewHistory/ViewHistory"),
  "View History (placeholder)"
);
const AdminReviewDashboard = lazyPage(
  () => import("../Admin/AdminReviewDashboard"),
  "Admin Review Dashboard (placeholder)"
);

// Layout component that keeps Navbar persistent and provides search/navigation behavior
function Layout() {
  const navigate = useNavigate();
  const { items = [] } = useCart() || {};

  const cartCount = items.reduce((sum, it) => sum + (it.qty || 0), 0);

  const handleSearch = (q) => {
    const params = new URLSearchParams();
    if (q) params.set("q", q);
    navigate(`/store?${params.toString()}`);
  };

  return (
    <>
      <Navbar cartCount={cartCount} onSearch={handleSearch} />
      <Suspense fallback={<Loading />}>
        <Outlet />
      </Suspense>
    </>
  );
}

export default function AppRoutes() {
  return (
    <BrowserRouter>
      <ToastProvider>
        <CartProvider>
          <Routes>
            <Route element={<Layout />}>
              <Route index element={<Home />} />
              <Route path="store" element={<Store />} />
              <Route path="games/:id" element={<GameDetails />} />
              <Route path="categories" element={<Categories />} />
              <Route path="cart" element={<Cart />} />
              <Route path="payment" element={<Payment />} />
              {/* Admin-only route: wrap with RequireRole */}
              <Route
                path="admin/add-game"
                element={
                  <RequireRole role="Admin">
                    <AddGame />
                  </RequireRole>
                }
              />
              <Route path="login" element={<Login />} />
              <Route path="register" element={<Register />} />
              <Route path="forgot-password" element={<ForgotPassword />} />
              <Route path="about" element={<About />} />
              <Route path="contact" element={<Contact />} />
              <Route path="checkout" element={<Checkout />} />
              <Route path="order/:id" element={<OrderConfirmation />} />
              <Route path="promotions" element={<PromotionList />} />
              <Route path="promotions/:slug" element={<PromotionDetail />} />
              <Route path="account" element={<Account />} />

              {/* Customer-only routes - Admin không truy cập được */}
              <Route
                path="wishlist"
                element={
                  <RequireCustomer>
                    <Wishlist />
                  </RequireCustomer>
                }
              />
              <Route
                path="history"
                element={
                  <RequireCustomer>
                    <ViewHistory />
                  </RequireCustomer>
                }
              />

              {/* Admin promotion routes */}
              <Route
                path="admin/promotion"
                element={
                  <RequireRole role="Admin">
                    <AdminPromotionDashboard />
                  </RequireRole>
                }
              />
              <Route
                path="admin/promotion/create"
                element={
                  <RequireRole role="Admin">
                    <CreatePromotion />
                  </RequireRole>
                }
              />
              <Route
                path="admin/promotion/:id/edit"
                element={
                  <RequireRole role="Admin">
                    <EditPromotion />
                  </RequireRole>
                }
              />
              {/* Admin users route */}
              <Route
                path="admin/users"
                element={
                  <RequireRole role="Admin">
                    <UsersList />
                  </RequireRole>
                }
              />
              {/* Admin reviews route */}
              <Route
                path="admin/reviews"
                element={
                  <RequireRole role="Admin">
                    <AdminReviewDashboard />
                  </RequireRole>
                }
              />
            </Route>

            <Route path="old-home" element={<Navigate to="/" replace />} />
            <Route
              path="*"
              element={<div style={{ padding: 20 }}>404 — Page not found</div>}
            />
            <Route path="/order/success" element={<OrderSuccessPage />} />
            <Route path="/orders/track" element={<OrderTracking />} />
          </Routes>
        </CartProvider>
      </ToastProvider>
    </BrowserRouter>
  );
}

// Small wrapper component to protect routes by role
function RequireRole({ role, children }) {
  // if not authenticated, redirect to login
  if (!isAuthenticated()) return <Navigate to="/login" replace />;
  const userRole = getUserRole();
  if (!userRole) return <Navigate to="/login" replace />;
  if (userRole !== role)
    return <div style={{ padding: 20 }}>403 — Access denied</div>;
  return children;
}

// Wrapper component để chỉ cho phép Customer (không cho Admin)
function RequireCustomer({ children }) {
  if (!isAuthenticated()) return <Navigate to="/login" replace />;
  const userRole = getUserRole();
  if (!userRole) return <Navigate to="/login" replace />;
  if (userRole === "Admin")
    return (
      <div style={{ padding: 40, textAlign: "center" }}>
        <h3>403 — Tính năng này chỉ dành cho khách hàng</h3>
        <p style={{ color: "#6b7280", marginTop: 8 }}>
          Admin không cần wishlist và lịch sử xem
        </p>
      </div>
    );
  return children;
}
