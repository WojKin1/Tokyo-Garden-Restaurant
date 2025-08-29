using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using TokyoGarden.Api.Mapping;
using TokyoGarden.IBL;

namespace TokyoGarden.Api.Controllers
{
    // Kontroler API dla operacji na adresach
    [Route("api/[controller]")]
    [ApiController]
    public class AdresyController : ControllerBase
    {
        private readonly IAdresyService _service;

        // Inicjalizacja serwisu przez wstrzykiwanie zależności
        public AdresyController(IAdresyService service)
        {
            _service = service;
        }

        // Pobieranie wszystkich adresów z bazy danych
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _service.GetAllAsync();
            return Ok(list.Select(a => a.ToDto()));
        }

        // Pobieranie adresu po identyfikatorze
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var address = await _service.GetByIdAsync(id);
            if (address == null) return NotFound();
            return Ok(address.ToDto());
        }

        // Tworzenie nowego adresu w bazie danych
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Model.Adresy address)
        {
            await _service.AddAsync(address);
            return CreatedAtAction(nameof(GetById), new { id = address.id }, address.ToDto());
        }

        // Aktualizacja istniejącego adresu
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Model.Adresy address)
        {
            if (id != address.id) return BadRequest();
            await _service.UpdateAsync(address);
            return NoContent();
        }

        // Usuwanie adresu po identyfikatorze
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }

        // Pobieranie adresów po nazwie miasta
        [HttpGet("city/{city}")]
        public async Task<IActionResult> GetByCity(string city)
        {
            var list = await _service.GetByCityAsync(city);
            return Ok(list.Select(a => a.ToDto()));
        }
    }
}