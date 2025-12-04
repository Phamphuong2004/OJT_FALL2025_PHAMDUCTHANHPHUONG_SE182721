# GameStoreMini - Tài liệu kỹ thuật

## 🛠️ Tech Stack

### Backend

- **Framework**: ASP.NET Core 8.0 (Web API)
- **ORM**: Entity Framework Core 8.0
- **Database**: SQL Server / PostgreSQL / InMemory (configurable)
- **Authentication**: JWT (JSON Web Tokens)
- **Real-time**: SignalR (WebSocket)
- **Testing**: xUnit, Moq, WebApplicationFactory

### Frontend

- **Framework**: React 19
- **Build Tool**: Vite 7
- **HTTP Client**: Axios
- **Real-time**: @microsoft/signalr
- **Routing**: React Router 7
- **Form Handling**: Formik + Yup
- **Testing**: Vitest, @testing-library/react
- **UI**: Custom CSS (modular)

### DevOps & Tools

- **Version Control**: Git
- **CI/CD**: GitHub Actions (recommended)
- **Package Manager**: npm (frontend), NuGet (backend)
- **Code Quality**: ESLint (frontend), Analyzer (backend)

## 🏗️ Kiến trúc hệ thống

### Kiến trúc tổng quan

```
┌─────────────┐      HTTPS/WSS      ┌─────────────┐
│   Browser   │ ◄─────────────────► │   Backend   │
│   (React)   │                     │  (.NET API) │
└─────────────┘                     └─────────────┘
                                           │
                                           ▼
                                    ┌─────────────┐
                                    │   Database  │
                                    │ (SQL Server)│
                                    └─────────────┘
```

### Backend Architecture

**Layered Architecture** (gần với 3-tier):

```
Controllers/           # API endpoints (HTTP handlers)
    ├─ GamesController
    ├─ CartController
    ├─ OrdersController
    └─ ...

Dtos/                  # Data Transfer Objects
    ├─ GameDtos.cs
    ├─ OrderDtos.cs
    └─ ...

Models/                # EF Core entities
    ├─ Game.cs
    ├─ User.cs
    ├─ Order.cs
    └─ ...

Data/
    └─ AppDbContext.cs # EF Core DbContext

Services/              # Business logic (optional layer)

Hubs/                  # SignalR hubs
    └─ CartHub.cs

Utils/                 # Helpers, extensions
```

**Patterns sử dụng**:

- **Repository Pattern** (implicit via EF Core DbSet)
- **DTO Pattern** để tách biệt API contract và database entities
- **Dependency Injection** (built-in ASP.NET Core)

### Frontend Architecture

**Component-based Architecture**:

```
src/
├─ API/                    # API client modules
│  ├─ ApiClient.js         # Axios instance + interceptors
│  ├─ GameAPI.js
│  ├─ CartAPI.js
│  └─ ...
├─ Cart/                   # Cart feature
│  ├─ CartProvider.jsx     # Context + SignalR
│  └─ ...
├─ Games/                  # Games feature
│  ├─ GameList.jsx
│  ├─ GameDetails.jsx
│  └─ ...
├─ Auth/                   # Authentication
│  ├─ useAuth.js           # Auth hook
│  └─ ...
├─ Components/             # Shared UI components
│  ├─ Header.jsx
│  ├─ Footer.jsx
│  ├─ Toast.jsx
│  └─ ...
├─ Pages/                  # Page components
│  ├─ Home.jsx
│  ├─ Store.jsx
│  └─ ...
└─ Routes/
   └─ AppRoutes.jsx        # Routing config
```

**Patterns sử dụng**:

- **Context API** cho state management (CartProvider)
- **Custom Hooks** cho business logic
- **Module pattern** cho API clients

## 🔐 Authentication & Authorization

### Flow đăng nhập

1. User gửi credentials (username/password) đến `/api/auth/login`
2. Backend xác thực, tạo JWT token
3. Token được trả về client (JSON response)
4. Client lưu token vào `localStorage`
5. Mọi request tiếp theo gửi token qua header:
   ```
   Authorization: Bearer <token>
   ```

### JWT Structure

```json
{
  "sub": "user-id",
  "unique_name": "username",
  "email": "user@example.com",
  "role": "Customer",
  "exp": 1234567890
}
```

### Authorization trong Backend

```csharp
[Authorize(Roles = "Admin")]
public async Task<IActionResult> DeleteGame(int id) { ... }
```

### Authorization trong Frontend

```javascript
const role = getUserRole();
if (role === "Admin") {
  // Show admin features
}
```

## 🔄 Real-time với SignalR

### Cart Hub

Backend (`Hubs/CartHub.cs`):

```csharp
public class CartHub : Hub
{
    public async Task JoinGroup(string groupId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, groupId);
    }

    public async Task NotifyCartUpdated(string groupId)
    {
        await Clients.Group(groupId).SendAsync("CartUpdated");
    }
}
```

Frontend (`CartProvider.jsx`):

```javascript
const connection = new HubConnectionBuilder()
  .withUrl("/hubs/cart", {
    accessTokenFactory: () => getToken(),
  })
  .build();

connection.on("CartUpdated", async () => {
  // Fetch latest cart from API
  const cart = await CartAPI.getCart();
  updateCart(cart);
});
```

## 🗃️ Database Schema (chính)

### Users

```sql
CREATE TABLE Users (
    Id INT PRIMARY KEY IDENTITY,
    Username NVARCHAR(100) UNIQUE NOT NULL,
    Email NVARCHAR(255) UNIQUE NOT NULL,
    PasswordHash NVARCHAR(MAX) NOT NULL,
    Role NVARCHAR(50) NOT NULL, -- Customer/Admin
    CreatedAt DATETIME2 NOT NULL,
    ...
);
```

### Games

```sql
CREATE TABLE Games (
    Id INT PRIMARY KEY IDENTITY,
    Title NVARCHAR(255) NOT NULL,
    Description NVARCHAR(MAX),
    Price DECIMAL(18,2) NOT NULL,
    Stock INT NOT NULL,
    ImageUrl NVARCHAR(500),
    CreatedAt DATETIME2 NOT NULL,
    ...
);
```

### Orders

```sql
CREATE TABLE Orders (
    Id INT PRIMARY KEY IDENTITY,
    UserId INT FOREIGN KEY REFERENCES Users(Id),
    TotalAmount DECIMAL(18,2) NOT NULL,
    Status NVARCHAR(50) NOT NULL, -- Pending/Processing/Shipped/Delivered
    CreatedAt DATETIME2 NOT NULL,
    ...
);
```

### OrderItems

```sql
CREATE TABLE OrderItems (
    Id INT PRIMARY KEY IDENTITY,
    OrderId INT FOREIGN KEY REFERENCES Orders(Id),
    GameId INT FOREIGN KEY REFERENCES Games(Id),
    Quantity INT NOT NULL,
    UnitPrice DECIMAL(18,2) NOT NULL,
    ...
);
```

## 🧪 Testing Strategy

### Backend Tests

**Unit Tests** (`Game_store.Tests/`):

- Test individual methods, business logic
- Mock dependencies (DbContext, services)
- Example: `GamesControllerTests.cs`

```csharp
[Fact]
public async Task Create_NegativePrice_ReturnsBadRequest()
{
    // Arrange
    var controller = new GamesController(mockDb, mockEnv);
    var dto = new CreateGameDto { Price = -1 };

    // Act
    var result = await controller.Create(dto);

    // Assert
    Assert.IsType<BadRequestObjectResult>(result);
}
```

**Integration Tests** (`Game_store.IntegrationTests/`):

- Test API endpoints end-to-end
- Use `WebApplicationFactory<Program>`
- InMemory database for isolation

```csharp
public class GamesIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    [Fact]
    public async Task GetAll_ReturnsGames()
    {
        var client = _factory.CreateClient();
        var response = await client.GetAsync("/api/games");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
```

### Frontend Tests

**Unit Tests**:

- Test pure functions, utilities
- Example: `formatCurrency.test.js`

```javascript
test("formats number to VND", () => {
  expect(formatCurrency(10000)).toMatch(/10\.000/);
});
```

**Component Tests**:

- Test React components with Testing Library
- Mock API calls, SignalR
- Example: `WishlistButton.test.jsx`

```javascript
test("adds item to wishlist", async () => {
  render(<WishlistButton gameId={1} />);
  const btn = screen.getByRole("button");
  await userEvent.click(btn);
  await waitFor(() => expect(btn).toHaveTextContent("Đã thích"));
});
```

**Integration-style Tests**:

- Test providers, context interactions
- Example: `CartProvider.test.jsx`

### E2E Tests (khuyến nghị)

- Playwright hoặc Cypress
- Test user flows: login → add to cart → checkout

## 📊 Performance Considerations

### Backend

- **Pagination**: API endpoints hỗ trợ `page` và `pageSize`
- **Caching**: có thể thêm Redis cho session, cart
- **Indexing**: tạo indexes trên `Users.Email`, `Games.Title` cho tìm kiếm nhanh
- **Async/Await**: tất cả DB operations đều async

### Frontend

- **Code Splitting**: React Router lazy loading
- **Memoization**: useMemo cho computed values
- **Debouncing**: tìm kiếm, filter
- **Image Optimization**: lazy loading images

## 🔒 Security

### Backend

- ✅ JWT authentication
- ✅ Role-based authorization
- ✅ Password hashing (ASP.NET Identity)
- ✅ HTTPS enforced
- ✅ CORS configured
- ⚠️ Rate limiting (nên thêm)
- ⚠️ Input validation (cần mở rộng)

### Frontend

- ✅ Token stored in localStorage (⚠️ cân nhắc httpOnly cookies cho production)
- ✅ XSS protection (React escapes by default)
- ✅ No sensitive data in client code

## 🚀 Deployment

### Backend

1. Build release:
   ```bash
   dotnet publish -c Release -o ./publish
   ```
2. Configure production `appsettings.json`
3. Deploy to IIS / Azure App Service / Docker

### Frontend

1. Build production:
   ```bash
   npm run build
   ```
2. Deploy `dist/` folder to static hosting (Netlify, Vercel, Nginx)
3. Configure reverse proxy cho `/api` → backend

### Docker (mẫu)

**Backend Dockerfile**:

```dockerfile
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app
COPY . .
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .
EXPOSE 80
ENTRYPOINT ["dotnet", "Game_store.dll"]
```

**Frontend Dockerfile**:

```dockerfile
FROM node:20-alpine AS build
WORKDIR /app
COPY package*.json ./
RUN npm ci
COPY . .
RUN npm run build

FROM nginx:alpine
COPY --from=build /app/dist /usr/share/nginx/html
EXPOSE 80
```

## 📈 Monitoring & Logging

### Backend

- Built-in logging: `ILogger<T>`
- Application Insights (Azure)
- Serilog (khuyến nghị)

### Frontend

- Console logging (development)
- Error boundaries
- Analytics (Google Analytics, Mixpanel)

## 🔧 Development Workflow

1. **Branching Strategy**: Git Flow hoặc GitHub Flow
2. **Code Review**: Pull requests bắt buộc
3. **CI/CD**: GitHub Actions chạy tests tự động
4. **Versioning**: Semantic Versioning (SemVer)

## 📚 Tài liệu tham khảo

- [ASP.NET Core Docs](https://docs.microsoft.com/aspnet/core)
- [Entity Framework Core](https://docs.microsoft.com/ef/core)
- [React Docs](https://react.dev)
- [Vite Guide](https://vitejs.dev/guide)
- [SignalR Documentation](https://docs.microsoft.com/aspnet/core/signalr)

## 🤝 Contributing

1. Fork repository
2. Tạo feature branch
3. Commit changes với message rõ ràng
4. Chạy tests: `dotnet test` và `npm test`
5. Tạo Pull Request

---

_Cập nhật: 2025-12-04_
