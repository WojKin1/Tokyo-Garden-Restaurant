using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using TokyoGarden.Api.Mapping;
using TokyoGarden.IBL;
using TokyoGarden.Model;
using BCrypt.Net;

// DODANE:
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace TokyoGarden.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UzytkownicyController : ControllerBase
    {
        // Serwis odpowiedzialny za operacje na danych użytkowników
        private readonly IUzytkownikService _service;
        private readonly IZamowieniaService _zamowieniaService;

        // Konstruktor kontrolera z wstrzykiwaniem zależności serwisów
        public UzytkownicyController(IUzytkownikService service, IZamowieniaService zamowieniaService)
        {
            _service = service;
            _zamowieniaService = zamowieniaService;
        }

        // Zwraca listę wszystkich użytkowników dostępnych w systemie
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _service.GetAllAsync();
            return Ok(list.Select(u => u.ToDto()));
        }

        // Zwraca pojedynczego użytkownika na podstawie jego identyfikatora
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _service.GetByIdAsync(id);
            if (user == null) return NotFound();
            return Ok(user.ToDto());
        }

        // Tworzy nowego użytkownika po uprzednim sprawdzeniu unikalności nazwy
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Uzytkownicy user)
        {
            if (await _service.UsernameExistsAsync(user.nazwa_uzytkownika))
                return BadRequest("Użytkownik o tej nazwie już istnieje");

            // Hashowanie hasła przed zapisaniem użytkownika
            user.haslo = BCrypt.Net.BCrypt.HashPassword(user.haslo);
            await _service.AddAsync(user);

            // Zwraca odpowiedź HTTP 201 z lokalizacją nowo utworzonego użytkownika
            return CreatedAtAction(nameof(GetById), new { id = user.id }, user.ToDto());
        }

        // Aktualizuje dane istniejącego użytkownika na podstawie jego identyfikatora
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Uzytkownicy user)
        {
            if (id != user.id) return BadRequest();

            // Hashowanie hasła, jeśli jest podane
            if (!string.IsNullOrEmpty(user.haslo))
            {
                user.haslo = BCrypt.Net.BCrypt.HashPassword(user.haslo);
            }
            await _service.UpdateAsync(user);
            return NoContent();
        }

        // Usuwa użytkownika z systemu na podstawie jego identyfikatora
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            // Sprawdzenie, czy użytkownik istnieje
            var user = await _service.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound("Użytkownik o podanym identyfikatorze nie istnieje.");
            }

            // Pobranie i usunięcie powiązanych zamówień
            var orders = await _zamowieniaService.GetByUserIdAsync(id);
            foreach (var order in orders)
            {
                await _zamowieniaService.DeleteAsync(order.id);
            }

            // Usunięcie użytkownika
            await _service.DeleteAsync(id);
            return NoContent();
        }

        // Autoryzuje użytkownika na podstawie przesłanych danych logowania
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequest req)
        {
            var user = await _service.GetByUsernameAsync(req.Username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(req.Password, user.haslo))
                return Unauthorized("Błędny login lub hasło");

            // DODANE: zbuduj tożsamość i WYSTAW COOKIE (cookie-auth)
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.id.ToString()),
                new Claim(ClaimTypes.Name, user.nazwa_uzytkownika ?? string.Empty),
                new Claim(ClaimTypes.Role, user.typ_uzytkownika ?? "Uzytkownik")
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            var authProps = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7),
                AllowRefresh = true
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authProps);

            // Zwróć DTO (front może go zapisać w localStorage, ale cookie trzyma sesję)
            return Ok(user.ToDto());
        }

        // DODANE: wylogowanie – usuń cookie autoryzacyjne
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return NoContent();
        }

        // DODANE: zwraca bieżącego zalogowanego użytkownika na podstawie cookie
        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> Me()
        {
            var idStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(idStr)) return Unauthorized();

            if (!int.TryParse(idStr, out var id)) return Unauthorized();

            var user = await _service.GetByIdAsync(id);
            if (user == null) return Unauthorized();

            return Ok(user.ToDto());
        }
    }

    // Klasa pomocnicza reprezentująca dane logowania użytkownika
    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
