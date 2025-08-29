using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using TokyoGarden.Api.Mapping;
using TokyoGarden.IBL;

namespace TokyoGarden.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ZamowieniaController : ControllerBase
    {
        private readonly IZamowieniaService _service;
        public ZamowieniaController(IZamowieniaService service) { _service = service; }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _service.GetAllWithDetailsAsync();
            return Ok(list.Select(z => z.ToDto()));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _service.GetByIdWithDetailsAsync(id);
            if (item == null) return NotFound();
            return Ok(item.ToDto());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Model.Zamowienia item)
        {
            await _service.AddAsync(item);
            var fresh = await _service.GetByIdWithDetailsAsync(item.id);
            return CreatedAtAction(nameof(GetById), new { id = item.id }, fresh.ToDto());
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Model.Zamowienia item)
        {
            if (id != item.id) return BadRequest();
            await _service.UpdateAsync(item);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }

        [HttpPost("{id}/recalculate")]
        public async Task<IActionResult> RecalculateTotal(int id)
        {
            var updated = await _service.RecalculateTotalAsync(id);
            return updated == null ? NotFound() : Ok(updated.ToDto());
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUser(int userId)
        {
            var list = await _service.GetByUserIdAsync(userId);
            return Ok(list.Select(z => z.ToDto()));
        }

        [HttpGet("status/{status}")]
        public async Task<IActionResult> GetByStatus(string status)
        {
            var list = await _service.GetByStatusAsync(status);
            return Ok(list.Select(z => z.ToDto()));
        }
    }
}
