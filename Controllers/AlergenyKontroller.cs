using Microsoft.AspNetCore.Mvc;
using IBL;
using Model;

namespace YourNamespace.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AlergenyController : ControllerBase
    {
        private readonly IObslugaAlergenow _obslugaAlergenow;

        public AlergenyController(IObslugaAlergenow obslugaAlergenow)
        {
            _obslugaAlergenow = obslugaAlergenow;
        }

        [HttpGet("posortowane")]
        public ActionResult<IEnumerable<Alergeny>> GetSortedAlergeny()
        {
            var alergeny = _obslugaAlergenow.PobierzPosortowaneAlergeny();
            return Ok(alergeny);
        }

        [HttpGet("liczba")]
        public ActionResult<int> GetAlergenyCount()
        {
            var count = _obslugaAlergenow.PoliczAlergeny();
            return Ok(count);
        }
    }
}
