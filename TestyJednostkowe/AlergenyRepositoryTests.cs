using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Xunit;
using DAL;
using Model;

namespace TestyJednostkowe
{
    public class AlergenyRepositoryTests
    {
        private DbTokyoGarden GetDbContext()
        {
            var options = new DbContextOptionsBuilder<DbTokyoGarden>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new DbTokyoGarden(options);
        }

        [Fact]
        public void InsertAlergen_PowinienDodacAlergen()
        {
            // Arrange
            var context = GetDbContext();
            var repo = new AlergenyRepository(context);
            var alergen = new Alergeny
            {
                id = 1,
                nazwa_alergenu = "Orzechy",
                opis_alergenu = "Reakcje skórne i oddechowe"
            };

            // Act
            repo.InsertAlergen(alergen);
            repo.Save();

            // Assert
            var zBazy = context.alergenies.FirstOrDefault();
            Assert.NotNull(zBazy);
            Assert.Equal("Orzechy", zBazy.nazwa_alergenu);
        }

        [Fact]
        public void GetAlergeny_PowinienZwrocicWszystkie()
        {
            // Arrange
            var context = GetDbContext();
            var repo = new AlergenyRepository(context);

            repo.InsertAlergen(new Alergeny { id = 1, nazwa_alergenu = "Gluten", opis_alergenu = "Reakcje pokarmowe" });
            repo.InsertAlergen(new Alergeny { id = 2, nazwa_alergenu = "Mleko", opis_alergenu = "Nietolerancja laktozy" });
            repo.Save();

            // Act
            var lista = repo.GetAlergeny().ToList();

            // Assert
            Assert.Equal(2, lista.Count);
        }

        [Fact]
        public void GetAlergenyByID_PowinienZwrocicPoprawnyAlergen()
        {
            // Arrange
            var context = GetDbContext();
            var repo = new AlergenyRepository(context);
            var alergen = new Alergeny { id = 5, nazwa_alergenu = "Jaja", opis_alergenu = "Mo¿e powodowaæ wysypkê" };
            repo.InsertAlergen(alergen);
            repo.Save();

            // Act
            var wynik = repo.GetAlergenyByID(5);

            // Assert
            Assert.NotNull(wynik);
            Assert.Equal("Jaja", wynik.nazwa_alergenu);
        }

        [Fact]
        public void DeleteAlergen_PowinienUsunacZBazy()
        {
            // Arrange
            var context = GetDbContext();
            var repo = new AlergenyRepository(context);
            repo.InsertAlergen(new Alergeny { id = 7, nazwa_alergenu = "Soja", opis_alergenu = "Wzdêcia, bóle brzucha" });
            repo.Save();

            // Act
            repo.DeleteAlergen(7);
            repo.Save();

            // Assert
            var wynik = repo.GetAlergenyByID(7);
            Assert.Null(wynik);
        }
    }
}
