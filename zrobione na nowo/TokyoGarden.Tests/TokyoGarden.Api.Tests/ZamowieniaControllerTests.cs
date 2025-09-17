using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TokyoGarden.Api.Controllers;
using TokyoGarden.Api.DTOs;
using TokyoGarden.IBL;
using TokyoGarden.Model;
using Xunit;

namespace TokyoGarden.Api.Tests
{
    public class ZamowieniaControllerTests
    {
        [Fact]
        public async Task GetAll_Zwraca_Liste()
        {
            var orders = new List<Zamowienia> { new Zamowienia { id = 1 } };

            var ordersSvc = new Mock<IZamowieniaService>();
            // Kontroler może wołać GetAllWithDetailsAsync – ustaw oba
            ordersSvc.Setup(x => x.GetAllAsync()).ReturnsAsync(orders);
            ordersSvc.Setup(x => x.GetAllWithDetailsAsync()).ReturnsAsync(orders);

            var usersSvc = new Mock<IUzytkownikService>();

            var ctrl = new ZamowieniaController(ordersSvc.Object, usersSvc.Object);
            var res = await ctrl.GetAll();

            var ok = Assert.IsType<OkObjectResult>(res);
            var list = Assert.IsAssignableFrom<IEnumerable<ZamowienieDTO>>(ok.Value);
            Assert.Single(list);
        }
    }
}
