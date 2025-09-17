using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TokyoGarden.DAL;
using TokyoGarden.Model;
using Xunit;

namespace TokyoGarden.DAL.Tests
{
    public class ZamowieniaRepositoryInMemoryTests
    {
        private static DbTokyoGarden NewDb()
        {
            var options = new DbContextOptionsBuilder<DbTokyoGarden>()
                .UseInMemoryDatabase($"TokyoGarden-Zam-{Guid.NewGuid()}")
                .Options;
            return new DbTokyoGarden(options);
        }

        [Fact]
        public async Task GetAllWithDetails_Returns_Positions()
        {
            await using var db = NewDb();
            var repo = new ZamowieniaRepository(db);

            // Przygotuj zależne encje (nawigacje, nie FK *_id)
            var user = new Uzytkownicy { nazwa_uzytkownika = "tester" };
            var m1 = new Pozycje_Menu { nazwa_pozycji = "M1", cena = 15 };
            var m2 = new Pozycje_Menu { nazwa_pozycji = "M2", cena = 30 };
            db.AddRange(user, m1, m2);
            await db.SaveChangesAsync();

            var order = new Zamowienia
            {
                uzytkownik = user,
                pozycje_zamowienia = new List<Pozycje_Zamowienia>
                {
                    new Pozycje_Zamowienia{ pozycja_menu = m1, ilosc = 2, cena = 15 },
                    new Pozycje_Zamowienia{ pozycja_menu = m2, ilosc = 1, cena = 30 }
                }
            };

            await repo.AddAsync(order);
            await db.SaveChangesAsync();

            var list = await repo.GetAllWithDetailsAsync();
            Assert.Single(list);
            var first = list.First();
            Assert.NotNull(first.pozycje_zamowienia);
            Assert.Equal(2, first.pozycje_zamowienia!.Count);
        }
    }
}
