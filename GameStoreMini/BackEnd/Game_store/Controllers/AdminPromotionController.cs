using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GameStoreMini.Data;
using Game_store.Dtos;
using Game_store.Models;
using GameStoreMini.Dtos;
using GameStoreMini.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System;
using System.Linq;

namespace GameStoreMini.Controllers
{
	[ApiController]
	[Route("api/admin/promotions")]
	[Authorize(Roles = Roles.Admin)]
	public class AdminPromotionController : ControllerBase
	{
		private readonly AppDbContext _db;
		private readonly IWebHostEnvironment _env;
		public AdminPromotionController(AppDbContext db, IWebHostEnvironment env) => (_db, _env) = (db, env);

		// GET: api/admin/promotions
		[HttpGet]
		public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 20, [FromQuery] string? q = null)
		{
			if (page <= 0) page = 1;
			if (pageSize <= 0) pageSize = 20;

			var query = _db.Promotions.AsQueryable();
			if (!string.IsNullOrWhiteSpace(q))
			{
				var pattern = $"%{q}%";
				query = query.Where(p => EF.Functions.ILike(p.Title, pattern) || EF.Functions.ILike(p.Content, pattern));
			}

			var total = await query.CountAsync();
			var items = await query
				.Include(p => p.PromotionGames)
				.ThenInclude(pg => pg.Game)
				.OrderByDescending(p => p.CreatedAt)
				.Skip((page - 1) * pageSize)
				.Take(pageSize)
				.ToListAsync();

			return Ok(new { data = items, total, page, pageSize });
		}

		// GET: api/admin/promotions/{id}
		[HttpGet("{id:int}")]
		public async Task<IActionResult> Get(int id)
		{
			var p = await _db.Promotions
				.Include(x => x.PromotionGames)
				.ThenInclude(pg => pg.Game)
				.FirstOrDefaultAsync(x => x.Id == id);
			if (p == null) return NotFound();
			return Ok(p);
		}

		// POST: api/admin/promotions
		[HttpPost]
		public async Task<IActionResult> Create([FromForm] CreatePromotionDto dto)
		{
			if (!ModelState.IsValid) return BadRequest(ModelState);

			string? imageUrl = null;
			if (dto.Image != null)
			{
				var uploads = Path.Combine(_env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"), "uploads", "promotions");
				Directory.CreateDirectory(uploads);
				var fileName = $"{Guid.NewGuid()}_{dto.Image.FileName}";
				var path = Path.Combine(uploads, fileName);
				using var fs = new FileStream(path, FileMode.Create);
				await dto.Image.CopyToAsync(fs);
				imageUrl = $"/uploads/promotions/{fileName}";
			}

			var p = new Promotion
			{
				Title = dto.Title,
				Content = dto.Content,
				Summary = dto.Summary,
				ImageUrl = imageUrl,
				Slug = string.IsNullOrWhiteSpace(dto.Slug) ? dto.Title.ToLower().Replace(' ', '-') : dto.Slug,
				StartDate = dto.StartDate,
				EndDate = dto.EndDate,
				DiscountPercentage = dto.DiscountPercentage,
				FixedDiscountAmount = dto.FixedDiscountAmount,
				PromotionType = dto.PromotionType,
				EventType = dto.EventType,
				IsActive = dto.IsActive,
				IsFeatured = dto.IsFeatured,
				CreatedAt = DateTime.UtcNow,
				UpdatedAt = DateTime.UtcNow,
				CreatedBy = User.Identity?.Name ?? "admin"
			};

			_db.Promotions.Add(p);
			await _db.SaveChangesAsync();

			if (dto.GameIds != null && dto.GameIds.Any())
			{
				var pgs = dto.GameIds.Select(gid => new PromotionGame { PromotionId = p.Id, GameId = gid });
				_db.PromotionGames.AddRange(pgs);
				await _db.SaveChangesAsync();
			}

			return CreatedAtAction(nameof(Get), new { id = p.Id }, p);
		}

		// PUT: api/admin/promotions/{id}
		[HttpPut("{id:int}")]
		public async Task<IActionResult> Update(int id, [FromForm] UpdatePromotionDto dto)
		{
			var p = await _db.Promotions.Include(x => x.PromotionGames).FirstOrDefaultAsync(x => x.Id == id);
			if (p == null) return NotFound();

			if (dto.Image != null)
			{
				var uploads = Path.Combine(_env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"), "uploads", "promotions");
				Directory.CreateDirectory(uploads);
				var fileName = $"{Guid.NewGuid()}_{dto.Image.FileName}";
				var path = Path.Combine(uploads, fileName);
				using var fs = new FileStream(path, FileMode.Create);
				await dto.Image.CopyToAsync(fs);
				p.ImageUrl = $"/uploads/promotions/{fileName}";
			}

			p.Title = dto.Title;
			p.Content = dto.Content;
			p.Summary = dto.Summary;
			p.Slug = string.IsNullOrWhiteSpace(dto.Slug) ? p.Slug : dto.Slug;
			p.StartDate = dto.StartDate;
			p.EndDate = dto.EndDate;
			p.DiscountPercentage = dto.DiscountPercentage;
			p.FixedDiscountAmount = dto.FixedDiscountAmount;
			p.PromotionType = dto.PromotionType;
			p.EventType = dto.EventType;
			p.IsActive = dto.IsActive;
			p.IsFeatured = dto.IsFeatured;
			p.UpdatedAt = DateTime.UtcNow;

			// replace games
			_db.PromotionGames.RemoveRange(p.PromotionGames);
			if (dto.GameIds != null && dto.GameIds.Any())
			{
				var pgs = dto.GameIds.Select(gid => new PromotionGame { PromotionId = p.Id, GameId = gid });
				_db.PromotionGames.AddRange(pgs);
			}

			await _db.SaveChangesAsync();
			return NoContent();
		}

		// DELETE: api/admin/promotions/{id}
		[HttpDelete("{id:int}")]
		public async Task<IActionResult> Delete(int id)
		{
			var p = await _db.Promotions.FindAsync(id);
			if (p == null) return NotFound();
			_db.Promotions.Remove(p);
			await _db.SaveChangesAsync();
			return NoContent();
		}

		// PATCH: api/admin/promotions/{id}/toggle-status
		[HttpPatch("{id:int}/toggle-status")]
		public async Task<IActionResult> ToggleStatus(int id)
		{
			var p = await _db.Promotions.FindAsync(id);
			if (p == null) return NotFound();
			p.IsActive = !p.IsActive;
			p.UpdatedAt = DateTime.UtcNow;
			await _db.SaveChangesAsync();
			return Ok(new { p.Id, p.IsActive });
		}

		// PATCH: api/admin/promotions/{id}/toggle-featured
		[HttpPatch("{id:int}/toggle-featured")]
		public async Task<IActionResult> ToggleFeatured(int id)
		{
			var p = await _db.Promotions.FindAsync(id);
			if (p == null) return NotFound();
			p.IsFeatured = !p.IsFeatured;
			p.UpdatedAt = DateTime.UtcNow;
			await _db.SaveChangesAsync();
			return Ok(new { p.Id, p.IsFeatured });
		}

		// GET: api/admin/promotions/stats
		[HttpGet("stats")]
		public async Task<IActionResult> GetStats()
		{
			var now = DateTime.UtcNow;
			var total = await _db.Promotions.CountAsync();
			var active = await _db.Promotions.CountAsync(p => p.IsActive && p.StartDate <= now && p.EndDate >= now);
			var expired = await _db.Promotions.CountAsync(p => p.EndDate < now);
			var featured = await _db.Promotions.CountAsync(p => p.IsFeatured && p.IsActive);
			var avg = await _db.Promotions.Where(p => p.PromotionType == "PERCENTAGE").AverageAsync(p => (decimal?)p.DiscountPercentage) ?? 0;
			var totalGames = await _db.PromotionGames.CountAsync();

			return Ok(new PromotionStatsDto
			{
				TotalPromotions = total,
				ActivePromotions = active,
				ExpiredPromotions = expired,
				FeaturedPromotions = featured,
				AverageDiscountPercentage = avg,
				TotalGamesInPromotions = totalGames
			});
		}

		// GET: api/admin/promotions/available-games?q=...
		[HttpGet("available-games")]
		public async Task<IActionResult> GetAvailableGames([FromQuery] string? q = null)
		{
			var query = _db.Games.AsQueryable();
			if (!string.IsNullOrWhiteSpace(q)) query = query.Where(g => g.Title.Contains(q));
			var items = await query.OrderBy(g => g.Title).Select(g => new { g.Id, g.Title, g.Price, g.Stock, g.ImageUrl }).Take(50).ToListAsync();
			return Ok(items);
		}
	}
}
