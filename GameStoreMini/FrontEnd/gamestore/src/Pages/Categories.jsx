import React, { useEffect, useMemo, useRef, useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import CategoryAPI from "../API/CategoryAPI";

export default function Categories() {
  const [categories, setCategories] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [query, setQuery] = useState("");
  const [suggestionsVisible, setSuggestionsVisible] = useState(false);
  const [activeIndex, setActiveIndex] = useState(-1);
  const inputRef = useRef(null);
  const suggestionsRef = useRef(null);
  const debounceRef = useRef(null);
  const navigate = useNavigate();

  useEffect(() => {
    let mounted = true;
    (async () => {
      setLoading(true);
      setError(null);
      try {
        const res = await CategoryAPI.getAll();
        const list = Array.isArray(res) ? res : res?.items ?? res?.data ?? [];
        if (mounted) setCategories(list);
      } catch (err) {
        console.error("[Categories] API error:", err);
        const status = err.response?.status;
        const respData = err.response?.data;
        const reqUrl = err.config?.url || "";
        const message = `Request failed: ${status ?? "no-response"} - ${
          err.message
        }`;
        if (mounted) setError({ message, status, respData, reqUrl });
      } finally {
        if (mounted) setLoading(false);
      }
    })();
    return () => {
      mounted = false;
    };
  }, []); // eslint-disable-line

  const filtered = useMemo(() => {
    const q = (query || "").trim().toLowerCase();
    if (!q) return categories;
    return categories.filter((c) => (c.name || "").toLowerCase().includes(q));
  }, [categories, query]);

  const suggestions = filtered.slice(0, 8);

  useEffect(() => {
    const onDocClick = (e) => {
      if (
        !inputRef.current?.contains(e.target) &&
        !suggestionsRef.current?.contains(e.target)
      ) {
        setSuggestionsVisible(false);
        setActiveIndex(-1);
      }
    };
    document.addEventListener("click", onDocClick);
    return () => document.removeEventListener("click", onDocClick);
  }, []);

  const openCategory = (c) => {
    // navigate to store with categoryId
    const id = encodeURIComponent(c.id);
    navigate(`/store?categoryId=${id}`);
    setSuggestionsVisible(false);
    setActiveIndex(-1);
  };

  const onInputChange = (value) => {
    setQuery(value);
    setSuggestionsVisible(true);
    setActiveIndex(-1);
    if (debounceRef.current) clearTimeout(debounceRef.current);
    // small debounce to mimic remote suggest
    debounceRef.current = setTimeout(() => {
      // nothing extra for now — suggestions are from local list
    }, 120);
  };

  const onKeyDown = (e) => {
    if (!suggestionsVisible) return;
    if (e.key === "ArrowDown") {
      e.preventDefault();
      setActiveIndex((i) => Math.min(i + 1, suggestions.length - 1));
    } else if (e.key === "ArrowUp") {
      e.preventDefault();
      setActiveIndex((i) => Math.max(i - 1, 0));
    } else if (e.key === "Enter") {
      e.preventDefault();
      if (activeIndex >= 0 && activeIndex < suggestions.length) {
        openCategory(suggestions[activeIndex]);
      }
    } else if (e.key === "Escape") {
      setSuggestionsVisible(false);
      setActiveIndex(-1);
    }
  };

  if (loading)
    return (
      <main className="page-container">
        <div>Đang tải thể loại...</div>
      </main>
    );
  if (error) {
    return (
      <main className="page-container">
        <h1>Lỗi tải thể loại</h1>
        <div style={{ color: "#b91c1c" }}>{error.message}</div>
      </main>
    );
  }

  return (
    <main className="page-container categories-page">
      <div className="page-hero">
        <h1>Thể loại</h1>
        <div style={{ minWidth: 260, position: "relative" }}>
          <input
            ref={inputRef}
            aria-label="Tìm thể loại"
            className="category-search"
            placeholder="Tìm thể loại..."
            value={query}
            onChange={(e) => onInputChange(e.target.value)}
            onKeyDown={onKeyDown}
            onFocus={() => setSuggestionsVisible(true)}
            style={{ width: "100%" }}
            autoComplete="off"
          />

          {suggestionsVisible && suggestions.length > 0 && (
            <div
              ref={suggestionsRef}
              className="search-suggestions"
              role="listbox"
            >
              {suggestions.map((s, idx) => {
                const name = s.name || "";
                const q = (query || "").trim();
                // highlight match
                const regex = new RegExp(
                  `(${q.replace(/[-\\/\\^$*+?.()|[\]{}]/g, "\\$&")})`,
                  "ig"
                );
                const parts = q ? name.split(regex) : [name];
                const isActive = idx === activeIndex;
                return (
                  <div
                    key={s.id}
                    role="option"
                    aria-selected={isActive}
                    className={`suggestion-item ${isActive ? "active" : ""}`}
                    onMouseDown={(ev) =>
                      ev.preventDefault()
                    } /* prevent input blur */
                    onClick={() => openCategory(s)}
                  >
                    <div className="suggestion-name">
                      {parts.map((p, i) =>
                        q && p.toLowerCase() === q.toLowerCase() ? (
                          <mark key={i}>{p}</mark>
                        ) : (
                          <span key={i}>{p}</span>
                        )
                      )}
                    </div>
                    {s.description ? (
                      <div className="suggestion-desc">{s.description}</div>
                    ) : null}
                  </div>
                );
              })}
            </div>
          )}
        </div>
      </div>

      {filtered.length === 0 ? (
        <div className="empty-state">Không tìm thấy thể loại nào.</div>
      ) : (
        <div className="category-list">
          {filtered.map((c) => (
            <Link
              key={c.id}
              to={`/store?categoryId=${encodeURIComponent(c.id)}`}
              className="category-card"
            >
              <div className="category-name">{c.name}</div>
              {c.description ? (
                <div className="category-desc">{c.description}</div>
              ) : null}
            </Link>
          ))}
        </div>
      )}
    </main>
  );
}
