using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GameStoreMini.Data;

namespace GameStoreMini.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LocationsController : ControllerBase
    {
        private readonly AppDbContext _db;
        public LocationsController(AppDbContext db) => _db = db;

        // GET: /api/locations
        // Returns all locations (small seed). Frontend can filter or request distinct lists.
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _db.Locations.AsNoTracking().ToListAsync();
            return Ok(list);
        }

        // GET: /api/locations/cities?country=Việt Nam
        [HttpGet("cities")]
        public async Task<IActionResult> GetCities([FromQuery] string? country)
        {
            var q = _db.Locations.AsNoTracking().AsQueryable();
            if (!string.IsNullOrEmpty(country)) q = q.Where(l => l.Country == country);
            var cities = await q.Select(l => l.City).Distinct().ToListAsync();
            return Ok(cities);
        }

        // GET: /api/locations/districts?city=Hà Nội
        [HttpGet("districts")]
        public async Task<IActionResult> GetDistricts([FromQuery] string? city)
        {
            if (string.IsNullOrEmpty(city))
                return BadRequest("City parameter is required");

            var districts = await _db.Locations
                .AsNoTracking()
                .Where(l => l.City == city && !string.IsNullOrEmpty(l.District))
                .Select(l => l.District!)
                .Distinct()
                .ToListAsync();

            return Ok(districts);
        }

        // GET: /api/locations/wards?city=Hà Nội&district=Ba Đình
        [HttpGet("wards")]
        public async Task<IActionResult> GetWards([FromQuery] string? city, [FromQuery] string? district)
        {
            if (string.IsNullOrEmpty(city))
                return BadRequest("City parameter is required");
            
            if (string.IsNullOrEmpty(district))
                return BadRequest("District parameter is required");

            var wards = await _db.Locations
                .AsNoTracking()
                .Where(l => l.City == city && l.District == district && !string.IsNullOrEmpty(l.Ward))
                .Select(l => l.Ward!)
                .Distinct()
                .OrderBy(w => w)
                .ToListAsync();

            return Ok(wards);
        }
    }
}
