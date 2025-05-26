using Microsoft.AspNetCore.Mvc;
using IBL;
using Model;
using System.Collections.Generic;

namespace YourNamespace.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PozycjaMenuController : ControllerBase
    {
        private readonly IObslugaPozycjiMenu _obsluga;

        public PozycjaMenuController(IObslugaPozycjiMenu obsluga)
        {
            _obsluga = obsluga;
        }

        [HttpGet("posortowane")]
        public ActionResult<IEnumerable<PozycjaMenu>> GetSortedPozycje()
        {
            var pozycje = _obsluga.PobierzPosortowanePozycje();
            return Ok(pozycje);
        }

        [HttpGet("liczba")]
        public ActionResult<int> GetPozycjeCount()
        {
            var count = _obsluga.PoliczPozycje();
            return Ok(count);
        }
    }
}
