using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using GameStoreMini.Data;
using System.Linq;

namespace GameStoreMini.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _db;
        public UsersController(AppDbContext db) => _db = db;

        private int? CurrentUserId
        {
            get
            {
                var sub = User?.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;
                if (string.IsNullOrEmpty(sub)) return null;
                if (int.TryParse(sub, out var id)) return id;
                return null;
            }
        }

        // GET: /api/users/me
        [HttpGet("me")]
        [Authorize]
        public IActionResult Me()
        {
            var userId = CurrentUserId;
            if (!userId.HasValue) return Unauthorized();

            var user = _db.Users.Find(userId.Value);
            if (user == null) return NotFound();

            return Ok(new
            {
                id = user.Id,
                email = user.Email,
                userName = user.UserName,
                fullName = user.FullName,
                phoneNumber = user.PhoneNumber,
                role = user.Role,
                emailConfirmed = user.EmailConfirmed,
                lockoutEnd = user.LockoutEnd
            });
        }
    }
}
