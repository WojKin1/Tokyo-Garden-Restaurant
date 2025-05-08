using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Xunit;
using DAL;
using Model;

namespace TestyJednostkowe
{
    public class UzytkownikRepositoryTests
    {
        private DbTokyoGarden GetDbContext()
        {
            var options = new DbContextOptionsBuilder<DbTokyoGarden>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new DbTokyoGarden(options);
        }

        [Fact]
        public void InsertUzytkownik_PowinienDodacUzytkownika()
        {
            // Arrange
            var context = GetDbContext();
            var repo = new UzytkownicyRepository(context);
            var user = new Uzytkownik
            {
                id = 1,
                nazwa_uzytkownika = "wojtek",
                haslo = "tajne123",
                telefon = "123456789",
                typ_uzytkownika = 1
            };

            // Act
            repo.InsertUzytkownik(user);
            repo.Save();

            // Assert
            var zBazy = context.uzytkownik.FirstOrDefault();
            Assert.NotNull(zBazy);
            Assert.Equal("wojtek", zBazy.nazwa_uzytkownika);
        }

        [Fact]
        public void GetUzytkownik_PowinienZwrocicWszystkich()
        {
            // Arrange
            var context = GetDbContext();
            var repo = new UzytkownicyRepository(context);

            repo.InsertUzytkownik(new Uzytkownik { id = 1, nazwa_uzytkownika = "anna", haslo = "haslo1", telefon = "987654321", typ_uzytkownika = 2 });
            repo.InsertUzytkownik(new Uzytkownik { id = 2, nazwa_uzytkownika = "jan", haslo = "haslo2", telefon = "111222333", typ_uzytkownika = 1 });
            repo.Save();

            // Act
            var lista = repo.GetUzytkownik().ToList();

            // Assert
            Assert.Equal(2, lista.Count);
        }

        [Fact]
        public void GetUzytkownikByID_PowinienZwrocicPoprawnegoUzytkownika()
        {
            // Arrange
            var context = GetDbContext();
            var repo = new UzytkownicyRepository(context);

            var user = new Uzytkownik { id = 3, nazwa_uzytkownika = "kasia", haslo = "abc123", telefon = "444555666", typ_uzytkownika = 2 };
            repo.InsertUzytkownik(user);
            repo.Save();

            // Act
            var wynik = repo.GetUzytkownikByID(3);

            // Assert
            Assert.NotNull(wynik);
            Assert.Equal("kasia", wynik.nazwa_uzytkownika);
        }

        [Fact]
        public void DeleteUzytkownik_PowinienUsunacUzytkownika()
        {
            // Arrange
            var context = GetDbContext();
            var repo = new UzytkownicyRepository(context);

            repo.InsertUzytkownik(new Uzytkownik { id = 4, nazwa_uzytkownika = "bartek", haslo = "xyz789", telefon = "999888777", typ_uzytkownika = 1 });
            repo.Save();

            // Act
            repo.DeleteUzytkownik(4);
            repo.Save();

            // Assert
            var wynik = repo.GetUzytkownikByID(4);
            Assert.Null(wynik);
        }
    }
}
