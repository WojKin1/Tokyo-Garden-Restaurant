using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using TokyoGarden.BL;
using TokyoGarden.IDAL;
using TokyoGarden.Model;
using Xunit;

namespace TokyoGarden.BL.Tests
{
    public class MoreBLTests
    {
        [Fact]
        public async Task AlergenyService_Add_Calls_Repo_And_Save()
        {
            var repo = new Mock<IAlergenyRepository>();
            var svc  = new AlergenyService(repo.Object);

            await svc.AddAsync(new Alergeny { nazwa_alergenu = "Orzechy" });

            repo.Verify(r => r.AddAsync(It.IsAny<Alergeny>()), Times.Once);
        }
    }
}
