import React, { createContext, useContext, useState, useCallback } from "react";
import "../Decorate/toast.css";

const ToastContext = createContext(null);

let idCounter = 1;

export default function ToastProvider({ children }) {
  const [toasts, setToasts] = useState([]);

  const push = useCallback((type, message, ttl = 3500) => {
    const id = idCounter++;
    setToasts((t) => [...t, { id, type, message }]);
    if (ttl > 0) {
      setTimeout(() => setToasts((t) => t.filter((x) => x.id !== id)), ttl);
    }
    return id;
  }, []);

  const remove = useCallback(
    (id) => setToasts((t) => t.filter((x) => x.id !== id)),
    []
  );

  const api = {
    success: (msg, ttl) => push("success", msg, ttl),
    error: (msg, ttl) => push("error", msg, ttl),
    info: (msg, ttl) => push("info", msg, ttl),
    remove,
  };

  return (
    <ToastContext.Provider value={api}>
      {children}
      <div className="gs-toast-root" aria-live="polite">
        {toasts.map((t) => (
          <div key={t.id} className={`gs-toast ${t.type}`}>
            <div className="gs-toast-msg">{t.message}</div>
            <button className="gs-toast-close" onClick={() => remove(t.id)}>
              ×
            </button>
          </div>
        ))}
      </div>
    </ToastContext.Provider>
  );
}

export const useToast = () => {
  const ctx = useContext(ToastContext);
  if (!ctx)
    return {
      success: () => {},
      error: () => {},
      info: () => {},
      remove: () => {},
    };
  return ctx;
};
