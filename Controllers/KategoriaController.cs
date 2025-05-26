using Microsoft.AspNetCore.Mvc;
using IBL;
using Model;
using System.Collections.Generic;

namespace YourNamespace.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class KategorieController : ControllerBase
    {
        private readonly IObslugaKategorii _obslugaKategorii;

        public KategorieController(IObslugaKategorii obslugaKategorii)
        {
            _obslugaKategorii = obslugaKategorii;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Kategoria>> GetKategorie()
        {
            var kategorie = _obslugaKategorii.PobierzKategorie();
            return Ok(kategorie);
        }

        [HttpGet("liczba")]
        public ActionResult<int> GetKategorieCount()
        {
            var count = _obslugaKategorii.PoliczKategorie();
            return Ok(count);
        }
    }
}
