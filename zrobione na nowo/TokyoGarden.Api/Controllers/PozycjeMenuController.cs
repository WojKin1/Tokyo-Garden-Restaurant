using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using TokyoGarden.Api.Mapping;
using TokyoGarden.IBL;
using TokyoGarden.Model;
using Newtonsoft.Json;

namespace TokyoGarden.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PozycjeMenuController : ControllerBase
    {
        // Serwis odpowiedzialny za operacje na pozycjach menu w systemie
        private readonly IPozycjeMenuService _service;

        // Konstruktor kontrolera z wstrzykiwaniem zależności serwisu pozycji menu
        public PozycjeMenuController(IPozycjeMenuService service)
        {
            _service = service;
        }

        // Zwraca listę wszystkich pozycji menu wraz z powiązanymi danymi
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _service.GetAllWithDetailsAsync();
            return Ok(list.Select(p => p.ToDto()));
        }

        // Zwraca konkretną pozycję menu na podstawie jej identyfikatora
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var list = await _service.GetAllWithDetailsAsync();
            var item = list.FirstOrDefault(p => p.id == id);
            if (item == null) return NotFound();
            return Ok(item.ToDto());
        }

        // Tworzy nową pozycję menu na podstawie danych przesłanych w formacie JSON
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Pozycje_Menu item,
            [FromServices] IKategorieService kategoriaService)
        {
            if (item.kategoria_menu == null || item.kategoria_menu.id == 0)
                return BadRequest("Brak wybranej kategorii.");

            // Pobiera istniejącą kategorię z bazy danych na podstawie identyfikatora
            var kat = await kategoriaService.GetByIdAsync(item.kategoria_menu.id);
            if (kat == null)
                return BadRequest("Nie znaleziono kategorii o podanym id.");

            // Przypisuje istniejącą kategorię do nowej pozycji menu
            item.kategoria_menu = kat;

            await _service.AddAsync(item);

            // Pobiera świeżo dodaną pozycję menu z pełnymi szczegółami
            var list = await _service.GetAllWithDetailsAsync();
            var fresh = list.FirstOrDefault(p => p.id == item.id);

            return CreatedAtAction(nameof(GetById), new { id = item.id }, fresh?.ToDto() ?? item.ToDto());
        }

        // Aktualizuje istniejącą pozycję menu na podstawie identyfikatora i przesłanych danych
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Pozycje_Menu item,
            [FromServices] IKategorieService kategoriaService)
        {
            if (id != item.id)
                return BadRequest("ID w URL nie zgadza się z ID w body.");

            if (item.kategoria_menu == null || item.kategoria_menu.id == 0)
                return BadRequest("Brak dostępnej kategorii.");

            var existing = await _service.GetByIdAsync(id);
            if (existing == null)
                return NotFound();

            // Aktualizuje podstawowe dane pozycji menu
            existing.nazwa_pozycji = item.nazwa_pozycji;
            existing.opis = item.opis;
            existing.cena = item.cena;
            existing.skladniki = item.skladniki;

            // Pobiera i przypisuje istniejącą kategorię do aktualizowanej pozycji
            var kat = await kategoriaService.GetByIdAsync(item.kategoria_menu.id);
            if (kat == null)
                return BadRequest("Nie znaleziono kategorii o podanym id.");

            existing.kategoria_menu = kat;

            await _service.UpdateAsync(existing);

            return NoContent();
        }

        // Usuwa pozycję menu z systemu na podstawie jej identyfikatora
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }

        // Zwraca listę pozycji menu przypisanych do konkretnej kategorii
        [HttpGet("category/{categoryId}")]
        public async Task<IActionResult> GetByCategory(int categoryId)
        {
            var list = await _service.GetByCategoryIdAsync(categoryId);
            return Ok(list.Select(p => p.ToDto()));
        }

        // Wyszukuje pozycje menu na podstawie ich nazwy przekazanej w adresie URL
        [HttpGet("search/{name}")]
        public async Task<IActionResult> GetByName(string name)
        {
            var list = await _service.SearchByNameAsync(name);
            return Ok(list.Select(p => p.ToDto()));
        }

        // Zwraca listę pozycji menu zawierających wskazany alergen
        [HttpGet("allergen/{allergenId}")]
        public async Task<IActionResult> GetByAllergen(int allergenId)
        {
            var list = await _service.GetByAllergenAsync(allergenId);
            return Ok(list.Select(p => p.ToDto()));
        }
    }
}
