using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using TokyoGarden.Api.Mapping;
using TokyoGarden.IBL;

namespace TokyoGarden.Api.Controllers
{
    // Kontroler API dla operacji na zamówieniach
    [Route("api/[controller]")]
    [ApiController]
    public class ZamowieniaController : ControllerBase
    {
        private readonly IZamowieniaService _service;

        // Inicjalizacja serwisu przez wstrzykiwanie zależności
        public ZamowieniaController(IZamowieniaService service)
        {
            _service = service;
        }

        // Pobieranie wszystkich zamówień z detalami
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _service.GetAllWithDetailsAsync();
            return Ok(list.Select(z => z.ToDto()));
        }

        // Pobieranie zamówienia po identyfikatorze z detalami
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _service.GetByIdWithDetailsAsync(id);
            if (item == null) return NotFound();
            return Ok(item.ToDto());
        }

        // Tworzenie nowego zamówienia w bazie danych
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Model.Zamowienia item)
        {
            await _service.AddAsync(item);
            var fresh = await _service.GetByIdWithDetailsAsync(item.id);
            return CreatedAtAction(nameof(GetById), new { id = item.id }, fresh.ToDto());
        }

        // Aktualizacja istniejącego zamówienia
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Model.Zamowienia item)
        {
            if (id != item.id) return BadRequest();
            await _service.UpdateAsync(item);
            return NoContent();
        }

        // Usuwanie zamówienia po identyfikatorze
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }

        // Ponowne obliczanie sumy zamówienia
        [HttpPost("{id}/recalculate")]
        public async Task<IActionResult> RecalculateTotal(int id)
        {
            var updated = await _service.RecalculateTotalAsync(id);
            return updated == null ? NotFound() : Ok(updated.ToDto());
        }

        // Pobieranie zamówień po identyfikatorze użytkownika
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUser(int userId)
        {
            var list = await _service.GetByUserIdAsync(userId);
            return Ok(list.Select(z => z.ToDto()));
        }

        // Pobieranie zamówień po statusie
        [HttpGet("status/{status}")]
        public async Task<IActionResult> GetByStatus(string status)
        {
            var list = await _service.GetByStatusAsync(status);
            return Ok(list.Select(z => z.ToDto()));
        }
    }
}