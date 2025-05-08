using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Xunit;
using DAL;
using Model;

namespace TestyJednostkowe
{
    public class PozycjaMenuRepositoryTests
    {
        private DbTokyoGarden GetDbContext()
        {
            var options = new DbContextOptionsBuilder<DbTokyoGarden>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new DbTokyoGarden(options);
        }

        [Fact]
        public void InsertPozycjaMenu_PowinienDodacPozycje()
        {
            // Arrange
            var context = GetDbContext();
            var repo = new PozycjaMenuRepository(context);
            var pozycja = new PozycjaMenu
            {
                id = 1,
                cena = 25.50,
                nazwa_pozycji = "Sushi Mix",
                opis = "Zestaw 8 rolek",
                skladniki = "Ry¿, ³osoœ, nori"
            };

            // Act
            repo.InsertPozycjaMenu(pozycja);
            repo.Save();

            // Assert
            var zBazy = context.PozycjeMenu.FirstOrDefault();
            Assert.NotNull(zBazy);
            Assert.Equal("Sushi Mix", zBazy.nazwa_pozycji);
            Assert.Equal(25.50, zBazy.cena);
        }

        [Fact]
        public void GetPozycjeMenu_PowinienZwrocicWszystkiePozycje()
        {
            // Arrange
            var context = GetDbContext();
            var repo = new PozycjaMenuRepository(context);

            repo.InsertPozycjaMenu(new PozycjaMenu { id = 1, cena = 15.00, nazwa_pozycji = "Zupa Miso", opis = "Japoñska zupa", skladniki = "Miso, tofu, wakame" });
            repo.InsertPozycjaMenu(new PozycjaMenu { id = 2, cena = 32.00, nazwa_pozycji = "Ramen", opis = "Ramen z jajkiem", skladniki = "Makaron, bulion, jajko" });
            repo.Save();

            // Act
            var lista = repo.GetPozycjeMenu().ToList();

            // Assert
            Assert.Equal(2, lista.Count);
        }

        [Fact]
        public void GetPozycjaMenuByID_PowinienZwrocicPoprawnaPozycje()
        {
            // Arrange
            var context = GetDbContext();
            var repo = new PozycjaMenuRepository(context);
            var pozycja = new PozycjaMenu { id = 3, cena = 18.00, nazwa_pozycji = "Gyoza", opis = "Pierogi z miêsem", skladniki = "Wieprzowina, cebula" };
            repo.InsertPozycjaMenu(pozycja);
            repo.Save();

            // Act
            var wynik = repo.GetPozycjaMenuByID(3);

            // Assert
            Assert.NotNull(wynik);
            Assert.Equal("Gyoza", wynik.nazwa_pozycji);
        }

        [Fact]
        public void DeletePozycjaMenu_PowinienUsunacPozycje()
        {
            // Arrange
            var context = GetDbContext();
            var repo = new PozycjaMenuRepository(context);
            repo.InsertPozycjaMenu(new PozycjaMenu { id = 4, cena = 10.00, nazwa_pozycji = "Edamame", opis = "Gotowana soja", skladniki = "Soja, sól" });
            repo.Save();

            // Act
            repo.DeletePozycjaMenu(4);
            repo.Save();

            // Assert
            var wynik = repo.GetPozycjaMenuByID(4);
            Assert.Null(wynik);
        }
    }
}
