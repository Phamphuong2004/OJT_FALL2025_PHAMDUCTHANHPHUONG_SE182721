import React from "react";
import "../Decorate/Pagination.css";

export default function Pagination({ currentPage, totalPages, onPageChange }) {
  if (!totalPages || totalPages <= 1) return null;

  const delta = 2;

  const makeRange = () => {
    const range = [];
    const left = Math.max(1, currentPage - delta);
    const right = Math.min(totalPages, currentPage + delta);

    range.push(1);
    if (left > 2) range.push("left-ellipsis");
    for (let i = Math.max(2, left); i <= Math.min(totalPages - 1, right); i++)
      range.push(i);
    if (right < totalPages - 1) range.push("right-ellipsis");
    if (totalPages > 1) range.push(totalPages);
    return [...new Set(range)];
  };

  const items = makeRange();

  return (
    <nav className="pagination" aria-label="Pagination">
      <div
        role="button"
        className={`page-item ${currentPage === 1 ? "disabled" : ""}`}
        onClick={() => currentPage > 1 && onPageChange(currentPage - 1)}
      >
        Prev
      </div>

      {items.map((it, idx) =>
        it === "left-ellipsis" || it === "right-ellipsis" ? (
          <div key={it + idx} className="page-item disabled">
            …
          </div>
        ) : (
          <div
            key={it}
            role="button"
            className={`page-item ${it === currentPage ? "active" : ""}`}
            onClick={() => it !== currentPage && onPageChange(it)}
          >
            {it}
          </div>
        )
      )}

      <div
        role="button"
        className={`page-item ${currentPage === totalPages ? "disabled" : ""}`}
        onClick={() =>
          currentPage < totalPages && onPageChange(currentPage + 1)
        }
      >
        Next
      </div>
    </nav>
  );
}
