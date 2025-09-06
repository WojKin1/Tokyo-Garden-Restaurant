using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using TokyoGarden.Api.Mapping;
using TokyoGarden.IBL;

namespace TokyoGarden.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KategorieController : ControllerBase
    {
        // Serwis odpowiedzialny za operacje na danych kategorii produktów
        private readonly IKategorieService _service;

        // Konstruktor kontrolera z wstrzykiwaniem zależności serwisu kategorii
        public KategorieController(IKategorieService service)
        {
            _service = service;
        }

        // Zwraca listę wszystkich kategorii dostępnych w bazie danych
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _service.GetAllAsync();
            return Ok(list.Select(k => k.ToDto()));
        }

        // Zwraca pojedynczą kategorię na podstawie jej identyfikatora
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _service.GetByIdAsync(id);
            return item == null ? NotFound() : Ok(item.ToDto());
        }

        // Tworzy nową kategorię na podstawie danych przesłanych w żądaniu
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Model.Kategorie item)
        {
            await _service.AddAsync(item);

            // Zwraca odpowiedź HTTP 201 z lokalizacją nowo utworzonej kategorii
            return CreatedAtAction(nameof(GetById), new { id = item.id }, item.ToDto());
        }

        // Aktualizuje dane istniejącej kategorii na podstawie identyfikatora
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Model.Kategorie item)
        {
            if (id != item.id) return BadRequest();
            await _service.UpdateAsync(item);
            return NoContent();
        }

        // Usuwa kategorię z bazy danych na podstawie jej identyfikatora
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }

        // Zwraca kategorię na podstawie jej nazwy przekazanej w adresie URL
        [HttpGet("by-name/{name}")]
        public async Task<IActionResult> GetByName(string name)
        {
            var cat = await _service.GetByNameAsync(name);
            return cat == null ? NotFound() : Ok(cat.ToDto());
        }
    }
}
