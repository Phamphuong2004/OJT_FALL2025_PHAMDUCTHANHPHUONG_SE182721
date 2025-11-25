import React from "react";
import { render, screen, waitFor } from "@testing-library/react";
import userEvent from "@testing-library/user-event";
import CartProvider, { useCart } from "../CartProvider";

// Mock Cart API (module in src/API/CartAPI.js)
vi.mock("../../API/CartAPI", () => {
  const mock = {
    getCart: vi.fn(() => Promise.resolve({ items: [] })),
    addToCart: vi.fn((p) =>
      Promise.resolve({
        items: [
          {
            gameId: 1,
            quantity: p.quantity ?? p.qty ?? 1,
            game: { title: "G", price: 10 },
          },
        ],
      })
    ),
    removeFromCart: vi.fn(() => Promise.resolve({ items: [] })),
    clearCart: vi.fn(() => Promise.resolve({})),
  };
  return { default: mock, ...mock };
});

// Mock toast
vi.mock("../../Components/Toast", () => ({
  useToast: () => ({ success: vi.fn(), error: vi.fn() }),
}));

// Mock signalr to avoid network attempts
vi.mock("@microsoft/signalr", () => ({
  HubConnectionBuilder: function () {
    return {
      withUrl() {
        return this;
      },
      withAutomaticReconnect() {
        return this;
      },
      configureLogging() {
        return this;
      },
      build() {
        return {
          on: () => {},
          onclose: () => {},
          start: async () => {},
          invoke: async () => {},
          stop: async () => {},
        };
      },
    };
  },
}));

// Consumer helper to expose context for assertions
function Consumer() {
  const { items, count, addToCart } = useCart();
  return (
    <div>
      <div data-testid="count">{count}</div>
      <button onClick={() => addToCart({ gameId: 1, qty: 2 })}>Add</button>
      <ul>
        {items.map((i) => (
          <li key={i.gameId}>
            {i.gameId}:{i.qty}
          </li>
        ))}
      </ul>
    </div>
  );
}

describe("CartProvider", () => {
  test("adds item optimistically then syncs with API", async () => {
    const user = userEvent.setup();
    // prevent network negotiate fetch in SignalR
    globalThis.fetch = vi.fn(() =>
      Promise.resolve({ ok: true, text: async () => "" })
    );

    render(
      <CartProvider>
        <Consumer />
      </CartProvider>
    );

    // initial count should be 0
    expect(screen.getByTestId("count").textContent).toBe("0");

    const btn = screen.getByRole("button", { name: /add/i });
    await user.click(btn);

    // optimistic update should reflect immediately (qty 2)
    await waitFor(() =>
      expect(screen.getByTestId("count").textContent).toBe("2")
    );

    // after API resolves, items should remain
    await waitFor(() => expect(screen.getByText(/1:2/)).toBeInTheDocument());
  });
});
