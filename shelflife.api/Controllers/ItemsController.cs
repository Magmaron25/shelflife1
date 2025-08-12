using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using shelflife.data;
using shelflife.data.Entities;
using shelflife.DTO;

namespace shelflife.api.Controllers;

/// <summary>
/// Controller exposing CRUD endpoints for shelf items.  Items can be
/// filtered, created, updated and deleted.  Additional endpoints support
/// retrieving items expiring within a specified number of days.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ItemsController : ControllerBase
{
    private readonly ApplicationDbContext _db;
    private readonly IMapper _mapper;

    public ItemsController(ApplicationDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    // GET /api/items?search=&location=&daysToExpire=
    [HttpGet]
    public async Task<ActionResult<IEnumerable<DtoItem>>> Get([FromQuery] string? search, [FromQuery] string? location, [FromQuery] int? daysToExpire)
    {
        var q = _db.Items.AsQueryable();
        if (!string.IsNullOrWhiteSpace(search))
            q = q.Where(i => i.Name.Contains(search));
        if (!string.IsNullOrWhiteSpace(location))
            q = q.Where(i => i.Location == location);
        if (daysToExpire.HasValue)
        {
            var until = DateTime.UtcNow.Date.AddDays(daysToExpire.Value + 1);
            q = q.Where(i => i.ExpiryDate != null && i.ExpiryDate <= until);
        }
        q = q.OrderBy(i => i.ExpiryDate == null).ThenBy(i => i.ExpiryDate);
        var list = await q.ToListAsync();
        return Ok(_mapper.Map<IEnumerable<DtoItem>>(list));
    }

    // GET /api/items/{id}
    [HttpGet("{id:int}")]
    public async Task<ActionResult<DtoItem>> GetById(int id)
    {
        var item = await _db.Items.FindAsync(id);
        if (item == null) return NotFound();
        return Ok(_mapper.Map<DtoItem>(item));
    }

    // POST /api/items
    [HttpPost]
    public async Task<ActionResult> Create([FromBody] DtoCreateItem dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name)) return BadRequest("Name is required.");
        if (dto.Quantity <= 0) return BadRequest("Quantity must be > 0.");
        var entity = _mapper.Map<Item>(dto);
        _db.Items.Add(entity);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = entity.Id }, new { id = entity.Id });
    }

    // PUT /api/items/{id}
    [HttpPut("{id:int}")]
    public async Task<ActionResult> Update(int id, [FromBody] DtoUpdateItem dto)
    {
        var entity = await _db.Items.FindAsync(id);
        if (entity == null) return NotFound();
        _mapper.Map(dto, entity);
        await _db.SaveChangesAsync();
        return NoContent();
    }

    // DELETE /api/items/{id}
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        var entity = await _db.Items.FindAsync(id);
        if (entity == null) return NotFound();
        _db.Items.Remove(entity);
        await _db.SaveChangesAsync();
        return NoContent();
    }

    // GET /api/items/expiring?days=7
    [HttpGet("expiring")]
    public async Task<ActionResult<IEnumerable<DtoItem>>> Expiring([FromQuery] int days = 7)
    {
        var until = DateTime.UtcNow.Date.AddDays(days + 1);
        var list = await _db.Items
            .Where(i => i.ExpiryDate != null && i.ExpiryDate <= until)
            .OrderBy(i => i.ExpiryDate)
            .ToListAsync();
        return Ok(_mapper.Map<IEnumerable<DtoItem>>(list));
    }
}