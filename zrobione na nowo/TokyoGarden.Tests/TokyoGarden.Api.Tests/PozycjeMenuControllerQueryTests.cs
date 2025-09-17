using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TokyoGarden.Api.Controllers;
using TokyoGarden.IBL;
using TokyoGarden.Model;
using Xunit;

namespace TokyoGarden.Api.Tests
{
    public class PozycjeMenuControllerQueryTests
    {
        [Fact]
        public async Task GetByAllergen_Returns_Results()
        {
            var mock = new Mock<IPozycjeMenuService>();
            mock.Setup(s => s.GetByAllergenAsync(1))
                .ReturnsAsync(new List<Pozycje_Menu> { new Pozycje_Menu{ id=1, nazwa_pozycji="Sushi"} });

            var ctrl = new PozycjeMenuController(mock.Object);
            var action = await ctrl.GetByAllergen(1);

            var ok = Assert.IsType<OkObjectResult>(action);
            Assert.NotNull(ok.Value);
        }
    }
}
