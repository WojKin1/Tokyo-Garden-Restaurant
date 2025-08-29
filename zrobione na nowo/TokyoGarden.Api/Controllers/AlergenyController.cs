using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using TokyoGarden.Api.Mapping;
using TokyoGarden.IBL;

namespace TokyoGarden.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlergenyController : ControllerBase
    {
        private readonly IAlergenyService _service;
        public AlergenyController(IAlergenyService service) { _service = service; }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _service.GetAllAsync();
            return Ok(list.Select(a => a.ToDto()));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _service.GetByIdAsync(id);
            return item == null ? NotFound() : Ok(item.ToDto());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Model.Alergeny item)
        {
            await _service.AddAsync(item);
            return CreatedAtAction(nameof(GetById), new { id = item.id }, item.ToDto());
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Model.Alergeny item)
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

        [HttpGet("by-name/{name}")]
        public async Task<IActionResult> GetByName(string name)
        {
            var item = await _service.GetByNameAsync(name);
            return item == null ? NotFound() : Ok(item.ToDto());
        }
    }
}
