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

            var order = new Zamowienia
            {
                uzytkownik_id = 1,
                pozycje_zamowienia = new List<Pozycje_Zamowienia>
                {
                    new Pozycje_Zamowienia{ pozycja_menu_id = 1, ilosc = 2, cena = 15 },
                    new Pozycje_Zamowienia{ pozycja_menu_id = 2, ilosc = 1, cena = 30 }
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
