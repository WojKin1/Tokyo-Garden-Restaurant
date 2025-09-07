/*
 * =====================================================================================
 *  TokyoGarden – Zestaw testów jednostkowych
 *  Ten plik należy do warstwy TESTÓW. Zawiera przykłady dobrych praktyk:
 *    - Wzorzec AAA (Arrange / Act / Assert)
 *    - Izolacja testów (brak współdzielenia stanu, nowa InMemory DB per test)
 *    - Test Doubles: Dummy / Stub / Fake / Mock / Spy
 *    - Czytelne nazwy metod testowych odzwierciedlające scenariusz i oczekiwany wynik
 *    - Minimalny coupling: testujemy kontrakty interfejsów zamiast implementacji
 *    - Brak efektów ubocznych poza zakresem testu
 *  
 *  Jak czytać testy:
 *    1) Sekcja Arrange przygotowuje dane, zależności i stubs/mocks.
 *    2) Sekcja Act wykonuje akcję – zwykle jedną metodę na SUT (System Under Test).
 *    3) Sekcja Assert weryfikuje wynik: wartości, wywołania, wyjątki, statusy itp.
 * 
 *  Wskazówki rozszerzeń:
 *    - Dodaj testy scenariuszy negatywnych (walidacja 400, brak 404, konflikt 409).
 *    - Dodaj testy paginacji i sortowania (jeśli endpointy to wspierają).
 *    - Rozważ testy integracyjne z WebApplicationFactory dla pełnego stacku HTTP.
 *    - Mierz pokrycie: coverlet.collector -> raporty Cobertura/LCOV w CI.
 * =====================================================================================
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TokyoGarden.DAL;
using TokyoGarden.Model;
using Xunit;

// -------------------------------------------------------------------------------------
//  Klasa testowa: opis celu i zakresu
//  - Weryfikuje kontrakty metod publicznych poprzez scenariusze o wysokiej wartości
//  - Minimalizuje powielanie setupu; w razie potrzeby używa helperów/fabryk
// -------------------------------------------------------------------------------------

namespace TokyoGarden.DAL.Tests
{
    public class PozycjeMenuRepositoryQueryTests
    {
        private static DbTokyoGarden NewDb()
        {
            var options = new DbContextOptionsBuilder<DbTokyoGarden>()
                .UseInMemoryDatabase($"TokyoGarden-PM-{Guid.NewGuid()}")
                .Options;
            return new DbTokyoGarden(options);
        }

        [Fact]
        public async Task SearchByName_Finds_Items()
        {
            await using var db = NewDb();
            var repo = new PozycjeMenuRepository(db);

            await repo.AddAsync(new Pozycje_Menu { nazwa = "Ramen", cena = 30 });
            await repo.AddAsync(new Pozycje_Menu { nazwa = "Udon", cena = 25 });
            await db.SaveChangesAsync();

            var res = await repo.SearchByNameAsync("ra");
            Assert.Contains(res, x => x.nazwa == "Ramen");
        }

        [Fact]
        public async Task GetByAllergen_Returns_Items()
        {
            await using var db = NewDb();
            var repo = new PozycjeMenuRepository(db);

            var al1 = new Alergeny{ id = 1, nazwa = "Gluten"};
            var al2 = new Alergeny{ id = 2, nazwa = "Soja"};

            var e1 = new Pozycje_Menu { nazwa = "Soba", alergeny = new List<Alergeny>{ al1 } };
            var e2 = new Pozycje_Menu { nazwa = "Tofu", alergeny = new List<Alergeny>{ al2 } };

            await repo.AddAsync(e1); await repo.AddAsync(e2);
            await db.SaveChangesAsync();

            var res = await repo.GetByAllergenAsync(2);
            Assert.Single(res);
            Assert.Equal("Tofu", res.First().nazwa);
        }
    }
}
