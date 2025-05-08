using Xunit;
using Moq;
using BL;
using IBL;
using IDAL;
using Model;
using System.Collections.Generic;

namespace TestProject.Moq
{
    public class MoqDummyTest
    {
        [Fact]
        public void DummyTest_ObslugaAlergenPozycjaMenu_PoliczPowiazania()
        {
            var powiazania = new List<AlergenPozycjaMenu>
            {
                new AlergenPozycjaMenu { id_alergen = 1, id_pozycja_menu = 1 },
                new AlergenPozycjaMenu { id_alergen = 1, id_pozycja_menu = 2 },
                new AlergenPozycjaMenu { id_alergen = 2, id_pozycja_menu = 2 }
            };

            var mockRepo = new Mock<IAlergenPozycjaMenuRepository>();
            mockRepo.Setup(repo => repo.GetAlergenPozycjaMenu()).Returns(powiazania);

            var dummyRepo = new Mock<IAdresyRepository>().Object;

            var obslugaPowiazan = new ObslugaAlergenPozycjaMenu(mockRepo.Object);

            var result = obslugaPowiazan.PoliczPowiazania();

            Assert.Equal(3, result);

            mockRepo.Verify(repo => repo.GetAlergenPozycjaMenu(), Times.Once);
        }
    }
}