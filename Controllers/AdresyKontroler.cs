using Microsoft.AspNetCore.Mvc;
using IBL;
using Model;

namespace YourNamespace.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdresyController : ControllerBase
    {
        private readonly IObslugaAdresow _obslugaAdresow;

        public AdresyController(IObslugaAdresow obslugaAdresow)
        {
            _obslugaAdresow = obslugaAdresow;
        }

        [HttpGet("posortowane")]
        public ActionResult<IEnumerable<Adresy>> GetSortedAdresy()
        {
            var adresy = _obslugaAdresow.PobierzPosortowaneAdresy();
            return Ok(adresy);
        }

        [HttpGet("liczba")]
        public ActionResult<int> GetAdresyCount()
        {
            var count = _obslugaAdresow.PoliczAdresy();
            return Ok(count);
        }
    }
}
