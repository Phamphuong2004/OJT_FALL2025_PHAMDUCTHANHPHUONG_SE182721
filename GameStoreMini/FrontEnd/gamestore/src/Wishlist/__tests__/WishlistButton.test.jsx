import React from "react";
import { render, screen, waitFor } from "@testing-library/react";
import userEvent from "@testing-library/user-event";
import WishlistButton from "../WishlistButton";

// Mock modules used by the component
vi.mock("../../API/WishlistAPI", () => ({
  default: {
    checkInWishlist: vi.fn(() => Promise.resolve({ inWishlist: true })),
    addToWishlist: vi.fn(() => Promise.resolve()),
    removeFromWishlist: vi.fn(() => Promise.resolve()),
  },
}));

vi.mock("../../Auth/useAuth", () => ({
  getUserRole: () => "User",
}));

vi.mock("../../Components/Toast", () => ({
  useToast: () => ({ success: vi.fn(), error: vi.fn() }),
}));

describe("WishlistButton", () => {
  beforeEach(() => {
    // provide token so button is visible
    localStorage.setItem("token", "token-value");
  });

  afterEach(() => {
    localStorage.clear();
    vi.clearAllMocks();
  });

  test("shows initial in-wishlist state and toggles", async () => {
    const user = userEvent.setup();
    render(<WishlistButton gameId={1} />);

    // wait for effect that checks wishlist status
    await waitFor(() => expect(screen.getByRole("button")).toBeInTheDocument());

    const btn = screen.getByRole("button");
    expect(btn).toHaveTextContent("Đã thích");

    // click to remove from wishlist
    await user.click(btn);

    // after click, text should change to 'Yêu thích' (removed)
    await waitFor(() => expect(btn).toHaveTextContent("Yêu thích"));
  });
});
