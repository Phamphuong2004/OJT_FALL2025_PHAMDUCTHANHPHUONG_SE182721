using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GameStoreMini.Data;

[ApiController]
[Route("api/admin/users")]
[Authorize(Roles = "Admin")]
public class AdminUsersController : ControllerBase
{
    private readonly AppDbContext _db;
    public AdminUsersController(AppDbContext db) { _db = db; }

    [HttpGet]
    public IActionResult List([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
            var users = _db.Users
            .AsNoTracking()
            .OrderBy(u => u.Id)
            .Skip((page-1)*pageSize)
            .Take(pageSize)
            .Select(u => new { u.Id, u.Email, u.UserName, u.FullName, u.Role, u.EmailConfirmed, u.LockoutEnd, createdAt = u.CreatedAt, updatedAt = u.UpdatedAt })
            .ToList();
        return Ok(users);
    }

    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
        var user = _db.Users
            .AsNoTracking()
            .Where(u => u.Id == id)
            .Select(u => new { u.Id, u.Email, u.UserName, u.FullName, u.Role, u.EmailConfirmed, u.LockoutEnd, createdAt = u.CreatedAt, updatedAt = u.UpdatedAt })
            .FirstOrDefault();
        if (user == null) return NotFound();
        return Ok(user);
    }

    [HttpPost("{id}/role")]
    public IActionResult UpdateRole(int id, [FromBody] UpdateRoleDto dto)
    {
        if (dto == null || string.IsNullOrWhiteSpace(dto.Role))
            return BadRequest(new { error = "Role is required." });

        var user = _db.Users.Find(id);
        if (user == null) return NotFound();

        user.Role = dto.Role;
        _db.SaveChanges();
        return NoContent();
    }

    [HttpPost("{id}/lock")]
    public IActionResult Lock(int id)
    {
        var user = _db.Users.Find(id);
        if (user == null) return NotFound();
        user.LockoutEnd = DateTime.UtcNow.AddYears(100);
        _db.SaveChanges();
        return NoContent();
    }

    [HttpPost("{id}/unlock")]
    public IActionResult Unlock(int id)
    {
        var user = _db.Users.Find(id);
        if (user == null) return NotFound();
        user.LockoutEnd = null;
        _db.SaveChanges();
        return NoContent();
    }

    [HttpPut("{id}")]
    public IActionResult UpdateUser(int id, [FromBody] UpdateUserDto dto)
    {
        if (dto == null) return BadRequest();
        var user = _db.Users.Find(id);
        if (user == null) return NotFound();
        // Allow updating only FullName for now
        if (dto.FullName != null) user.FullName = dto.FullName;
        _db.SaveChanges();
        return NoContent();
    }

    [HttpPost("populate-fullnames")]
    public IActionResult PopulateFullNames()
    {
        // One-time helper: populate FullName for users where it's missing by deriving
        // from UserName or email local-part. Safe to call multiple times.
        var users = _db.Users.Where(u => string.IsNullOrWhiteSpace(u.FullName)).ToList();
        int updated = 0;
        foreach (var u in users)
        {
            string? candidate = null;
            if (!string.IsNullOrWhiteSpace(u.UserName)) candidate = u.UserName;
            else if (!string.IsNullOrWhiteSpace(u.Email)) candidate = u.Email.Split('@')[0];
            if (string.IsNullOrWhiteSpace(candidate)) continue;
            // simple cleanup: replace separators with space and capitalize
            var cleaned = System.Text.RegularExpressions.Regex.Replace(candidate, "[_\\.\\-]+", " ");
            var parts = cleaned.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(p => p.Length > 0 ? char.ToUpper(p[0]) + p.Substring(1).ToLower() : p)
                .ToArray();
            var full = string.Join(' ', parts);
            if (!string.IsNullOrWhiteSpace(full))
            {
                u.FullName = full;
                updated++;
            }
        }
        _db.SaveChanges();
        return Ok(new { updated });
    }
}

public class UpdateRoleDto { public string Role { get; set; } }
public class UpdateUserDto { public string? FullName { get; set; } }