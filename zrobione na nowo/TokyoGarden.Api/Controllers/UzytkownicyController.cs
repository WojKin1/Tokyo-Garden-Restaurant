using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using TokyoGarden.Api.Mapping;
using TokyoGarden.IBL;
using TokyoGarden.Model;

namespace TokyoGarden.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UzytkownicyController : ControllerBase
    {
        private readonly IUzytkownikService _service;

        public UzytkownicyController(IUzytkownikService service)
        {
            _service = service;
        }

        // Metoda zwraca listę wszystkich użytkowników w systemie
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _service.GetAllAsync();
            // Tutaj mapujemy encje na DTO aby nie ujawniać całego modelu
            return Ok(list.Select(u => u.ToDto()));
        }

        // Metoda pobiera konkretnego użytkownika po identyfikatorze
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _service.GetByIdAsync(id);
            if (user == null) return NotFound();
            // Jeżeli użytkownik istnieje zwracamy DTO zamiast encji
            return Ok(user.ToDto());
        }

        // Tworzenie nowego użytkownika poprzez przesłanie danych w JSON
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Uzytkownicy user)
        {
            // sprawdzenie czy login jest zajęty przez innego użytkownika
            if ((await _service.GetAllAsync()).Any(u => u.nazwa_uzytkownika == user.nazwa_uzytkownika))
                return BadRequest("Użytkownik o tej nazwie już istnieje");

            // 🔒 hashowanie hasła aby nie przechowywać plaintextu
            user.haslo = BCrypt.Net.BCrypt.HashPassword(user.haslo);

            // zapisujemy użytkownika do bazy danych przez serwis
            await _service.AddAsync(user);
            // zwracamy informację o utworzeniu nowego użytkownika
            return CreatedAtAction(nameof(GetById), new { id = user.id }, user.ToDto());
        }

        // Aktualizacja danych istniejącego użytkownika na podstawie ID
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Uzytkownicy user)
        {
            // Sprawdzamy zgodność identyfikatora w URL i w obiekcie
            if (id != user.id) return BadRequest();
            await _service.UpdateAsync(user);
            // Zwracamy brak treści bo update nie zwraca obiektu
            return NoContent();
        }

        // Usuwanie użytkownika z systemu po identyfikatorze
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            // Zwracamy status 204 aby potwierdzić usunięcie
            return NoContent();
        }

        // Logowanie użytkownika i weryfikacja danych uwierzytelniających
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest req)
        {
            // Szukamy użytkownika po nazwie wśród wszystkich w bazie
            var user = (await _service.GetAllAsync())
                .FirstOrDefault(u => u.nazwa_uzytkownika == req.Username);

            // Jeżeli nie istnieje lub hasło się nie zgadza zwracamy 401
            if (user == null || !BCrypt.Net.BCrypt.Verify(req.Password, user.haslo))
                return Unauthorized("Błędny login lub hasło");

            // Jeśli dane poprawne zwracamy DTO użytkownika
            return Ok(user.ToDto());
        }
    }

    // Klasa pomocnicza reprezentująca dane logowania
    public class LoginRequest
    {
        // Nazwa użytkownika wykorzystywana przy logowaniu
        public string Username { get; set; }
        // Hasło przekazywane do weryfikacji z hashem w bazie
        public string Password { get; set; }
    }
}
