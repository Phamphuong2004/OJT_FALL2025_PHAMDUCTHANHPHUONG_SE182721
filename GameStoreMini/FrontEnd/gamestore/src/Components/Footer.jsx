import React from "react";
import { Link } from "react-router-dom";
import "../Decorate/Footer.css";

export default function Footer() {
  return (
    <footer className="footer">
      <div className="footer-container">
        {/* Brand Section */}
        <div className="footer-brand">
          <Link to="/" className="footer-logo">
            <div className="footer-logo-icon">🎮</div>
            <span className="footer-logo-text">ĐAM MÊ GAME</span>
          </Link>
          <p className="footer-description">
            Nền tảng mua sắm game hàng đầu Việt Nam. Khám phá hàng ngàn game
            chất lượng với giá tốt nhất.
          </p>
          <div className="footer-copyright">
            © {new Date().getFullYear()} ĐAMMEGAME. All rights reserved.
          </div>
        </div>

        {/* Quick Links */}
        <nav className="footer-nav">
          <h3 className="footer-nav-title">Liên kết nhanh</h3>
          <div className="footer-nav-links">
            <Link to="/" className="footer-nav-link">
              Trang chủ
            </Link>
            <Link to="/store" className="footer-nav-link">
              Games
            </Link>
            <Link to="/categories" className="footer-nav-link">
              Thể loại
            </Link>
            <Link to="/promotions" className="footer-nav-link">
              Khuyến mãi
            </Link>
            <Link to="/about" className="footer-nav-link">
              Về chúng tôi
            </Link>
          </div>
        </nav>

        {/* Support Links */}
        <nav className="footer-nav">
          <h3 className="footer-nav-title">Hỗ trợ</h3>
          <div className="footer-nav-links">
            <Link to="/help" className="footer-nav-link">
              Trung tâm trợ giúp
            </Link>
            <Link to="/contact" className="footer-nav-link">
              Liên hệ
            </Link>
            <Link to="/terms" className="footer-nav-link">
              Điều khoản
            </Link>
            <Link to="/privacy" className="footer-nav-link">
              Chính sách bảo mật
            </Link>
          </div>
        </nav>

        {/* Social Media */}
        <div className="footer-social">
          <h3 className="footer-social-title">Kết nối với chúng tôi</h3>
          <div className="footer-social-links">
            <a
              href="https://facebook.com"
              target="_blank"
              rel="noopener noreferrer"
              className="footer-social-link"
              aria-label="Facebook"
              title="Facebook"
            >
              📘
            </a>
            <a
              href="https://twitter.com"
              target="_blank"
              rel="noopener noreferrer"
              className="footer-social-link"
              aria-label="Twitter"
              title="Twitter"
            >
              🐦
            </a>
            <a
              href="https://discord.com"
              target="_blank"
              rel="noopener noreferrer"
              className="footer-social-link"
              aria-label="Discord"
              title="Discord"
            >
              💬
            </a>
            <a
              href="https://instagram.com"
              target="_blank"
              rel="noopener noreferrer"
              className="footer-social-link"
              aria-label="Instagram"
              title="Instagram"
            >
              📸
            </a>
            <a
              href="https://youtube.com"
              target="_blank"
              rel="noopener noreferrer"
              className="footer-social-link"
              aria-label="YouTube"
              title="YouTube"
            >
              📺
            </a>
          </div>
        </div>
      </div>

      {/* Bottom Bar */}
      <div className="footer-bottom">
        <div className="footer-bottom-text">Made with ❤️ in Vietnam</div>
        <div className="footer-bottom-links">
          <Link to="/terms" className="footer-bottom-link">
            Điều khoản sử dụng
          </Link>
          <Link to="/privacy" className="footer-bottom-link">
            Chính sách bảo mật
          </Link>
          <Link to="/cookies" className="footer-bottom-link">
            Cookie
          </Link>
        </div>
      </div>
    </footer>
  );
}
