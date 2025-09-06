using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using TokyoGarden.Api.Mapping;
using TokyoGarden.IBL;

namespace TokyoGarden.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdresyController : ControllerBase
    {
        // Serwis odpowiedzialny za operacje na danych adresowych
        private readonly IAdresyService _service;

        // Konstruktor kontrolera z wstrzykiwaniem zależności serwisu adresowego
        public AdresyController(IAdresyService service)
        {
            _service = service;
        }

        // Zwraca listę wszystkich adresów dostępnych w bazie danych
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _service.GetAllAsync();
            return Ok(list.Select(a => a.ToDto()));
        }

        // Zwraca pojedynczy adres na podstawie jego identyfikatora
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var address = await _service.GetByIdAsync(id);
            if (address == null) return NotFound();
            return Ok(address.ToDto());
        }

        // Tworzy nowy adres na podstawie danych przesłanych w formacie JSON
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] JsonElement body)
        {
            // Tworzy obiekt adresu na podstawie właściwości JSON
            var address = new Model.Adresy
            {
                miasto = body.GetProperty("miasto").GetString(),
                ulica = body.GetProperty("ulica").GetString(),
                nr_domu = body.TryGetProperty("nr_domu", out var nrDomu) ? nrDomu.GetString() : null,
                nr_mieszkania = body.TryGetProperty("nr_mieszkania", out var nrMieszkania) ? nrMieszkania.GetString() : null
            };

            // Dodaje nowy adres do bazy danych za pomocą serwisu
            await _service.AddAsync(address);

            // Zwraca odpowiedź HTTP 201 z lokalizacją nowego zasobu
            return CreatedAtAction(nameof(GetById), new { id = address.id }, address.ToDto());
        }

        // Aktualizuje istniejący adres na podstawie przesłanych danych
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Model.Adresy address)
        {
            if (id != address.id) return BadRequest();
            await _service.UpdateAsync(address);
            return NoContent();
        }

        // Usuwa adres z bazy danych na podstawie jego identyfikatora
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }

        // Zwraca listę adresów znajdujących się w podanym mieście
        [HttpGet("city/{city}")]
        public async Task<IActionResult> GetByCity(string city)
        {
            var list = await _service.GetByCityAsync(city);
            return Ok(list.Select(a => a.ToDto()));
        }
    }
}
