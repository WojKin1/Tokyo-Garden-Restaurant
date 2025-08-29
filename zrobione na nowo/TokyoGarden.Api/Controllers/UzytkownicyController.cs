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

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _service.GetAllAsync();
            return Ok(list.Select(u => u.ToDto()));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _service.GetByIdAsync(id);
            if (user == null) return NotFound();
            return Ok(user.ToDto());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Uzytkownicy user)
        {
            if (await _service.UsernameExistsAsync(user.nazwa_uzytkownika))
                return BadRequest("Użytkownik o tej nazwie już istnieje");

            await _service.AddAsync(user);
            return CreatedAtAction(nameof(GetById), new { id = user.id }, user.ToDto());
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Uzytkownicy user)
        {
            if (id != user.id) return BadRequest();
            await _service.UpdateAsync(user);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest req)
        {
            var user = await _service.AuthenticateAsync(req.Username, req.Password);
            if (user == null) return Unauthorized("Błędny login lub hasło");
            return Ok(user.ToDto());
        }
    }

    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
