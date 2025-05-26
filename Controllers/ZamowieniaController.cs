using Microsoft.AspNetCore.Mvc;
using IBL;
using Model;
using System.Collections.Generic;

namespace YourNamespace.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ZamowieniaController : ControllerBase
    {
        private readonly IObslugaZamowien _obsluga;

        public ZamowieniaController(IObslugaZamowien obsluga)
        {
            _obsluga = obsluga;
        }

        [HttpGet("posortowane")]
        public ActionResult<IEnumerable<Zamowienie>> GetSortedZamowienia()
        {
            var zamowienia = _obsluga.PobierzZamowieniaPosortowane();
            return Ok(zamowienia);
        }

        [HttpGet("liczba")]
        public ActionResult<int> GetZamowieniaCount()
        {
            var count = _obsluga.PoliczZamowienia();
            return Ok(count);
        }
    }
}
