using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Xunit;
using DAL;
using Model;

namespace TestyJednostkowe
{
    public class KategoriaRepositoryTests
    {
        private DbTokyoGarden GetDbContext()
        {
            var options = new DbContextOptionsBuilder<DbTokyoGarden>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new DbTokyoGarden(options);
        }

        [Fact]
        public void InsertKategoria_PowinienDodacKategorie()
        {
            // Arrange
            var context = GetDbContext();
            var repo = new KategoriaRepository(context);
            var kategoria = new Kategoria
            {
                id = 1,
                nazwa_kategorii = "Zupy"
            };

            // Act
            repo.InsertKategoria(kategoria);
            repo.Save();

            // Assert
            var zBazy = context.Kategorie.FirstOrDefault();
            Assert.NotNull(zBazy);
            Assert.Equal("Zupy", zBazy.nazwa_kategorii);
        }

        [Fact]
        public void GetKategorie_PowinienZwrocicWszystkie()
        {
            // Arrange
            var context = GetDbContext();
            var repo = new KategoriaRepository(context);

            repo.InsertKategoria(new Kategoria { id = 1, nazwa_kategorii = "Przystawki" });
            repo.InsertKategoria(new Kategoria { id = 2, nazwa_kategorii = "Desery" });
            repo.Save();

            // Act
            var lista = repo.GetKategorie().ToList();

            // Assert
            Assert.Equal(2, lista.Count);
        }

        [Fact]
        public void GetKategoriaByID_PowinienZwrocicPoprawnaKategorie()
        {
            // Arrange
            var context = GetDbContext();
            var repo = new KategoriaRepository(context);

            var kategoria = new Kategoria { id = 3, nazwa_kategorii = "Napoje" };
            repo.InsertKategoria(kategoria);
            repo.Save();

            // Act
            var wynik = repo.GetKategoriaByID(3);

            // Assert
            Assert.NotNull(wynik);
            Assert.Equal("Napoje", wynik.nazwa_kategorii);
        }

        [Fact]
        public void DeleteKategoria_PowinienUsunacKategorie()
        {
            // Arrange
            var context = GetDbContext();
            var repo = new KategoriaRepository(context);

            repo.InsertKategoria(new Kategoria { id = 4, nazwa_kategorii = "Dania g³ówne" });
            repo.Save();

            // Act
            repo.DeleteKategoria(4);
            repo.Save();

            // Assert
            var wynik = repo.GetKategoriaByID(4);
            Assert.Null(wynik);
        }
    }
}
