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
        // Serwis odpowiedzialny za operacje na pozycjach zamówienia w systemie
        private readonly IPozycjeZamowieniaService _service;

        // Konstruktor kontrolera z wstrzykiwaniem zależności serwisu pozycji zamówienia
        public PozycjeZamowieniaController(IPozycjeZamowieniaService service)
        {
            _service = service;
        }

        // Zwraca listę wszystkich pozycji zamówienia wraz z powiązanymi szczegółami
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _service.GetAllWithDetailsAsync();
            return Ok(list.Select(p => p.ToDto()));
        }

        // Zwraca konkretną pozycję zamówienia na podstawie jej identyfikatora
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var all = await _service.GetAllWithDetailsAsync();
            var item = all.FirstOrDefault(p => p.id == id);
            if (item == null) return NotFound();
            return Ok(item.ToDto());
        }

        // Tworzy nową pozycję zamówienia na podstawie przesłanych danych
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Model.Pozycje_Zamowienia item)
        {
            await _service.AddAsync(item);

            // Pobiera świeżo dodaną pozycję zamówienia z pełnymi szczegółami
            var all = await _service.GetAllWithDetailsAsync();
            var fresh = all.FirstOrDefault(p => p.id == item.id);

            return CreatedAtAction(nameof(GetById), new { id = item.id }, fresh?.ToDto() ?? item.ToDto());
        }

        // Aktualizuje istniejącą pozycję zamówienia na podstawie identyfikatora
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Model.Pozycje_Zamowienia item)
        {
            if (id != item.id) return BadRequest();
            await _service.UpdateAsync(item);
            return NoContent();
        }

        // Usuwa pozycję zamówienia z systemu na podstawie jej identyfikatora
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }

        // Zwraca listę pozycji zamówienia przypisanych do konkretnego zamówienia
        [HttpGet("order/{orderId}")]
        public async Task<IActionResult> GetByOrder(int orderId)
        {
            var list = await _service.GetByOrderIdAsync(orderId);
            return Ok(list.Select(p => p.ToDto()));
        }
    }
}
