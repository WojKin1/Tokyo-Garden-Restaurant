using Microsoft.AspNetCore.Mvc;
using IBL;
using Model;
using System.Collections.Generic;

namespace YourNamespace.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AlergenPozycjaMenuController : ControllerBase
    {
        private readonly IObslugaAlergenPozycjaMenu _obsluga;

        public AlergenPozycjaMenuController(IObslugaAlergenPozycjaMenu obsluga)
        {
            _obsluga = obsluga;
        }

        [HttpGet("wszystkie")]
        public ActionResult<IEnumerable<AlergenPozycjaMenu>> GetAllPowiazania()
        {
            var powiazania = _obsluga.PobierzWszystkiePowiazania();
            return Ok(powiazania);
        }

        [HttpGet("liczba")]
        public ActionResult<int> GetPowiazaniaCount()
        {
            var count = _obsluga.PoliczPowiazania();
            return Ok(count);
        }
    }
}
