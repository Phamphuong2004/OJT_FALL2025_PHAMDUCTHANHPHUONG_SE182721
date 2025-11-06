using Microsoft.AspNetCore.Mvc;
using GameStoreMini.Data;
using GameStoreMini.Dtos;
using GameStoreMini.Models;
using GameStoreMini.Services;
using GameStoreMini.Utils;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Collections.Concurrent;
using Microsoft.AspNetCore.Http; // required for CookieOptions / SameSiteMode
using System.Threading.Tasks;

namespace GameStoreMini.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly ITokenService _token;
        private readonly IWebHostEnvironment _env;
    // Note: we persist refresh token on the User model (RefreshToken, RefreshTokenExpiry)
    // so refresh tokens survive server restarts. We no longer use an in-memory store.
        // Note: we only depend on AppDbContext and ITokenService here.
        // Refresh-token support (httpOnly cookies + rotation) would require
        // additional services (a refresh token store and validation helpers).
        public AuthController(AppDbContext db, ITokenService token, IWebHostEnvironment env)
        {
            _db = db;
            _token = token;
            _env = env;
        }

        // Temporary debug endpoint to help frontend devs verify cookie behavior.
        // Call GET /api/auth/debug-cookie from the browser (use credentials) to see
        // whether the refreshToken cookie is present and what the request looks like.
        [HttpGet("debug-cookie")]
        public IActionResult DebugCookie()
        {
            var cookie = Request.Cookies["refreshToken"];
            return Ok(new
            {
                cookiePresent = !string.IsNullOrEmpty(cookie),
                cookieLength = cookie?.Length ?? 0,
                isHttps = Request.IsHttps,
                host = Request.Host.ToString(),
                origin = Request.Headers["Origin"].ToString()
            });
        }

        // POST: /api/auth/register
        [HttpPost("register")]
        [RequestSizeLimit(10_000_000)] // limit upload to ~10MB
        public async Task<IActionResult> Register([FromForm] RegisterDto dto)
        {
            // This endpoint accepts multipart/form-data register requests.
            // It validates the incoming DTO, optionally saves an uploaded avatar file
            // to the server wwwroot/uploads folder, creates a new User entity with
            // a hashed password, and returns a JWT for immediate use by the client.
            // Keep this method simple and synchronous-looking: IO is performed
            // asynchronously where possible (file copy and EF SaveChangesAsync).

            // Basic validation
            if (!dto.TermsAccepted) return BadRequest("You must accept terms.");
            if (dto.Password != dto.ConfirmPassword) return BadRequest("Passwords do not match.");
            if (_db.Users.Any(u => u.Email == dto.Email)) return BadRequest("Email already used.");

            string? avatarUrl = null;
            if (dto.Avatar != null && dto.Avatar.Length > 0)
            {
                var uploads = Path.Combine(_env.WebRootPath ?? "wwwroot", "uploads");
                if (!Directory.Exists(uploads)) Directory.CreateDirectory(uploads);
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(dto.Avatar.FileName);
                var filePath = Path.Combine(uploads, fileName);
                await using var stream = System.IO.File.Create(filePath);
                await dto.Avatar.CopyToAsync(stream);
                avatarUrl = $"/uploads/{fileName}";
            }

            var user = new User
            {
                Email = dto.Email,
                UserName = dto.UserName,
                FullName = dto.FullName,
                PhoneNumber = dto.PhoneNumber,
                AvatarUrl = avatarUrl,
                // Store a secure hash (never the plain password)
                PasswordHash = PasswordHelper.HashPassword(dto.Password),
                Role = Roles.Customer,
                // For convenience set EmailConfirmed true and timestamps
                EmailConfirmed = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            // Create an access token (short-lived) for the newly created user and
            // return it in the response body. We do NOT set a refresh cookie here
            // (could be added if you want persistent login for new registrations).
            var token = _token.CreateToken(user);
            return Ok(new AuthResultDto { Token = token });
        }

        // POST: /api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            // This login endpoint accepts either an email or username (see LoginDto).
            // It authenticates credentials, issues an access token and also creates
            // a refresh token which is stored in a HttpOnly cookie to support
            // silent refresh flows on the frontend.

            // Determine identifier: prefer Identifier field, then Email, then UserName
            var identifier = dto.Identifier?.Trim();
            if (string.IsNullOrWhiteSpace(identifier))
            {
                identifier = dto.Email?.Trim();
            }
            if (string.IsNullOrWhiteSpace(identifier))
            {
                identifier = dto.UserName?.Trim();
            }

            if (string.IsNullOrWhiteSpace(identifier))
            {
                return BadRequest(new { errors = new { Identifier = new[] { "Email or Username is required." } }, title = "Validation error", status = 400 });
            }

            User? user = null;

            // Detect if identifier looks like an email
            if (identifier.Contains("@"))
            {
                user = _db.Users.FirstOrDefault(u => u.Email == identifier);
            }
            else
            {
                user = _db.Users.FirstOrDefault(u => u.UserName == identifier);
            }

            if (user == null) return Unauthorized("Invalid credentials.");

            if (!PasswordHelper.VerifyPassword(user.PasswordHash, dto.Password))
                return Unauthorized("Invalid credentials.");

            // Ensure user has a role. If seeded admin email, make Admin; otherwise ensure Customer.
            bool changed = false;
            if (string.IsNullOrWhiteSpace(user.Role))
            {
                user.Role = Roles.Customer;
                changed = true;
            }
            // If this is the known admin email, ensure Admin role
            if (user.Email == "admin@gamestore.local" && user.Role != Roles.Admin)
            {
                user.Role = Roles.Admin;
                changed = true;
            }
            if (changed)
            {
                user.UpdatedAt = DateTime.UtcNow;
                await _db.SaveChangesAsync();
            }

            // Generate a refresh token (server-side) and store it in a secure,
            // HttpOnly cookie so JavaScript cannot read it. The frontend can call
            // POST /api/auth/refresh which will validate this cookie and return a
            // fresh access token without asking the user to re-enter credentials.
            // NOTE: refresh-token/cookie logic is intentionally omitted here.
            // The current implementation returns only an access token. If you
            // want "silent" or persistent login, implement a refresh-token
            // service and set a secure HttpOnly cookie here. See comments above
            // the Refresh endpoint (if present) for an example of that flow.

            var token = _token.CreateToken(user);

            // create refresh token (secure random string)
            var refreshBytes = RandomNumberGenerator.GetBytes(64);
            var refreshToken = Convert.ToBase64String(refreshBytes);
            var expiry = DateTime.UtcNow.AddDays(7);
            // store refresh token on the user record (persisted to DB)
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiry = expiry;
            await _db.SaveChangesAsync();

            // set the refresh token as HttpOnly cookie so the browser will send it
            // Choose cookie options that work for both dev (HTTP + proxy) and HTTPS.
            // Modern browsers require SameSite=None to be paired with Secure=true. When
            // running the frontend via the Vite dev proxy (http://localhost:5173) the
            // browser receives the proxied response over HTTP, so a Secure cookie would
            // be rejected by the browser. For local development prefer SameSite=Lax
            // and Secure=false. In production/https use SameSite=None and Secure=true.
            // Determine whether the original request was HTTPS. When the app is
            // behind a proxy the header X-Forwarded-Proto may indicate the
            // original protocol; fall back to Request.IsHttps otherwise.
            var forwardedProto = Request.Headers["X-Forwarded-Proto"].ToString();
            var effectiveIsHttps = (!string.IsNullOrEmpty(forwardedProto) && forwardedProto.Equals("https", StringComparison.OrdinalIgnoreCase)) || Request.IsHttps;
            Console.WriteLine($"[Auth] login: X-Forwarded-Proto='{forwardedProto}', Request.IsHttps={Request.IsHttps}, effectiveIsHttps={effectiveIsHttps}");

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = effectiveIsHttps,
                SameSite = effectiveIsHttps ? SameSiteMode.None : SameSiteMode.Lax,
                Expires = user.RefreshTokenExpiry,
                Path = "/",
                IsEssential = true
            };
            // Development override: when running in Development force non-secure
            // cookie options so browsers on localhost will accept the cookie.
            if (_env.IsDevelopment())
            {
                cookieOptions.Secure = false;
                cookieOptions.SameSite = SameSiteMode.Lax;
                Console.WriteLine("[Auth] Development override: Secure=false, SameSite=Lax for refresh cookie");
            }
            Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
            // debug log to help frontend dev: show that we set a refresh cookie for this user
            Console.WriteLine($"[Auth] Set refresh cookie for user {user.Id}, expires {user.RefreshTokenExpiry}");

            var result = new AuthResultDto { Token = token };
            if (_env.IsDevelopment()) result.RefreshTokenDebug = refreshToken;
            return Ok(result);
        }

        // NOTE: Refresh endpoint removed.
        // If you want to add refresh-token support, implement a small service
        // to generate/validate refresh tokens and store them (DB or cache),
        // then recreate the Refresh action using that service.

        // Implement refresh: read the HttpOnly cookie named "refreshToken",
        // validate it against the stored refresh token on the user, and if
        // valid issue a new access token and rotate the refresh token.
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh()
        {
            // ensure this async method contains an await (prevents CS1998 warning)
            await Task.Yield();
            var cookie = Request.Cookies["refreshToken"];
            Console.WriteLine($"[Auth] Refresh called - cookie present: {(!string.IsNullOrEmpty(cookie)).ToString()}");
            if (string.IsNullOrEmpty(cookie)) return Unauthorized(new { title = "Unauthorized", status = 401 });

            // find user by refresh token stored in DB
            var user = _db.Users.FirstOrDefault(u => u.RefreshToken == cookie);
            if (user == null)
            {
                Console.WriteLine($"[Auth] Refresh token not found in DB (cookie length={cookie.Length})");
                return Unauthorized(new { title = "Unauthorized", status = 401 });
            }

            if (!user.RefreshTokenExpiry.HasValue || user.RefreshTokenExpiry.Value < DateTime.UtcNow)
            {
                Console.WriteLine($"[Auth] Refresh token expired for user {user.Id}");
                // clear stored refresh token
                user.RefreshToken = null;
                user.RefreshTokenExpiry = null;
                await _db.SaveChangesAsync();
                return Unauthorized(new { title = "Unauthorized", status = 401 });
            }
            if (user == null) return Unauthorized(new { title = "Unauthorized", status = 401 });

            // valid -> issue new access token and rotate refresh token
            var newAccess = _token.CreateToken(user);
            var newRefreshBytes = RandomNumberGenerator.GetBytes(64);
            var newRefresh = Convert.ToBase64String(newRefreshBytes);
            var newExpiry = DateTime.UtcNow.AddDays(7);
            // rotate refresh token on the user record
            user.RefreshToken = newRefresh;
            user.RefreshTokenExpiry = newExpiry;
            await _db.SaveChangesAsync();


            // See comment above for cookie selection rationale.
            var forwardedProto2 = Request.Headers["X-Forwarded-Proto"].ToString();
            var effectiveIsHttps2 = (!string.IsNullOrEmpty(forwardedProto2) && forwardedProto2.Equals("https", StringComparison.OrdinalIgnoreCase)) || Request.IsHttps;
            Console.WriteLine($"[Auth] refresh: X-Forwarded-Proto='{forwardedProto2}', Request.IsHttps={Request.IsHttps}, effectiveIsHttps={effectiveIsHttps2}");

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = effectiveIsHttps2,
                SameSite = effectiveIsHttps2 ? SameSiteMode.None : SameSiteMode.Lax,
                Expires = user.RefreshTokenExpiry,
                Path = "/",
                IsEssential = true
            };
            if (_env.IsDevelopment())
            {
                cookieOptions.Secure = false;
                cookieOptions.SameSite = SameSiteMode.Lax;
                Console.WriteLine("[Auth] Development override: Secure=false, SameSite=Lax for refresh cookie (refresh)");
            }

            Response.Cookies.Append("refreshToken", newRefresh, cookieOptions);

            var resDto = new AuthResultDto { Token = newAccess };
            if (_env.IsDevelopment()) resDto.RefreshTokenDebug = newRefresh;
            return Ok(resDto);
        }

        // Development-only: accept a refresh token in the request body (for local dev when
        // cookies are unreliable). This endpoint should only be enabled in Development.
        [HttpPost("refresh-dev")]
        public async Task<IActionResult> RefreshDev([FromBody] RefreshDevDto dto)
        {
            if (!_env.IsDevelopment()) return NotFound();
            if (dto == null || string.IsNullOrEmpty(dto.Token)) return BadRequest();

            var cookie = dto.Token;
            Console.WriteLine($"[Auth] Refresh-Dev called - token present: {(!string.IsNullOrEmpty(cookie)).ToString()}");

            var user = _db.Users.FirstOrDefault(u => u.RefreshToken == cookie);
            if (user == null)
            {
                Console.WriteLine("[Auth] Refresh-Dev token not found in DB");
                return Unauthorized(new { title = "Unauthorized", status = 401 });
            }
            if (!user.RefreshTokenExpiry.HasValue || user.RefreshTokenExpiry.Value < DateTime.UtcNow)
            {
                Console.WriteLine($"[Auth] Refresh-Dev token expired for user {user.Id}");
                user.RefreshToken = null;
                user.RefreshTokenExpiry = null;
                await _db.SaveChangesAsync();
                return Unauthorized(new { title = "Unauthorized", status = 401 });
            }

            var newAccess = _token.CreateToken(user);
            var newRefreshBytes = RandomNumberGenerator.GetBytes(64);
            var newRefresh = Convert.ToBase64String(newRefreshBytes);
            var newExpiry = DateTime.UtcNow.AddDays(7);
            user.RefreshToken = newRefresh;
            user.RefreshTokenExpiry = newExpiry;
            await _db.SaveChangesAsync();

            var forwardedProto2 = Request.Headers["X-Forwarded-Proto"].ToString();
            var effectiveIsHttps2 = (!string.IsNullOrEmpty(forwardedProto2) && forwardedProto2.Equals("https", StringComparison.OrdinalIgnoreCase)) || Request.IsHttps;
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = effectiveIsHttps2,
                SameSite = effectiveIsHttps2 ? SameSiteMode.None : SameSiteMode.Lax,
                Expires = user.RefreshTokenExpiry,
                Path = "/",
                IsEssential = true
            };
            if (_env.IsDevelopment())
            {
                cookieOptions.Secure = false;
                cookieOptions.SameSite = SameSiteMode.Lax;
                Console.WriteLine("[Auth] Development override: Secure=false, SameSite=Lax for refresh cookie (refresh-dev)");
            }

            Response.Cookies.Append("refreshToken", newRefresh, cookieOptions);
            var resDto = new AuthResultDto { Token = newAccess };
            if (_env.IsDevelopment()) resDto.RefreshTokenDebug = newRefresh;
            return Ok(resDto);
        }

        // POST: /api/auth/logout
        // Clears the refresh token stored on the server and removes the cookie
        // from the browser so a page reload won't silently restore the session.
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            // read cookie (if present) and clear the stored refresh token
            var cookie = Request.Cookies["refreshToken"];
            Console.WriteLine($"[Auth] Logout called - cookie present: {(!string.IsNullOrEmpty(cookie)).ToString()}");
            if (!string.IsNullOrEmpty(cookie))
            {
                var user = _db.Users.FirstOrDefault(u => u.RefreshToken == cookie);
                if (user != null)
                {
                    user.RefreshToken = null;
                    user.RefreshTokenExpiry = null;
                    await _db.SaveChangesAsync();
                }

                var forwardedProto = Request.Headers["X-Forwarded-Proto"].ToString();
                var effectiveIsHttps = (!string.IsNullOrEmpty(forwardedProto) && forwardedProto.Equals("https", StringComparison.OrdinalIgnoreCase)) || Request.IsHttps;
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = effectiveIsHttps,
                    SameSite = effectiveIsHttps ? SameSiteMode.None : SameSiteMode.Lax,
                    Expires = DateTime.UtcNow.AddDays(-1),
                    Path = "/",
                    IsEssential = true
                };

                if (_env.IsDevelopment())
                {
                    cookieOptions.Secure = false;
                    cookieOptions.SameSite = SameSiteMode.Lax;
                    Console.WriteLine("[Auth] Development override: Secure=false, SameSite=Lax for refresh cookie (logout)");
                }

                // Delete the cookie by setting it expired (Delete supports options)
                Response.Cookies.Delete("refreshToken", cookieOptions);
            }

            return NoContent();
        }
    }
}