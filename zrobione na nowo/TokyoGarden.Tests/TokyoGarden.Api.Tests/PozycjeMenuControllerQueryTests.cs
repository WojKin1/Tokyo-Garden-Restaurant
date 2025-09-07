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


using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TokyoGarden.Api.Controllers;
using TokyoGarden.IBL;
using TokyoGarden.Model;
using Xunit;

// -------------------------------------------------------------------------------------
//  Klasa testowa: opis celu i zakresu
//  - Weryfikuje kontrakty metod publicznych poprzez scenariusze o wysokiej wartości
//  - Minimalizuje powielanie setupu; w razie potrzeby używa helperów/fabryk
// -------------------------------------------------------------------------------------

namespace TokyoGarden.Api.Tests
{
    public class PozycjeMenuControllerQueryTests
    {
        [Fact]
        public async Task SearchByName_Returns_Results()
        {
            var mock = new Mock<IPozycjeMenuService>();
            mock.Setup(s => s.SearchByNameAsync("ra"))
                .ReturnsAsync(new List<Pozycje_Menu> { new Pozycje_Menu{ id=1, nazwa="Ramen"} });

            var ctrl = new PozycjeMenuController(mock.Object);
            var action = await ctrl.GetByName("ra");

            var ok = Assert.IsType<OkObjectResult>(action);
            var list = ((IEnumerable<object>)ok.Value!).ToList();
            Assert.Single(list);
        }

        [Fact]
        public async Task GetByAllergen_Returns_Results()
        {
            var mock = new Mock<IPozycjeMenuService>();
            mock.Setup(s => s.GetByAllergenAsync(1))
                .ReturnsAsync(new List<Pozycje_Menu> { new Pozycje_Menu{ id=1, nazwa="Sushi"} });

            var ctrl = new PozycjeMenuController(mock.Object);
            var action = await ctrl.GetByAllergen(1);

            var ok = Assert.IsType<OkObjectResult>(action);
            Assert.NotNull(ok.Value);
        }
    }
}
