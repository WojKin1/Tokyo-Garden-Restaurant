using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

using System.Linq;
using System.Threading.Tasks;

using TokyoGarden.Api.Mapping;
using TokyoGarden.IBL;
using TokyoGarden.Model;
using BCrypt.Net;

namespace TokyoGarden.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UzytkownicyController : ControllerBase
    {
        private readonly IUzytkownikService _service;
        private readonly IZamowieniaService _zamowieniaService;

        public UzytkownicyController(IUzytkownikService service, IZamowieniaService zamowieniaService)
        {
            _service = service;
            _zamowieniaService = zamowieniaService;
        }

        // LISTA
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _service.GetAllAsync();
            return Ok(list.Select(u => u.ToDto()));
        }

        // SZCZEGÓŁY
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _service.GetByIdAsync(id);
            if (user == null) return NotFound();
            return Ok(user.ToDto());
        }

        // REJESTRACJA
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Create([FromBody] Uzytkownicy user)
        {
            if (await _service.UsernameExistsAsync(user.nazwa_uzytkownika))
                return BadRequest("Użytkownik o tej nazwie już istnieje.");

            user.haslo = BCrypt.Net.BCrypt.HashPassword(user.haslo);
            await _service.AddAsync(user);

            return CreatedAtAction(nameof(GetById), new { id = user.id }, user.ToDto());
        }

        // AKTUALIZACJA
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Uzytkownicy user)
        {
            if (id != user.id) return BadRequest();

            if (!string.IsNullOrEmpty(user.haslo))
                user.haslo = BCrypt.Net.BCrypt.HashPassword(user.haslo);

            await _service.UpdateAsync(user);
            return NoContent();
        }

        // USUWANIE
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _service.GetByIdAsync(id);
            if (user == null)
                return NotFound("Użytkownik o podanym identyfikatorze nie istnieje.");

            var orders = await _zamowieniaService.GetByUserIdAsync(id);
            foreach (var order in orders)
                await _zamowieniaService.DeleteAsync(order.id);

            await _service.DeleteAsync(id);
            return NoContent();
        }

        // LOGIN -> ustawia cookie sesyjne
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequest req)
        {
            var user = await _service.GetByUsernameAsync(req.Username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(req.Password, user.haslo))
                return Unauthorized("Błędny login lub hasło.");

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

            // Zwracamy DTO informacyjnie; sesja jest w cookie
            return Ok(user.ToDto());
        }

        // LOGOUT -> usuwa cookie
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return NoContent();
        }

        // BIEŻĄCY UŻYTKOWNIK -> wymaga cookie (withCredentials po stronie frontu)
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

    public class LoginRequest
    {
        public string Username { get; set; } = default!;
        public string Password { get; set; } = default!;
    }
}
