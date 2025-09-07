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
    public class OtherRepositoriesInMemoryTests
    {
        private static DbTokyoGarden NewDb()
        {
            var options = new DbContextOptionsBuilder<DbTokyoGarden>()
                .UseInMemoryDatabase($"TokyoGarden-Other-{Guid.NewGuid()}")
                .Options;
            return new DbTokyoGarden(options);
        }

        [Fact]
        public async Task Alergeny_Add_Then_Get()
        {
            await using var db = NewDb();
            var repo = new AlergenyRepository(db);
            await repo.AddAsync(new Alergeny { nazwa = "Soja" });
            await db.SaveChangesAsync();

            var list = await repo.GetAllAsync();
            Assert.Contains(list, a => a.nazwa == "Soja");
        }

        [Fact]
        public async Task Adresy_Add_Update_Delete()
        {
            await using var db = NewDb();
            var repo = new AdresyRepository(db);
            var a = new Adresy { ulica = "Test", miasto = "Poznań", kod_pocztowy = "60-000" };
            await repo.AddAsync(a); await db.SaveChangesAsync();

            a.miasto = "Warszawa"; await repo.UpdateAsync(a); await db.SaveChangesAsync();
            var got = await repo.GetByIdAsync(a.id);
            Assert.Equal("Warszawa", got?.miasto);

            await repo.DeleteAsync(a.id); await db.SaveChangesAsync();
            var again = await repo.GetByIdAsync(a.id);
            Assert.Null(again);
        }
    }
}
