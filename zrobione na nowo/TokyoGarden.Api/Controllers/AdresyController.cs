using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using TokyoGarden.Api.Mapping;
using TokyoGarden.IBL;

namespace TokyoGarden.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdresyController : ControllerBase
    {
        private readonly IAdresyService _service;

        public AdresyController(IAdresyService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _service.GetAllAsync();
            return Ok(list.Select(a => a.ToDto()));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var address = await _service.GetByIdAsync(id);
            if (address == null) return NotFound();
            return Ok(address.ToDto());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Model.Adresy address)
        {
            await _service.AddAsync(address);
            return CreatedAtAction(nameof(GetById), new { id = address.id }, address.ToDto());
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Model.Adresy address)
        {
            if (id != address.id) return BadRequest();
            await _service.UpdateAsync(address);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }

        [HttpGet("city/{city}")]
        public async Task<IActionResult> GetByCity(string city)
        {
            var list = await _service.GetByCityAsync(city);
            return Ok(list.Select(a => a.ToDto()));
        }
    }
}
