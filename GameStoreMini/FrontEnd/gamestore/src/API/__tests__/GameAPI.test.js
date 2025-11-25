// Using globals provided by Vitest instead of explicit imports

// Mock ApiClient used by GameAPI — must be declared before importing GameAPI
vi.mock("../ApiClient", () => ({
  api: {
    interceptors: { request: { use: vi.fn() }, response: { use: vi.fn() } },
    get: vi.fn(() =>
      Promise.resolve({ data: { items: [{ id: 1, title: "G" }], total: 1 } })
    ),
    post: vi.fn(() => Promise.resolve({ data: {} })),
    put: vi.fn(() => Promise.resolve({ data: {} })),
    delete: vi.fn(() => Promise.resolve({ data: {} })),
  },
}));

describe("GameAPI", () => {
  it("fetches list with params", async () => {
    const mod = await import("../GameAPI");
    const GameAPI = mod.default || mod;
    const res = await GameAPI.getAll({ page: 2, pageSize: 5, q: "x" });
    expect(res.items).toHaveLength(1);
  });
});
