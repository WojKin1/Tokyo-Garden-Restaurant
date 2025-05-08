using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Xunit;
using DAL;
using Model;

namespace TestyJednostkowe
{
    public class PozycjaZamowieniaRepositoryTests
    {
        private DbTokyoGarden GetDbContext()
        {
            var options = new DbContextOptionsBuilder<DbTokyoGarden>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new DbTokyoGarden(options);
        }

        [Fact]
        public void InsertPozycjaZamowienia_PowinienDodacRekord()
        {
            // Arrange
            var context = GetDbContext();
            var repo = new PozycjaZamowieniaRepository(context);
            var pozycja = new PozycjaZamowienia
            {
                id = 1,
                cena = 45.00,
                ilosc = 2
            };

            // Act
            repo.InsertPozycjaZamowienia(pozycja);
            repo.Save();

            // Assert
            var zBazy = context.PozycjeZamowienia.FirstOrDefault();
            Assert.NotNull(zBazy);
            Assert.Equal(45.00, zBazy.cena);
            Assert.Equal(2, zBazy.ilosc);
        }

        [Fact]
        public void GetPozycjeZamowienia_PowinienZwrocicWszystkie()
        {
            // Arrange
            var context = GetDbContext();
            var repo = new PozycjaZamowieniaRepository(context);

            repo.InsertPozycjaZamowienia(new PozycjaZamowienia { id = 1, cena = 20.00, ilosc = 1 });
            repo.InsertPozycjaZamowienia(new PozycjaZamowienia { id = 2, cena = 30.00, ilosc = 3 });
            repo.Save();

            // Act
            var lista = repo.GetPozycjeZamowienia().ToList();

            // Assert
            Assert.Equal(2, lista.Count);
        }

        [Fact]
        public void GetPozycjaZamowieniaByID_PowinienZwrocicPoprawnaPozycje()
        {
            // Arrange
            var context = GetDbContext();
            var repo = new PozycjaZamowieniaRepository(context);
            var pozycja = new PozycjaZamowienia { id = 3, cena = 12.00, ilosc = 4 };
            repo.InsertPozycjaZamowienia(pozycja);
            repo.Save();

            // Act
            var wynik = repo.GetPozycjaZamowieniaByID(3);

            // Assert
            Assert.NotNull(wynik);
            Assert.Equal(12.00, wynik.cena);
            Assert.Equal(4, wynik.ilosc);
        }

        [Fact]
        public void DeletePozycjaZamowienia_PowinienUsunacPozycje()
        {
            // Arrange
            var context = GetDbContext();
            var repo = new PozycjaZamowieniaRepository(context);
            repo.InsertPozycjaZamowienia(new PozycjaZamowienia { id = 4, cena = 18.00, ilosc = 2 });
            repo.Save();

            // Act
            repo.DeletePozycjaZamowienia(4);
            repo.Save();

            // Assert
            var wynik = repo.GetPozycjaZamowieniaByID(4);
            Assert.Null(wynik);
        }
    }
}
