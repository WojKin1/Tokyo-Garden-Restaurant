using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using TokyoGarden.Api.DTOs;
using TokyoGarden.Api.Mapping;
using TokyoGarden.IBL;
using TokyoGarden.Model;

namespace TokyoGarden.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ZamowieniaController : ControllerBase
    {
        private readonly IZamowieniaService _service;
        private readonly IUzytkownikService _userService;

        public ZamowieniaController(IZamowieniaService service, IUzytkownikService userService)
        {
            _service = service;
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _service.GetAllWithDetailsAsync();
            var users = await _userService.GetAllAsync();

            var dtos = list.Select(order =>
            {
                var dto = order.ToDto();
                // Przypisanie użytkownika na podstawie obiektu encji (jeżeli istnieje)
                if (dto.Uzytkownik == null && order.uzytkownik != null)
                    dto.Uzytkownik = order.uzytkownik.ToDto();
                return dto;
            });

            return Ok(dtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _service.GetByIdWithDetailsAsync(id);
            if (item == null) return NotFound();

            var dto = item.ToDto();
            if (dto.Uzytkownik == null && item.uzytkownik != null)
                dto.Uzytkownik = item.uzytkownik.ToDto();

            return Ok(dto);
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Zamowienia item)
        {
            if (item.uzytkownik != null && item.uzytkownik.id > 0)
            {
                var currentUser = await _userService.GetByIdAsync(item.uzytkownik.id);
                if (currentUser != null)
                    item.uzytkownik = currentUser;
                else
                    return BadRequest("Nie znaleziono użytkownika o podanym ID.");
            }
            else
            {
                return BadRequest("Brak danych użytkownika w zamówieniu.");
            }
            item.status_zamowienia = "Nowy";

            await _service.AddAsync(item);
            var fresh = await _service.GetByIdWithDetailsAsync(item.id);
            return CreatedAtAction(nameof(GetById), new { id = item.id }, fresh.ToDto());
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Model.Zamowienia item)
        {
            if (id != item.id) return BadRequest();
            await _service.UpdateAsync(item);
            return NoContent();
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] JsonElement statusData)
        {
            try
            {
                var order = await _service.GetByIdWithDetailsAsync(id);
                if (order == null)
                {
                    return NotFound($"Zamówienie o ID {id} nie istnieje.");
                }

                if (!statusData.TryGetProperty("statusZamowienia", out var statusProperty))
                {
                    return BadRequest("Brak pola 'statusZamowienia' w żądaniu.");
                }

                string newStatus = statusProperty.GetString();
                if (string.IsNullOrEmpty(newStatus))
                {
                    return BadRequest("Pole 'statusZamowienia' nie może być puste.");
                }

                var validStatuses = new List<string> { "Nowy", "W realizacji", "Wyslany", "Zakonczony", "Anulowany" };
                if (!validStatuses.Contains(newStatus))
                {
                    return BadRequest($"Nieprawidłowy status zamówienia: {newStatus}. Dozwolone statusy: {string.Join(", ", validStatuses)}");
                }

                order.status_zamowienia = newStatus;
                await _service.UpdateAsync(order);
                return Ok(order.ToDto());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd podczas aktualizacji statusu zamówienia ID {id}: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                return StatusCode(500, $"Wystąpił błąd serwera: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }

        [HttpPost("{id}/recalculate")]
        public async Task<IActionResult> RecalculateTotal(int id)
        {
            var updated = await _service.RecalculateTotalAsync(id);
            if (updated == null) return NotFound();

            var dto = updated.ToDto();
            if (dto.Uzytkownik == null && updated.uzytkownik != null)
                dto.Uzytkownik = updated.uzytkownik.ToDto();

            return Ok(dto);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUser(int userId)
        {
            var list = await _service.GetByUserIdAsync(userId);
            var allUsers = await _userService.GetAllAsync();

            var dtoList = list.Select(z =>
            {
                var dto = z.ToDto();
                if (dto.Uzytkownik == null && z.uzytkownik != null)
                    dto.Uzytkownik = z.uzytkownik.ToDto();
                return dto;
            }).ToList();

            return Ok(dtoList);
        }

        [HttpGet("status/{status}")]
        public async Task<IActionResult> GetByStatus(string status)
        {
            var list = await _service.GetByStatusAsync(status);
            var allUsers = await _userService.GetAllAsync();

            var dtoList = list.Select(z =>
            {
                var dto = z.ToDto();
                if (dto.Uzytkownik == null && z.uzytkownik != null)
                    dto.Uzytkownik = z.uzytkownik.ToDto();
                return dto;
            }).ToList();

            return Ok(dtoList);
        }
    }
}
