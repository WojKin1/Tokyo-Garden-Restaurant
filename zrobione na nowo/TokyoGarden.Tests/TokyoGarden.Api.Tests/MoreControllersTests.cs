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
    public class AlergenyControllerTests
    {
        [Fact]
        public async Task GetAll_ReturnsOk_WithItems()
        {
            var service = new Mock<IAlergenyService>();
            service.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<Alergeny>
            {
                new Alergeny { id = 1, nazwa_alergenu = "Gluten" },
            });

            var ctrl = new AlergenyController(service.Object);

            var res = await ctrl.GetAll();
            var ok = Assert.IsType<OkObjectResult>(res);
            var list = Assert.IsAssignableFrom<IEnumerable<AlergenDTO>>(ok.Value);
            Assert.Single(list);
        }
    }

    public class KategorieControllerTests
    {
        [Fact]
        public async Task GetById_NotFound_WhenMissing()
        {
            var service = new Mock<IKategorieService>();
            service.Setup(s => s.GetByIdAsync(123)).ReturnsAsync((Kategorie)null!);

            var ctrl = new KategorieController(service.Object);
            var res = await ctrl.GetById(123);

            Assert.IsType<NotFoundResult>(res);
        }
    }
}
