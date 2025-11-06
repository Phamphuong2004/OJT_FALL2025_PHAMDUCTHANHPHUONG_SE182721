import React from "react";
import { createRoot } from "react-dom/client";
import App from "./App";
import "./Decorate/Pagination.css"; // <- global import
import { refreshToken } from "./API/UserAPI";

(async function startApp() {
  // cố gắng lấy access token bằng refresh cookie (nếu có)
  await refreshToken();
  // tiếp tục mount React app...
})();
createRoot(document.getElementById("root")).render(<App />);
