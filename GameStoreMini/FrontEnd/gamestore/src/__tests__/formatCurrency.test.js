import formatCurrency from "../Utils/formatCurrency";

describe("formatCurrency", () => {
  test("formats number to VND by default", () => {
    const out = formatCurrency(1000000);
    expect(out).toMatch(/1\.000\.000/); // locale formatting uses dots in vi-VN
  });

  test("returns empty string for null/undefined", () => {
    expect(formatCurrency(null)).toBe("");
    expect(formatCurrency(undefined)).toBe("");
  });

  test("returns input string if not a number", () => {
    expect(formatCurrency("abc")).toBe("abc");
  });
});
