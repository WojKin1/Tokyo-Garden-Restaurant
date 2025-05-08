using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Xunit;
using DAL;
using Model;

namespace TestyJednostkowe
{
    public class AdresyRepositoryTests
    {
        private DbTokyoGarden GetDbContext()
        {
            var options = new DbContextOptionsBuilder<DbTokyoGarden>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new DbTokyoGarden(options);
        }

        [Fact]
        public void InsertAdres_PowinienDodacAdresDoBazy()
        {
            // Arrange
            var context = GetDbContext();
            var repo = new AdresyRepository(context);
            var adres = new Adresy
            {
                id = 1,
                miasto = "Warszawa",
                nr_domu = 10,
                nr_mieszkania = 2,
                ulica = "Nowowiejska"
            };

            // Act
            repo.InsertAdres(adres);
            repo.Save();

            // Assert
            var zBazy = context.Adres.FirstOrDefault();
            Assert.NotNull(zBazy);
            Assert.Equal("Warszawa", zBazy.miasto);
        }

        [Fact]
        public void GetAdresy_PowinienZwrocicWszystkie()
        {
            // Arrange
            var context = GetDbContext();
            var repo = new AdresyRepository(context);

            repo.InsertAdres(new Adresy { id = 1, miasto = "Gdañsk", nr_domu = 12, nr_mieszkania = 3, ulica = "D³uga" });
            repo.InsertAdres(new Adresy { id = 2, miasto = "Poznañ", nr_domu = 15, nr_mieszkania = 0, ulica = "Pó³wiejska" });
            repo.Save();

            // Act
            var lista = repo.GetAdresy().ToList();

            // Assert
            Assert.Equal(2, lista.Count);
        }

        [Fact]
        public void GetAdresyByID_PowinienZwrocicPoprawnyAdres()
        {
            // Arrange
            var context = GetDbContext();
            var repo = new AdresyRepository(context);
            var adres = new Adresy { id = 5, miasto = "Kraków", nr_domu = 22, nr_mieszkania = 1, ulica = "Floriañska" };

            repo.InsertAdres(adres);
            repo.Save();

            // Act
            var wynik = repo.GetAdresyByID(5);

            // Assert
            Assert.NotNull(wynik);
            Assert.Equal("Kraków", wynik.miasto);
        }

        [Fact]
        public void DeleteAdres_PowinienUsunacAdresZBazy()
        {
            // Arrange
            var context = GetDbContext();
            var repo = new AdresyRepository(context);

            repo.InsertAdres(new Adresy { id = 7, miasto = "£ódŸ", nr_domu = 5, nr_mieszkania = 0, ulica = "Piotrkowska" });
            repo.Save();

            // Act
            repo.DeleteAdres(7);
            repo.Save();

            // Assert
            var wynik = repo.GetAdresyByID(7);
            Assert.Null(wynik);
        }
    }
}
