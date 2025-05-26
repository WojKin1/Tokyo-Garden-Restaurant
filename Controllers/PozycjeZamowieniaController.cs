using Microsoft.AspNetCore.Mvc;
using IBL;
using Model;
using System.Collections.Generic;

namespace YourNamespace.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PozycjeZamowieniaController : ControllerBase
    {
        private readonly IObslugaPozycjiZamowienia _obsluga;

        public PozycjeZamowieniaController(IObslugaPozycjiZamowienia obsluga)
        {
            _obsluga = obsluga;
        }

        [HttpGet]
        public ActionResult<IEnumerable<PozycjaZamowienia>> GetPozycjeZamowienia()
        {
            var pozycje = _obsluga.PobierzPozycjeZamowienia();
            return Ok(pozycje);
        }

        [HttpGet("liczba")]
        public ActionResult<int> GetPozycjeZamowieniaCount()
        {
            var count = _obsluga.PoliczPozycjeZamowienia();
            return Ok(count);
        }
    }
}
