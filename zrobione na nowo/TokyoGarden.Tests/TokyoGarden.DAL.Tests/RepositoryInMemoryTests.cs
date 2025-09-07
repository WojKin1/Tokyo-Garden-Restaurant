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
    public class RepositoryInMemoryTests
    {
        private static DbTokyoGarden NewDb()
        {
            var options = new DbContextOptionsBuilder<DbTokyoGarden>()
                .UseInMemoryDatabase($"TokyoGarden-{Guid.NewGuid()}")
                .Options;
            return new DbTokyoGarden(options);
        }

        [Fact]
        public async Task AddAsync_Dodaje_PozycjeMenu()
        {
            await using var db = NewDb();
            var repo = new PozycjeMenuRepository(db);

            await repo.AddAsync(new Pozycje_Menu { nazwa = "Udon", cena = 35m });
            await db.SaveChangesAsync();

            var all = await repo.GetAllAsync();
            Assert.Contains(all, x => x.nazwa == "Udon");
        }

        [Fact]
        public async Task UpdateAsync_Aktualizuje_PozycjeMenu()
        {
            await using var db = NewDb();
            var repo = new PozycjeMenuRepository(db);

            var e = new Pozycje_Menu { nazwa = "Temp", cena = 10m };
            await repo.AddAsync(e);
            await db.SaveChangesAsync();

            e.cena = 12m;
            await repo.UpdateAsync(e);
            await db.SaveChangesAsync();

            var got = await repo.GetByIdAsync(e.id);
            Assert.Equal(12m, got?.cena);
        }

        [Fact]
        public async Task DeleteAsync_Usuwa_PozycjeMenu()
        {
            await using var db = NewDb();
            var repo = new PozycjeMenuRepository(db);

            var e = new Pozycje_Menu { nazwa = "Del", cena = 10m };
            await repo.AddAsync(e);
            await db.SaveChangesAsync();

            await repo.DeleteAsync(e.id);
            await db.SaveChangesAsync();

            var all = await repo.GetAllAsync();
            Assert.DoesNotContain(all, x => x.id == e.id);
        }
    }
}
