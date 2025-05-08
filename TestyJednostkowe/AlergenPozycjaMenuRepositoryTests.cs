using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Xunit;
using DAL;
using Model;

namespace TestyJednostkowe
{
    public class AlergenPozycjaMenuRepositoryTests
    {
        private DbTokyoGarden GetDbContext()
        {
            var options = new DbContextOptionsBuilder<DbTokyoGarden>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new DbTokyoGarden(options);
        }

        [Fact]
        public void InsertAlergenPozycjaMenu_PowinienDodacDoBazy()
        {
            // Arrange
            var context = GetDbContext();
            var repo = new AlergenPozycjaMenuRepository(context);
            var apm = new AlergenPozycjaMenu
            {
                id_pozycja_menu = 1,
                id_alergen = 2
            };

            // Act
            repo.InsertAlergenPozycjaMenu(apm);
            repo.Save();

            // Assert
            var zBazy = context.AlergenPozycjaMenu.FirstOrDefault();
            Assert.NotNull(zBazy);
            Assert.Equal(1, zBazy.id_pozycja_menu);
            Assert.Equal(2, zBazy.id_alergen);
        }

        [Fact]
        public void GetAlergenPozycjaMenu_PowinienZwrocicWszystkie()
        {
            // Arrange
            var context = GetDbContext();
            var repo = new AlergenPozycjaMenuRepository(context);

            repo.InsertAlergenPozycjaMenu(new AlergenPozycjaMenu { id_pozycja_menu = 1, id_alergen = 2 });
            repo.InsertAlergenPozycjaMenu(new AlergenPozycjaMenu { id_pozycja_menu = 3, id_alergen = 4 });
            repo.Save();

            // Act
            var lista = repo.GetAlergenPozycjaMenu().ToList();

            // Assert
            Assert.Equal(2, lista.Count);
        }

        [Fact]
        public void GetByIDs_PowinienZwrocicPoprawnyRekord()
        {
            // Arrange
            var context = GetDbContext();
            var repo = new AlergenPozycjaMenuRepository(context);

            var apm = new AlergenPozycjaMenu { id_pozycja_menu = 5, id_alergen = 6 };
            repo.InsertAlergenPozycjaMenu(apm);
            repo.Save();

            // Act
            var wynik = repo.GetByIDs(6, 5); // id_alergen, id_pozycja_menu

            // Assert
            Assert.NotNull(wynik);
            Assert.Equal(6, wynik.id_alergen);
            Assert.Equal(5, wynik.id_pozycja_menu);
        }

        [Fact]
        public void DeleteAlergenPozycjaMenu_PowinienUsunacRekord()
        {
            // Arrange
            var context = GetDbContext();
            var repo = new AlergenPozycjaMenuRepository(context);
            var apm = new AlergenPozycjaMenu { id_pozycja_menu = 7, id_alergen = 8 };
            repo.InsertAlergenPozycjaMenu(apm);
            repo.Save();

            // Act
            repo.DeleteAlergenPozycjaMenu(8, 7); // id_alergen, id_pozycja_menu
            repo.Save();

            // Assert
            var wynik = repo.GetByIDs(8, 7);
            Assert.Null(wynik);
        }
    }
}
