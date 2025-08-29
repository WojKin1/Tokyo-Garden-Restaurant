using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using TokyoGarden.Api.Mapping;
using TokyoGarden.IBL;

namespace TokyoGarden.Api.Controllers
{
    // Kontroler API dla operacji na kategoriach
    [Route("api/[controller]")]
    [ApiController]
    public class KategorieController : ControllerBase
    {
        private readonly IKategorieService _service;

        // Inicjalizacja serwisu przez wstrzykiwanie zależności
        public KategorieController(IKategorieService service)
        {
            _service = service;
        }

        // Pobieranie wszystkich kategorii z bazy danych
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _service.GetAllAsync();
            return Ok(list.Select(k => k.ToDto()));
        }

        // Pobieranie kategorii po identyfikatorze
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _service.GetByIdAsync(id);
            return item == null ? NotFound() : Ok(item.ToDto());
        }

        // Tworzenie nowej kategorii w bazie danych
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Model.Kategorie item)
        {
            await _service.AddAsync(item);
            return CreatedAtAction(nameof(GetById), new { id = item.id }, item.ToDto());
        }

        // Aktualizacja istniejącej kategorii
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Model.Kategorie item)
        {
            if (id != item.id) return BadRequest();
            await _service.UpdateAsync(item);
            return NoContent();
        }

        // Usuwanie kategorii po identyfikatorze
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }

        // Pobieranie kategorii po nazwie
        [HttpGet("by-name/{name}")]
        public async Task<IActionResult> GetByName(string name)
        {
            var cat = await _service.GetByNameAsync(name);
            return cat == null ? NotFound() : Ok(cat.ToDto());
        }
    }
}