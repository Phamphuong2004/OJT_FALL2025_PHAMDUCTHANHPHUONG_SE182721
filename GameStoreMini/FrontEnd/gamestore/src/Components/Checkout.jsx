import React from "react";
import Payment from "../Payment/Payment";

export default function Checkout() {
  console.log("Checkout component rendered");

  try {
    return <Payment />;
  } catch (error) {
    console.error("Error rendering Payment:", error);
    return (
      <div style={{ padding: "20px", color: "red" }}>
        Error: {error.message}
      </div>
    );
  }
}
