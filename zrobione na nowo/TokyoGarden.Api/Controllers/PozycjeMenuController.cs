using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using TokyoGarden.Api.Mapping;
using TokyoGarden.IBL;

namespace TokyoGarden.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PozycjeMenuController : ControllerBase
    {
        private readonly IPozycjeMenuService _service;

        public PozycjeMenuController(IPozycjeMenuService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _service.GetAllWithDetailsAsync();
            return Ok(list.Select(p => p.ToDto()));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var list = await _service.GetAllWithDetailsAsync();
            var item = list.FirstOrDefault(p => p.id == id);
            if (item == null) return NotFound();
            return Ok(item.ToDto());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Model.Pozycje_Menu item)
        {
            await _service.AddAsync(item);
            var list = await _service.GetAllWithDetailsAsync();
            var fresh = list.FirstOrDefault(p => p.id == item.id);
            return CreatedAtAction(nameof(GetById), new { id = item.id }, fresh?.ToDto() ?? item.ToDto());
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Model.Pozycje_Menu item)
        {
            if (id != item.id) return BadRequest();
            await _service.UpdateAsync(item);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }

        [HttpGet("category/{categoryId}")]
        public async Task<IActionResult> GetByCategory(int categoryId)
        {
            var list = await _service.GetByCategoryIdAsync(categoryId);
            return Ok(list.Select(p => p.ToDto()));
        }

        [HttpGet("search/{name}")]
        public async Task<IActionResult> Search(string name)
        {
            var list = await _service.SearchByNameAsync(name);
            return Ok(list.Select(p => p.ToDto()));
        }

        [HttpGet("allergen/{allergenId}")]
        public async Task<IActionResult> GetByAllergen(int allergenId)
        {
            var list = await _service.GetByAllergenAsync(allergenId);
            return Ok(list.Select(p => p.ToDto()));
        }
    }
}
