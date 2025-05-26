using Microsoft.AspNetCore.Mvc;
using IBL;
using Model;
using System.Collections.Generic;

namespace YourNamespace.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UzytkownikController : ControllerBase
    {
        private readonly IObslugaUzytkownik _obsluga;

        public UzytkownikController(IObslugaUzytkownik obsluga)
        {
            _obsluga = obsluga;
        }

        [HttpGet("posortowane")]
        public ActionResult<IEnumerable<Uzytkownik>> GetSortedUzytkownicy()
        {
            var uzytkownicy = _obsluga.PobierzPosortowaneUzytkownikow();
            return Ok(uzytkownicy);
        }

        [HttpGet("liczba")]
        public ActionResult<int> GetUzytkownicyCount()
        {
            var count = _obsluga.PoliczUzytkownikow();
            return Ok(count);
        }
    }
}
