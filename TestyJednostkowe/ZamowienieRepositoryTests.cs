using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Xunit;
using DAL;
using Model;

namespace TestyJednostkowe
{
    public class ZamowienieRepositoryTests
    {
        private DbTokyoGarden GetDbContext()
        {
            var options = new DbContextOptionsBuilder<DbTokyoGarden>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new DbTokyoGarden(options);
        }

        [Fact]
        public void InsertZamowienie_PowinienDodacZamowienie()
        {
            var context = GetDbContext();
            var repo = new ZamowienieRepository(context);

            var zamowienie = new Zamowienie
            {
                id = 1,
                data_zamowienia = DateTime.Now,
                dodatkowe_informacje = "Dostarcz na 5. piêtro",
                status_zamowienia = "Nowe",
                laczna_cena = 80.00,
                metoda_platnosci = "Karta",
                opcje_zamowienia = "Na wynos",
                AdresId = 1,
                UzytkownikId = 1
            };

            repo.InsertZamowienie(zamowienie);
            repo.Save();

            var zBazy = context.Zamowienia.FirstOrDefault();
            Assert.NotNull(zBazy);
            Assert.Equal("Nowe", zBazy.status_zamowienia);
        }

        [Fact]
        public void GetZamowienia_PowinienZwrocicWszystkie()
        {
            var context = GetDbContext();
            var repo = new ZamowienieRepository(context);

            repo.InsertZamowienie(new Zamowienie
            {
                id = 1,
                data_zamowienia = DateTime.Today,
                dodatkowe_informacje = "Zamówienie testowe 1",
                status_zamowienia = "Nowe",
                laczna_cena = 50,
                metoda_platnosci = "Blik",
                opcje_zamowienia = "Na miejscu",
                AdresId = 1,
                UzytkownikId = 1
            });

            repo.InsertZamowienie(new Zamowienie
            {
                id = 2,
                data_zamowienia = DateTime.Today,
                dodatkowe_informacje = "Zamówienie testowe 2",
                status_zamowienia = "W realizacji",
                laczna_cena = 120,
                metoda_platnosci = "Gotówka",
                opcje_zamowienia = "Na wynos",
                AdresId = 2,
                UzytkownikId = 2
            });

            repo.Save();

            var lista = repo.GetZamowienia().ToList();
            Assert.Equal(2, lista.Count);
        }

        [Fact]
        public void GetZamowienieByID_PowinienZwrocicPoprawneZamowienie()
        {
            var context = GetDbContext();
            var repo = new ZamowienieRepository(context);

            var zamowienie = new Zamowienie
            {
                id = 3,
                data_zamowienia = DateTime.Today,
                dodatkowe_informacje = "Test ID",
                status_zamowienia = "Nowe",
                laczna_cena = 70,
                metoda_platnosci = "Blik",
                opcje_zamowienia = "Na miejscu",
                AdresId = 3,
                UzytkownikId = 3
            };

            repo.InsertZamowienie(zamowienie);
            repo.Save();

            var wynik = repo.GetZamowienieByID(3);
            Assert.NotNull(wynik);
            Assert.Equal("Test ID", wynik.dodatkowe_informacje);
        }

        [Fact]
        public void DeleteZamowienie_PowinienUsunacZamowienie()
        {
            var context = GetDbContext();
            var repo = new ZamowienieRepository(context);

            repo.InsertZamowienie(new Zamowienie
            {
                id = 4,
                data_zamowienia = DateTime.Today,
                dodatkowe_informacje = "Do usuniêcia",
                status_zamowienia = "Do usuniêcia",
                laczna_cena = 10,
                metoda_platnosci = "Przelew",
                opcje_zamowienia = "Online",
                AdresId = 4,
                UzytkownikId = 4
            });

            repo.Save();
            repo.DeleteZamowienie(4);
            repo.Save();

            var wynik = repo.GetZamowienieByID(4);
            Assert.Null(wynik);
        }
    }
}
