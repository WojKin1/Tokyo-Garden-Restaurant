using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using TokyoGarden.Api.Mapping;
using TokyoGarden.IBL;

namespace TokyoGarden.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PozycjeZamowieniaController : ControllerBase
    {
        private readonly IPozycjeZamowieniaService _service;
        public PozycjeZamowieniaController(IPozycjeZamowieniaService service) { _service = service; }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _service.GetAllWithDetailsAsync();
            return Ok(list.Select(p => p.ToDto()));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var all = await _service.GetAllWithDetailsAsync();
            var item = all.FirstOrDefault(p => p.id == id);
            if (item == null) return NotFound();
            return Ok(item.ToDto());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Model.Pozycje_Zamowienia item)
        {
            await _service.AddAsync(item);
            var all = await _service.GetAllWithDetailsAsync();
            var fresh = all.FirstOrDefault(p => p.id == item.id);
            return CreatedAtAction(nameof(GetById), new { id = item.id }, fresh?.ToDto() ?? item.ToDto());
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Model.Pozycje_Zamowienia item)
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

        [HttpGet("order/{orderId}")]
        public async Task<IActionResult> GetByOrder(int orderId)
        {
            var list = await _service.GetByOrderIdAsync(orderId);
            return Ok(list.Select(p => p.ToDto()));
        }
    }
}
