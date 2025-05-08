using Xunit;
using Moq;
using BL;
using IBL;
using IDAL;
using Model;
using System.Collections.Generic;
using System.Linq;

namespace TestProject.Moq
{
    public class MoqStubTest
    {
        [Fact]
        public void StubTest_ObslugaKategorii_PobierzKategorie()
        {
            var kategorie = new List<Kategoria>
            {
                new Kategoria { id = 1, nazwa_kategorii = "Zupy" },
                new Kategoria { id = 2, nazwa_kategorii = "Dania g³ówne" },
                new Kategoria { id = 3, nazwa_kategorii = "Przystawki" }
            };

            var stubRepo = new Mock<IKategoriaRepository>();
            stubRepo.Setup(repo => repo.GetKategorie()).Returns(kategorie);

            var obslugaKategorii = new ObslugaKategorii(stubRepo.Object);

            var result = obslugaKategorii.PobierzKategorie().ToList();

            Assert.Equal(3, result.Count);
            Assert.Equal("Dania g³ówne", result[0].nazwa_kategorii);
            Assert.Equal("Przystawki", result[1].nazwa_kategorii);
            Assert.Equal("Zupy", result[2].nazwa_kategorii);
        }
    }
}