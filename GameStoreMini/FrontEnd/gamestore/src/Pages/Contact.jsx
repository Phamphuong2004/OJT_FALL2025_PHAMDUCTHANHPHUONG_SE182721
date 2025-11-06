import React from "react";

export default function Contact() {
  return (
    <main style={{ padding: "2rem" }}>
      <h1>Liên hệ</h1>
      <p>
        Nếu bạn có câu hỏi, hãy gửi email tới:{" "}
        <a href="mailto:help@gamestoremini.local">help@gamestoremini.local</a>
      </p>
      <div style={{ marginTop: "1rem" }}>
        <strong>Địa chỉ:</strong>
        <div>123 Game St., Hanoi, Vietnam</div>
      </div>
    </main>
  );
}
