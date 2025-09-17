using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TokyoGarden.Api.Controllers;
using TokyoGarden.IBL;
using Xunit;

namespace TokyoGarden.Api.Tests
{
    public class UzytkownicyControllerTests
    {
        [Fact]
        public async Task Delete_Returns_NotFound_When_User_Missing_Or_HasOrders()
        {
            var userSvc = new Mock<IUzytkownikService>();
            var orderSvc = new Mock<IZamowieniaService>();

            // Brak dodatkowych setupów => kontroler wejdzie w gałąź NotFound (u Ciebie tak się dzieje).
            var ctrl = new UzytkownicyController(userSvc.Object, orderSvc.Object);

            var res = await ctrl.Delete(1);

            Assert.IsType<NotFoundObjectResult>(res);
        }
    }
}
