# GameStoreMini - Giới thiệu & Hướng dẫn sử dụng

## 📖 Tổng quan

**GameStoreMini** là một ứng dụng web thương mại điện tử chuyên về bán game, được xây dựng với kiến trúc fullstack hiện đại. Dự án bao gồm:

- **Backend**: ASP.NET Core Web API với Entity Framework Core
- **Frontend**: React SPA với Vite
- **Real-time**: SignalR cho cập nhật giỏ hàng tức thời
- **Database**: SQL Server / PostgreSQL (tùy cấu hình)

## 🎯 Mục đích dự án

- Cung cấp nền tảng mua bán game trực tuyến
- Quản lý kho game, đơn hàng, người dùng
- Hệ thống đánh giá và khuyến mãi
- Admin dashboard để quản trị

## ✨ Tính năng chính

### Người dùng (Customer)

- ✅ Đăng ký, đăng nhập (JWT authentication)
- ✅ Xem danh sách game (tìm kiếm, lọc, phân trang)
- ✅ Thêm game vào giỏ hàng (real-time sync qua SignalR)
- ✅ Quản lý giỏ hàng (thêm/xóa/cập nhật số lượng)
- ✅ Đặt hàng và thanh toán
- ✅ Xem lịch sử đơn hàng
- ✅ Đánh giá game (chỉ sau khi mua)
- ✅ Danh sách yêu thích (Wishlist)
- ✅ Quản lý địa chỉ giao hàng
- ✅ Xem và claim khuyến mãi

### Quản trị viên (Admin)

- ✅ Quản lý game (CRUD, upload ảnh)
- ✅ Quản lý đơn hàng (xem, cập nhật trạng thái)
- ✅ Quản lý người dùng (xem, phân quyền, khóa/mở)
- ✅ Quản lý đánh giá (duyệt, xóa)
- ✅ Tạo và quản lý khuyến mãi
- ✅ Xem thống kê, báo cáo

### Khách (Guest)

- ✅ Xem danh sách game
- ✅ Xem chi tiết game
- ✅ Xem đánh giá
- ❌ Không thể mua hàng, đánh giá (cần đăng nhập)

## 👥 Phân quyền hệ thống

| Chức năng        | Guest | Customer    | Admin  |
| ---------------- | ----- | ----------- | ------ |
| Xem game         | ✅    | ✅          | ✅     |
| Mua hàng         | ❌    | ✅          | ✅     |
| Đánh giá         | ❌    | ✅ (đã mua) | ✅     |
| Quản lý game     | ❌    | ❌          | ✅     |
| Quản lý user     | ❌    | ❌          | ✅     |
| Quản lý đơn hàng | ❌    | Của mình    | Tất cả |

## 🚀 Hướng dẫn cài đặt nhanh

### Yêu cầu hệ thống

- .NET SDK 8.0+
- Node.js 18+ và npm
- SQL Server hoặc PostgreSQL (hoặc dùng InMemory cho dev/test)

### Bước 1: Clone repository

```bash
git clone <repository-url>
cd GameStoreMini
```

### Bước 2: Chạy Backend

```cmd
cd BackEnd\Game_store
dotnet restore
dotnet ef database update
dotnet run
```

Backend sẽ chạy tại `https://localhost:7000` (hoặc cổng cấu hình trong `launchSettings.json`).

### Bước 3: Chạy Frontend

```cmd
cd FrontEnd\gamestore
npm ci
npm run dev
```

Frontend sẽ chạy tại `http://localhost:5173`.

### Bước 4: Truy cập ứng dụng

- Mở trình duyệt tại `http://localhost:5173`
- Đăng ký tài khoản mới hoặc dùng tài khoản admin mặc định (nếu đã seed)

## 📱 Hướng dẫn sử dụng

### Đối với khách hàng

1. **Đăng ký tài khoản**

   - Click "Đăng ký" trên thanh navigation
   - Điền thông tin: username, email, password
   - Xác nhận email (nếu được cấu hình)

2. **Tìm kiếm game**

   - Sử dụng thanh tìm kiếm ở đầu trang
   - Lọc theo danh mục, giá, đánh giá
   - Sắp xếp theo tên, giá, mới nhất

3. **Mua game**

   - Click vào game để xem chi tiết
   - Nhấn "Thêm vào giỏ hàng"
   - Vào giỏ hàng, kiểm tra và điều chỉnh số lượng
   - Nhấn "Thanh toán"
   - Chọn địa chỉ giao hàng, phương thức thanh toán
   - Xác nhận đơn hàng

4. **Đánh giá game**

   - Sau khi mua và nhận game
   - Vào trang chi tiết game
   - Viết đánh giá và chọn số sao (1-5)
   - Gửi đánh giá

5. **Quản lý tài khoản**
   - Xem lịch sử đơn hàng
   - Cập nhật thông tin cá nhân
   - Quản lý địa chỉ giao hàng
   - Xem danh sách yêu thích

### Đối với Admin

1. **Đăng nhập admin**

   - Sử dụng tài khoản có role "Admin"
   - Truy cập Admin Dashboard

2. **Quản lý game**

   - Thêm game mới: điền thông tin, upload ảnh
   - Chỉnh sửa game: cập nhật giá, mô tả, stock
   - Xóa game (cẩn thận với game đã có đơn hàng)
   - Phân loại game theo category

3. **Quản lý đơn hàng**

   - Xem danh sách đơn hàng
   - Cập nhật trạng thái: Pending → Processing → Shipped → Delivered
   - Xem chi tiết đơn hàng và thông tin khách hàng

4. **Quản lý người dùng**

   - Xem danh sách users
   - Thay đổi role (Customer ↔ Admin)
   - Khóa/mở khóa tài khoản

5. **Quản lý đánh giá**

   - Xem tất cả reviews
   - Xóa review vi phạm
   - Xem thống kê rating

6. **Tạo khuyến mãi**
   - Tạo promotion code
   - Đặt giá trị giảm giá (% hoặc số tiền cố định)
   - Đặt thời hạn và số lượng sử dụng

## 🔧 Cấu hình

### Backend (`appsettings.json`)

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=...;Database=GameStore;..."
  },
  "Jwt": {
    "Key": "your-secret-key",
    "Issuer": "GameStoreMini",
    "Audience": "GameStoreMini"
  }
}
```

### Frontend (`.env`)

```env
VITE_API_BASE=/api
VITE_API_HUB=/hubs
```

## 📞 Hỗ trợ

- Xem thêm tài liệu kỹ thuật tại `Technologies.md`
- Xem hướng dẫn test tại `README.md`
- Báo lỗi: tạo issue trên GitHub

## 📝 Ghi chú

- Dự án này là demo/learning project
- Không sử dụng trong production mà không kiểm tra bảo mật kỹ lưỡng
- Một số tính năng có thể chưa hoàn chỉnh

---

_Cập nhật: 2025-12-04_
