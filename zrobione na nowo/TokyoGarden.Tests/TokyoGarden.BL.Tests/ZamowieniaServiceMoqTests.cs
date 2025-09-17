using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using TokyoGarden.BL;
using TokyoGarden.IDAL;
using TokyoGarden.Model;
using Xunit;

namespace TokyoGarden.BL.Tests
{
    public class ZamowieniaServiceMoqTests
    {
        [Fact]
        public async Task GetAllWithDetails_Returns_List()
        {
            var repo = new Mock<IZamowieniaRepository>();
            repo.Setup(r => r.GetAllWithDetailsAsync())
                .ReturnsAsync(new List<Zamowienia> { new Zamowienia{ id = 1 } });

            var posRepo = new Mock<IPozycjeZamowieniaRepository>();

            var svc = new ZamowieniaService(repo.Object, posRepo.Object);
            var res = await svc.GetAllWithDetailsAsync();

            Assert.Single(res);
        }
    }
}
