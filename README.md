# 🎮 GameStoreMini

> Nền tảng thương mại điện tử game trực tuyến với kiến trúc fullstack hiện đại

<div align="center">

![License](https://img.shields.io/badge/license-MIT-blue.svg)
![.NET](https://img.shields.io/badge/.NET-8.0-purple.svg)
![React](https://img.shields.io/badge/React-19-blue.svg)
![Status](https://img.shields.io/badge/status-active-success.svg)

</div>

---

## 📋 Mục lục

- [Giới thiệu dự án](#-giới-thiệu-dự-án)
- [Tổng quan](#-tổng-quan)
- [Tính năng chính](#-tính-năng-chính)
- [Demo & Screenshots](#-demo--screenshots)
- [Yêu cầu hệ thống](#-yêu-cầu-hệ-thống)
- [Cấu trúc dự án](#-cấu-trúc-dự-án)
- [Cài đặt & Chạy](#-cài-đặt--chạy)
- [Kiểm thử](#-kiểm-thử)
- [API Documentation](#-api-documentation)
- [Triển khai](#-triển-khai)
- [Kiến trúc kỹ thuật](#-kiến-trúc-kỹ-thuật)
- [Đóng góp](#-đóng-góp)
- [Liên hệ](#-liên-hệ)

---

## 🎯 Giới thiệu dự án

**GameStoreMini** là một dự án thương mại điện tử chuyên về bán game trực tuyến, được phát triển như một nền tảng học tập và demo cho các công nghệ web hiện đại. Dự án này minh họa việc xây dựng một ứng dụng fullstack hoàn chỉnh với các tính năng thực tế như quản lý giỏ hàng real-time, hệ thống đánh giá, khuyến mãi và quản trị viên.

### 🎓 Mục tiêu dự án

- **Học tập**: Demo các best practices trong phát triển web fullstack
- **Thực hành**: Áp dụng các pattern và kiến trúc hiện đại (Repository, DTO, Clean Architecture)
- **Portfolio**: Showcase kỹ năng ASP.NET Core + React cho developers
- **Mở rộng**: Nền tảng có thể phát triển thêm nhiều tính năng (payment gateway, recommendation engine...)

### 👥 Team & Role

Dự án phù hợp cho:

- **Backend Developer**: Xây dựng RESTful API, SignalR, Database design với EF Core
- **Frontend Developer**: React UI/UX, State management, Real-time integration
- **Full-stack Developer**: End-to-end features, Integration testing, CI/CD, Deployment

### 🎨 Công nghệ nổi bật

- ⚡ **Real-time Updates** với SignalR WebSocket (giỏ hàng đồng bộ tức thời)
- 🔐 **JWT Authentication** bảo mật với role-based authorization
- 🧪 **Comprehensive Testing** (Unit + Integration + Component tests)
- 📱 **Responsive Design** tương thích mọi thiết bị (mobile-first approach)
- 🚀 **Modern Stack** (.NET 8 + React 19 + Vite với HMR)
- 🗄️ **Database Migrations** với Entity Framework Core
- 📦 **Modular Architecture** dễ bảo trì và mở rộng

---

## 🌟 Tổng quan

GameStoreMini là một ứng dụng web fullstack gồm:

### Backend (ASP.NET Core)

```
Technology Stack:
├── Framework: ASP.NET Core 8.0 Web API
├── ORM: Entity Framework Core 8.0
├── Database: SQL Server / PostgreSQL (configurable)
├── Authentication: JWT (JSON Web Tokens)
├── Real-time: SignalR cho WebSocket communication
├── File Upload: IFormFile với validation
├── Testing: xUnit + Moq + WebApplicationFactory
└── Documentation: Swagger/OpenAPI
```

**Kiến trúc Backend:**

- **Controllers**: RESTful API endpoints (Games, Cart, Orders, Users, Reviews...)
- **Models**: Entity classes với annotations
- **DTOs**: Data Transfer Objects cho request/response
- **Services**: Business logic layer (CartService, EmailService...)
- **Data**: DbContext với Migrations
- **Hubs**: SignalR hubs (CartHub cho real-time updates)
- **Utils**: Helpers, Extensions, Middleware

### Frontend (React + Vite)

```
Technology Stack:
├── Framework: React 19 (functional components + hooks)
├── Build Tool: Vite 7 (fast HMR, optimized production builds)
├── HTTP Client: Axios với interceptors
├── Real-time: @microsoft/signalr client
├── Routing: React Router 7
├── Forms: Formik + Yup validation
├── UI: Custom components + modular CSS
├── State: Context API (AuthContext, CartContext)
├── Testing: Vitest + React Testing Library
└── Dev Tools: ESLint, Prettier
```

**Kiến trúc Frontend:**

- **Pages**: Home, Games, GameDetail, Cart, Checkout, Profile, Admin...
- **Components**: Reusable UI components (Button, Card, Modal...)
- **API**: Axios client với base configuration
- **Auth**: JWT handling, protected routes
- **Context**: Global state management
- **Hooks**: Custom hooks (useAuth, useCart, useToast...)
- **Utils**: Formatters, validators, helpers

### 🔄 Flow tổng quan

```
User Browser
    ↓
React Frontend (Vite Dev Server / Production Build)
    ↓ HTTP/HTTPS
ASP.NET Core API (REST + SignalR)
    ↓
Entity Framework Core
    ↓
SQL Server / PostgreSQL Database
```

### Đặc điểm nổi bật

✨ **Real-time Cart Updates**: Giỏ hàng tự động đồng bộ khi có thay đổi từ bất kỳ tab/device nào  
🔒 **Role-based Access**: Phân quyền Customer/Admin chi tiết với JWT claims  
💳 **Complete E-commerce Flow**: Browse → Add to Cart → Checkout → Payment → Order Tracking  
⭐ **Review System**: Đánh giá sản phẩm 5 sao với verified purchase badge  
🎁 **Promotion System**: Mã giảm giá linh hoạt (%, fixed amount, minimum order value)  
📊 **Admin Dashboard**: Quản lý toàn diện games, orders, users, reviews, promotions  
🧪 **Well-tested**: Unit tests, integration tests (InMemory DB), component tests  
🚀 **CI/CD Ready**: GitHub Actions workflow mẫu cho automated testing  
📱 **Mobile Responsive**: Layout tự động điều chỉnh theo màn hình  
🌐 **API Documentation**: Swagger UI để test và explore API

---

## ⚡ Tính năng chính

### 🛍️ Dành cho khách hàng (Customer Role)

| Tính năng                  | Mô tả                                                        | Status |
| -------------------------- | ------------------------------------------------------------ | ------ |
| 🔍 **Tìm kiếm & Lọc**      | Tìm game theo tên, category, giá, rating, sort by popularity | ✅     |
| 🎮 **Catalog**             | Browse games với pagination, lazy loading images             | ✅     |
| 📖 **Chi tiết Game**       | View description, screenshots, specs, reviews                | ✅     |
| 🛒 **Giỏ hàng thông minh** | Real-time sync qua SignalR, auto-save, quantity validation   | ✅     |
| ❤️ **Wishlist**            | Lưu game yêu thích, nhận thông báo giảm giá                  | ✅     |
| ⭐ **Đánh giá**            | Rating 1-5 sao + review text (chỉ sau khi mua và nhận hàng)  | ✅     |
| 📦 **Theo dõi đơn hàng**   | Real-time status: Pending → Processing → Shipped → Delivered | ✅     |
| 🎁 **Khuyến mãi**          | Nhập mã giảm giá khi checkout, validate conditions           | ✅     |
| 📍 **Địa chỉ giao hàng**   | Quản lý nhiều địa chỉ, set default                           | ✅     |
| 💳 **Thanh toán**          | Multiple payment methods (COD, Credit Card...)               | ✅     |
| 👤 **Tài khoản**           | Profile management, order history, view history              | ✅     |
| 🔔 **Thông báo**           | Email confirmations (order, shipping updates)                | ⏳     |

### 👨‍💼 Dành cho Admin (Admin Role)

| Tính năng                 | Mô tả                                                                | Status |
| ------------------------- | -------------------------------------------------------------------- | ------ |
| 🎮 **Quản lý Game**       | CRUD games, upload images, set stock/price, visibility               | ✅     |
| 📂 **Categories**         | Phân loại game (Action, RPG, Strategy...)                            | ✅     |
| 📋 **Quản lý Orders**     | View all orders, update status, filter by status/date, export CSV    | ✅     |
| 👥 **Quản lý Users**      | View users, search, change roles, lock/unlock accounts               | ✅     |
| 💬 **Kiểm duyệt Reviews** | View all reviews, moderate/delete inappropriate content              | ✅     |
| 🎟️ **Tạo Promotions**     | Mã giảm giá với điều kiện (%, fixed, minimum order, expiry date)     | ✅     |
| 📊 **Dashboard**          | Stats tổng quan: revenue, orders count, top games, recent activities | ✅     |
| 📈 **Analytics**          | Sales charts, revenue trends (by day/month)                          | ⏳     |
| 🔧 **Settings**           | Site configuration, email templates                                  | ⏳     |

**Legend**: ✅ Implemented | ⏳ Planned | ❌ Not Started

---

## 🖼️ Demo & Screenshots

> _Thêm screenshots tại đây khi có demo live_

### Giao diện chính

**Homepage**

- Hero section với featured games slider
- Categories navigation bar
- Search bar với autocomplete
- Top-rated games section

**Game Store (Catalog)**

- Grid layout responsive (4 cols desktop → 1 col mobile)
- Filters sidebar (Category, Price range, Rating)
- Sort options (Price, Rating, Newest)
- Pagination với page numbers

**Game Detail Page**

- Hero image gallery (main + thumbnails)
- Game info (title, price, stock status)
- Add to Cart / Wishlist buttons
- Description tabs (Overview, Specs, Reviews)
- Related games carousel

**Cart & Checkout**

- Cart items table với quantity controls
- Real-time price updates khi thay đổi quantity
- Promotion code input với validation feedback
- Order summary với tax/shipping calculation
- Shipping address form với validation
- Payment method selection

**User Profile**

- Personal info editor
- Order history table với status badges
- Wishlist grid
- Address book management

### Admin Dashboard

**Dashboard Overview**

- Revenue card với comparison to last month
- Orders count card
- Users count card
- Quick stats table (Top 5 games, Recent orders)

**Game Management**

- Data table với search, filter, sort
- Inline edit / Delete actions
- Add New Game form với image upload preview
- Stock management với low stock alerts

**Order Management**

- Orders table với status filters
- Order detail modal với timeline
- Status update dropdown (Pending → Processing → Shipped → Delivered)
- Export to CSV button

---

## 💻 Yêu cầu hệ thống

### Phát triển (Development)

**Backend:**

- .NET SDK 8.0 or later ([Download](https://dotnet.microsoft.com/download))
- SQL Server 2019+ hoặc PostgreSQL 14+ (hoặc SQLite cho dev)
- IDE: Visual Studio 2022, Rider, hoặc VS Code với C# extension

**Frontend:**

- Node.js 18+ và npm/yarn ([Download](https://nodejs.org/))
- IDE: VS Code, WebStorm

**Optional:**

- Docker Desktop (để run database qua container)
- Git (version control)
- Postman (API testing)

### Production

**Backend:**

- Server/VPS với .NET 8 Runtime
- Database server (SQL Server hoặc PostgreSQL)
- Nginx/IIS cho reverse proxy (optional)
- SSL certificate cho HTTPS

**Frontend:**

- Static hosting (Vercel, Netlify, Cloudflare Pages...)
- Hoặc serve từ backend qua `wwwroot`

---

## 📁 Cấu trúc dự án

```
GameStoreMini/
├── BackEnd/
│   └── Game_store/                    # Main backend project
│       ├── Controllers/               # API Controllers
│       │   ├── AuthController.cs      # Login, Register, JWT
│       │   ├── GamesController.cs     # Game CRUD
│       │   ├── CartController.cs      # Cart operations
│       │   ├── OrdersController.cs    # Order management
│       │   ├── ReviewsController.cs   # User reviews
│       │   ├── UsersController.cs     # User profile
│       │   └── ...
│       ├── Models/                    # Entity models
│       │   ├── Game.cs
│       │   ├── User.cs
│       │   ├── Order.cs
│       │   ├── Cart.cs
│       │   └── ...
│       ├── Data/
│       │   └── AppDbContext.cs        # EF Core DbContext
│       ├── Dtos/                      # Data Transfer Objects
│       ├── Services/                  # Business logic
│       ├── Hubs/
│       │   └── CartHub.cs             # SignalR hub
│       ├── Migrations/                # EF Core migrations
│       ├── Utils/                     # Helpers, extensions
│       ├── Uploads/                   # Uploaded game images
│       ├── appsettings.json           # Configuration
│       └── Program.cs                 # App entry point
│
├── BackEnd/Game_store.Tests/         # Backend unit tests
│   └── Unit/
│       └── GamesControllerTests.cs
│
├── BackEnd/Game_store.IntegrationTests/  # Backend integration tests
│   ├── CustomWebApplicationFactory.cs
│   └── GamesIntegrationTests.cs
│
├── FrontEnd/
│   └── gamestore/                     # React frontend
│       ├── public/                    # Static assets
│       ├── src/
│       │   ├── Pages/                 # Route pages
│       │   │   ├── Home.jsx
│       │   │   ├── Games.jsx
│       │   │   ├── GameDetail.jsx
│       │   │   ├── Cart.jsx
│       │   │   └── ...
│       │   ├── Components/            # Reusable components
│       │   ├── API/                   # Axios API clients
│       │   │   ├── ApiClient.js       # Base axios config
│       │   │   ├── GameAPI.js
│       │   │   ├── CartAPI.js
│       │   │   └── ...
│       │   ├── Auth/                  # Auth utilities
│       │   │   └── useAuth.js
│       │   ├── Cart/                  # Cart context & components
│       │   │   └── CartProvider.jsx
│       │   ├── Wishlist/              # Wishlist components
│       │   ├── Utils/                 # Formatters, helpers
│       │   ├── __tests__/             # Test files
│       │   ├── App.jsx
│       │   └── main.jsx
│       ├── vitest.config.js           # Vitest configuration
│       ├── package.json
│       └── vite.config.js             # Vite configuration
│
├── Introduction.md                    # User guide (Vietnamese)
├── Technologies.md                    # Technical docs (Vietnamese)
└── README.md                          # This file
```

---

## 🚀 Cài đặt & Chạy

### 1️⃣ Clone Repository

```bash
git clone <repository-url>
cd GameStoreMini
```

### 2️⃣ Backend Setup

```bash
cd BackEnd/Game_store

# Restore dependencies
dotnet restore

# Update connection string in appsettings.json
# Mở appsettings.json và sửa ConnectionStrings:
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=GameStoreDB;User Id=sa;Password=YourPassword;TrustServerCertificate=True"
  }
}

# Run migrations để tạo database
dotnet ef database update

# (Optional) Seed data mẫu
# Chạy script SQL trong Scripts/ hoặc tạo manual

# Run backend
dotnet run
```

Backend sẽ chạy tại: `https://localhost:7134` (hoặc port trong launchSettings.json)

**Swagger UI**: Truy cập `https://localhost:7134/swagger` để xem API docs

### 3️⃣ Frontend Setup

```bash
cd FrontEnd/gamestore

# Install dependencies
npm install

# Update API base URL trong src/API/ApiClient.js
# Sửa baseURL nếu backend chạy port khác:
const ApiClient = axios.create({
  baseURL: 'https://localhost:7134/api'
});

# Run development server
npm run dev
```

Frontend sẽ chạy tại: `http://localhost:5173`

### 4️⃣ Truy cập ứng dụng

- **Frontend**: http://localhost:5173
- **Backend API**: https://localhost:7134
- **Swagger Docs**: https://localhost:7134/swagger

### 🔑 Tài khoản mẫu (sau khi seed data)

**Admin:**

- Email: `admin@gamestore.com`
- Password: `Admin@123`

**Customer:**

- Email: `user@gamestore.com`
- Password: `User@123`

Hoặc đăng ký tài khoản mới qua `/register`

---

## 🧪 Kiểm thử

### Backend Tests (xUnit)

```bash
cd BackEnd/Game_store.Tests
dotnet test --logger "console;verbosity=detailed"

cd ../Game_store.IntegrationTests
dotnet test
```

**Unit Tests** (`Game_store.Tests`):

- Mock dependencies với Moq
- Test business logic trong controllers
- Fast execution (in-memory)

**Integration Tests** (`Game_store.IntegrationTests`):

- WebApplicationFactory với InMemory database
- Test full HTTP request/response flow
- Database integration validation

### Frontend Tests (Vitest)

```bash
cd FrontEnd/gamestore

# Run all tests
npm test

# Run with UI
npm run test:ui

# Run with coverage
npm run test:coverage
```

**Test Coverage:**

- Unit tests: Utility functions (formatCurrency, validators...)
- Component tests: WishlistButton, CartProvider với mocked API
- Integration tests: API clients với mocked axios

**Mocking Strategy:**

- API calls: Mock axios/ApiClient
- SignalR: Mock HubConnectionBuilder
- Context: Mock AuthContext, CartContext
- Browser APIs: Mock localStorage, fetch

### Manual Testing

**Postman Collection**: Import `Game_store.http` vào Postman/REST Client

**Test Scenarios:**

1. User registration → Login → Browse games → Add to cart → Checkout → Track order
2. Admin login → Create game → Upload image → Manage orders → View reports
3. Real-time cart sync: Mở 2 tabs, thay đổi cart ở tab 1, verify update ở tab 2

---

## 📚 API Documentation

### Base URL

```
Development: https://localhost:7134/api
Production: https://your-domain.com/api
```

### Authentication

Sử dụng JWT Bearer token trong header:

```
Authorization: Bearer <your-jwt-token>
```

### Endpoints Overview

**Auth:**

- `POST /api/auth/register` - Đăng ký user mới
- `POST /api/auth/login` - Đăng nhập, nhận JWT token

**Games:**

- `GET /api/games` - List games (pagination, filter, sort)
- `GET /api/games/{id}` - Get game detail
- `POST /api/games` - Create game (Admin only)
- `PUT /api/games/{id}` - Update game (Admin only)
- `DELETE /api/games/{id}` - Delete game (Admin only)

**Cart:**

- `GET /api/cart` - Get current user's cart
- `POST /api/cart/add` - Add item to cart
- `PUT /api/cart/update` - Update quantity
- `DELETE /api/cart/{id}` - Remove item

**Orders:**

- `POST /api/orders` - Create order from cart
- `GET /api/orders` - Get user orders (or all for Admin)
- `GET /api/orders/{id}` - Get order detail
- `PUT /api/orders/{id}/status` - Update status (Admin only)

**Reviews:**

- `GET /api/reviews/game/{gameId}` - Get reviews for a game
- `POST /api/reviews` - Create review (verified purchase only)
- `DELETE /api/reviews/{id}` - Delete review (Admin/Owner)

**Promotions:**

- `GET /api/promotions` - List active promotions
- `POST /api/promotions/validate` - Validate promo code
- `POST /api/promotions` - Create promotion (Admin only)

**Users:**

- `GET /api/users/profile` - Get current user profile
- `PUT /api/users/profile` - Update profile
- `GET /api/users` - List users (Admin only)

**SignalR Hub:**

- Hub URL: `/carthub`
- Methods: `JoinUserGroup(userId)`, `CartUpdated(cartData)`

Chi tiết đầy đủ tại Swagger UI khi run backend.

---

## 🌐 Triển khai

### Backend Deployment (.NET)

**Option 1: Azure App Service**

```bash
# Publish to folder
dotnet publish -c Release -o ./publish

# Deploy to Azure (using Azure CLI)
az webapp up --name your-app-name --resource-group your-rg
```

**Option 2: Docker**

```dockerfile
# Dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Game_store.csproj", "./"]
RUN dotnet restore
COPY . .
RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Game_store.dll"]
```

```bash
docker build -t gamestore-backend .
docker run -p 8080:80 gamestore-backend
```

**Option 3: VPS (Ubuntu)**

```bash
# Install .NET Runtime
wget https://dot.net/v1/dotnet-install.sh
bash dotnet-install.sh --channel 8.0 --runtime aspnetcore

# Copy published files
scp -r ./publish/* user@your-vps:/var/www/gamestore

# Setup systemd service
sudo nano /etc/systemd/system/gamestore.service

# Configure Nginx reverse proxy
sudo nano /etc/nginx/sites-available/gamestore
```

### Frontend Deployment (React)

**Option 1: Vercel (Recommended)**

```bash
# Install Vercel CLI
npm i -g vercel

# Deploy
cd FrontEnd/gamestore
vercel
```

**Option 2: Netlify**

```bash
# Build
npm run build

# Deploy folder `dist/` to Netlify via UI or CLI
```

**Option 3: Serve from Backend**

```bash
# Build frontend
cd FrontEnd/gamestore
npm run build

# Copy dist/ to BackEnd/Game_store/wwwroot/
cp -r dist/* ../../BackEnd/Game_store/wwwroot/

# Update Program.cs to serve static files
app.UseStaticFiles();
app.MapFallbackToFile("index.html");
```

### Environment Variables

**Backend (`appsettings.json` hoặc Environment Variables):**

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=prod-server;Database=GameStoreDB;..."
  },
  "Jwt": {
    "Key": "your-super-secret-key-min-32-chars",
    "Issuer": "GameStore",
    "Audience": "GameStoreUsers",
    "ExpireMinutes": 60
  },
  "EmailSettings": {
    "SmtpServer": "smtp.gmail.com",
    "Port": 587,
    "Username": "your-email@gmail.com",
    "Password": "your-app-password"
  }
}
```

**Frontend (`.env.production`):**

```
VITE_API_BASE_URL=https://api.yourdomain.com/api
VITE_SIGNALR_HUB_URL=https://api.yourdomain.com/carthub
```

### CI/CD với GitHub Actions

Tạo file `.github/workflows/ci.yml`:

```yaml
name: CI/CD Pipeline

on:
  push:
    branches: [main]
  pull_request:
    branches: [main]

jobs:
  backend-test:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v3
      - name: Setup .NET
        uses: actions/setup-dotnet@v3
        with:
          dotnet-version: "8.0.x"
      - name: Restore
        run: dotnet restore BackEnd/Game_store/Game_store.csproj
      - name: Build
        run: dotnet build BackEnd/Game_store/Game_store.csproj --no-restore
      - name: Test
        run: |
          dotnet test BackEnd/Game_store.Tests/Game_store.UnitTests.csproj --no-build --verbosity normal
          dotnet test BackEnd/Game_store.IntegrationTests/Game_store.IntegrationTests.csproj --no-build --verbosity normal

  frontend-test:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v3
      - name: Setup Node
        uses: actions/setup-node@v3
        with:
          node-version: "18"
      - name: Install
        run: npm ci
        working-directory: FrontEnd/gamestore
      - name: Test
        run: npm test
        working-directory: FrontEnd/gamestore
```

---

## 🏗️ Kiến trúc kỹ thuật

### Database Schema (Simplified)

```sql
Users
├── UserId (PK)
├── Email (unique)
├── PasswordHash
├── Role (Customer/Admin)
└── IsEmailConfirmed

Games
├── GameId (PK)
├── Title
├── Description
├── Price
├── Stock
├── CategoryId (FK)
└── ImageUrl

Categories
├── CategoryId (PK)
└── Name

Orders
├── OrderId (PK)
├── UserId (FK)
├── Status (Pending/Processing/Shipped/Delivered)
├── TotalAmount
└── OrderDate

OrderItems
├── OrderItemId (PK)
├── OrderId (FK)
├── GameId (FK)
├── Quantity
└── Price

Cart
├── CartId (PK)
├── UserId (FK)
└── CreatedDate

CartItems
├── CartItemId (PK)
├── CartId (FK)
├── GameId (FK)
└── Quantity

Reviews
├── ReviewId (PK)
├── GameId (FK)
├── UserId (FK)
├── Rating (1-5)
├── Comment
└── CreatedAt

Promotions
├── PromotionId (PK)
├── Code (unique)
├── DiscountType (Percentage/FixedAmount)
├── DiscountValue
├── MinimumOrderValue
└── ExpiryDate
```

### Luồng xử lý chính

**1. Authentication Flow:**

```
User → Register/Login → Backend validates → Generate JWT → Return token
→ Frontend stores in localStorage → Attach to subsequent API requests
```

**2. Add to Cart Flow (Real-time):**

```
User clicks "Add to Cart" → Frontend calls API → Backend updates DB
→ Backend broadcasts via SignalR → All connected clients receive update
→ Frontend updates UI automatically
```

**3. Checkout Flow:**

```
User in Cart → Click Checkout → Select Address → Apply Promo Code
→ Choose Payment → Submit Order → Backend creates Order + OrderItems
→ Clear Cart → Send confirmation email → Redirect to Order Detail
```

Chi tiết đầy đủ xem file `Technologies.md`

---

## 🤝 Đóng góp

Contributions are welcome! Nếu bạn muốn contribute:

1. Fork repository
2. Tạo branch mới (`git checkout -b feature/amazing-feature`)
3. Commit changes (`git commit -m 'Add amazing feature'`)
4. Push to branch (`git push origin feature/amazing-feature`)
5. Mở Pull Request

**Guidelines:**

- Viết tests cho features mới
- Update documentation nếu cần
- Follow coding conventions (C# + React best practices)
- Commit messages rõ ràng (Conventional Commits format)

---

## 📞 Liên hệ

**Project Link**: [https://github.com/your-username/GameStoreMini](https://github.com/your-username/GameStoreMini)

**Issues**: [https://github.com/your-username/GameStoreMini/issues](https://github.com/your-username/GameStoreMini/issues)

**Maintainer**: Your Name - your.email@example.com

---

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

---

## 🙏 Acknowledgments

- ASP.NET Core Documentation
- React Documentation
- SignalR Guide
- Entity Framework Core Docs
- Testing Library Best Practices
- Community contributors

---

<div align="center">
  <strong>Được phát triển với ❤️ bởi [Your Team Name]</strong>
  <br>
  <sub>Dự án mẫu cho mục đích học tập và demo</sub>
</div>
