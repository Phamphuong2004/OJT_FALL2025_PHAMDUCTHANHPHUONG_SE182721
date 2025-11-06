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
            .Select(u => new { u.Id, u.Email, u.Role, u.EmailConfirmed, u.LockoutEnd })
            .ToList();
        return Ok(users);
    }

    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
        var user = _db.Users
            .AsNoTracking()
            .Where(u => u.Id == id)
            .Select(u => new { u.Id, u.Email, u.Role, u.EmailConfirmed, u.LockoutEnd })
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
}

public class UpdateRoleDto { public string Role { get; set; } }