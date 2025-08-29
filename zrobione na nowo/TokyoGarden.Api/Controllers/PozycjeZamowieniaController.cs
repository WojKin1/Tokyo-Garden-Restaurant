using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using TokyoGarden.Api.Mapping;
using TokyoGarden.IBL;

namespace TokyoGarden.Api.Controllers
{
    // Kontroler API dla operacji na pozycjach zamówienia
    [Route("api/[controller]")]
    [ApiController]
    public class PozycjeZamowieniaController : ControllerBase
    {
        private readonly IPozycjeZamowieniaService _service;

        // Inicjalizacja serwisu przez wstrzykiwanie zależności
        public PozycjeZamowieniaController(IPozycjeZamowieniaService service)
        {
            _service = service;
        }

        // Pobieranie wszystkich pozycji zamówienia z detalami
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _service.GetAllWithDetailsAsync();
            return Ok(list.Select(p => p.ToDto()));
        }

        // Pobieranie pozycji zamówienia po identyfikatorze
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var all = await _service.GetAllWithDetailsAsync();
            var item = all.FirstOrDefault(p => p.id == id);
            if (item == null) return NotFound();
            return Ok(item.ToDto());
        }

        // Tworzenie nowej pozycji zamówienia w bazie danych
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Model.Pozycje_Zamowienia item)
        {
            await _service.AddAsync(item);
            var all = await _service.GetAllWithDetailsAsync();
            var fresh = all.FirstOrDefault(p => p.id == item.id);
            return CreatedAtAction(nameof(GetById), new { id = item.id }, fresh?.ToDto() ?? item.ToDto());
        }

        // Aktualizacja istniejącej pozycji zamówienia
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Model.Pozycje_Zamowienia item)
        {
            if (id != item.id) return BadRequest();
            await _service.UpdateAsync(item);
            return NoContent();
        }

        // Usuwanie pozycji zamówienia po identyfikatorze
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }

        // Pobieranie pozycji zamówienia po identyfikatorze zamówienia
        [HttpGet("order/{orderId}")]
        public async Task<IActionResult> GetByOrder(int orderId)
        {
            var list = await _service.GetByOrderIdAsync(orderId);
            return Ok(list.Select(p => p.ToDto()));
        }
    }
}