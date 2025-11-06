import React from "react";
import Banner from "../Components/Banner";
import Footer from "../Components/Footer";

export default function Home() {
  return (
    <>
      <Banner />
      <main style={{ padding: 20, maxWidth: 1100, margin: "0 auto" }}>
        <h1>Chào mừng đến với cửa hàng game</h1>
        <p>Nơi bạn có thể tìm kiếm các thể loại game mà bạn muốn </p>
      </main>
      <Footer />
    </>
  );
}
