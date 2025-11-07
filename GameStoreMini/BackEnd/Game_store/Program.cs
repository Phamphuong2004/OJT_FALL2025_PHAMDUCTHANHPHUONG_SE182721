using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using GameStoreMini.Data;
using GameStoreMini.Services;
using GameStoreMini.Models;
using GameStoreMini.Utils;
using Microsoft.Extensions.FileProviders;
using System.IO;

var builder = WebApplication.CreateBuilder(args);

// Add controllers and Swagger for easy testing.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// SignalR for realtime cart updates
builder.Services.AddSignalR();

// SignalR for realtime cart updates
builder.Services.AddSignalR();

// ===== CORS Configuration =====
builder.Services.AddCors(options =>
{
    if (builder.Environment.IsDevelopment())
    {
        // Development: Cho phép tất cả localhost ports
        options.AddPolicy("AllowFrontend", policy =>
        {
            policy.SetIsOriginAllowed(origin =>
            {
                // Cho phép tất cả localhost với bất kỳ port nào
                if (string.IsNullOrEmpty(origin)) return false;
                var uri = new Uri(origin);
                return uri.Host == "localhost" || uri.Host == "127.0.0.1";
            })
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
        });
    }
    else
    {
        // Production: Chỉ định cụ thể origins
        options.AddPolicy("AllowFrontend", policy =>
        {
            policy.WithOrigins(
                    "https://yourdomain.com",  // Thay bằng domain thật
                    "https://www.yourdomain.com"
                  )
                  .AllowAnyMethod()
                  .AllowAnyHeader()
                  .AllowCredentials();
        });
    }
});
// ===== HẾT CORS Configuration =====

// Configure PostgreSQL database. The connection string is in appsettings.json.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register our token service which creates JWTs.
builder.Services.AddScoped<ITokenService, TokenService>();

// Register review service
builder.Services.AddScoped<Game_store.Services.IReviewService, Game_store.Services.ReviewService>();

// Read JWT settings and configure authentication.
var jwtSection = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtSection.GetValue<string>("Key")!);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false; // set true in production
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = jwtSection.GetValue<string>("Issuer"),
        ValidateAudience = true,
        ValidAudience = jwtSection.GetValue<string>("Audience"),
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateLifetime = true
            ,
            // Ensure Role claim from JWT is mapped correctly to ClaimsPrincipal.IsInRole
            // Token issuers sometimes use the long claim type URI; try the common ones.
            RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
    };
});

// Optional: add an authorization policy named "RequireAdmin" that requires the Admin role.
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdmin", policy => policy.RequireRole(Roles.Admin));
});

builder.WebHost.UseWebRoot("wwwroot");
// Explicitly set URLs so HTTPS redirection middleware can determine the HTTPS port in development.
// These should match Properties/launchSettings.json (https://localhost:7154; http://localhost:5179)
builder.WebHost.UseUrls("https://localhost:7154", "http://localhost:5179");

var app = builder.Build();

// If the app is behind a proxy (for example the Vite dev server proxy),
// use forwarded headers so Request.Scheme and Request.IsHttps reflect the
// original client request. This is important when deciding cookie Secure/SameSite.
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

app.UseRouting();

// serve wwwroot by default
app.UseStaticFiles();

// serve Uploads folder at /uploads
var uploadsPath = Path.Combine(builder.Environment.ContentRootPath, "Uploads");
if (!Directory.Exists(uploadsPath))
{
    Directory.CreateDirectory(uploadsPath); // ensure exists
}
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(uploadsPath),
    RequestPath = "/uploads"
});

// Apply EF Core migrations and seed a demo admin user (for learning).
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    try
    {
        // Apply pending migrations to the PostgreSQL database
        db.Database.Migrate();
    }
    catch (Exception ex)
    {
        // In some environments the DB may not be available at startup.
        // Log and rethrow so the developer sees the problem.
        Console.WriteLine($"Migrations failed: {ex.Message}");
        throw;
    }
    // Seed a demo admin user only when explicitly enabled via configuration.
    // This prevents automatic creation of the admin account on every startup
    // (which can be surprising if you delete the account and it reappears).
    // To enable demo seeding set "SeedDemoData": true in appsettings or environment.
    var seedDemo = app.Configuration.GetValue<bool>("SeedDemoData", false);
    if (seedDemo)
    {
        if (!db.Users.Any(u => u.Email == "admin@gamestore.local"))
        {
            var admin = new User
            {
                Email = "admin@gamestore.local",
                UserName = "admin",
                Role = Roles.Admin
            };
            // Hash the password "Admin123!" and save.
            admin.PasswordHash = PasswordHelper.HashPassword("Admin123!");
            db.Users.Add(admin);
            db.SaveChanges();
        }
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Enable CORS early so browser preflight and calls are allowed
app.UseCors("AllowFrontend"); // hoặc "AllowAllOrigins"

app.UseHttpsRedirection();

// Enable authentication/authorization middleware so protected endpoints work.
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
// map SignalR hubs
app.MapHub<GameStoreMini.Hubs.CartHub>("/hubs/cart");
app.Run();
