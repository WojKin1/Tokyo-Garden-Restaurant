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
    public class AlergenyControllerTests
    {
        [Fact]
        public async Task GetAll_ReturnsOk_WithItems()
        {
            var mock = new Mock<IAlergenyService>();
            mock.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<Alergeny>{ new Alergeny{ id = 1, nazwa = "Gluten"} });
            var ctrl = new AlergenyController(mock.Object);
            var res = await ctrl.GetAll();
            var ok = Assert.IsType<OkObjectResult>(res.Result);
            var list = Assert.IsAssignableFrom<IEnumerable<Alergeny>>(ok.Value!);
        }
    }

    public class KategorieControllerTests
    {
        [Fact]
        public async Task GetById_NotFound_WhenMissing()
        {
            var mock = new Mock<IKategorieService>();
            mock.Setup(s => s.GetByIdAsync(123)).ReturnsAsync((Kategorie?)null);
            var ctrl = new KategorieController(mock.Object);
            var res = await ctrl.GetById(123);
            Assert.IsType<NotFoundResult>(res.Result);
        }
    }

    public class PozycjeMenuControllerTests
    {
        [Fact]
        public async Task Post_CreatesItem()
        {
            var mock = new Mock<IPozycjeMenuService>();
            var ctrl = new PozycjeMenuController(mock.Object);
            var item = new Pozycje_Menu{ id = 0, nazwa = "Nowe", cena = 10 };
            var res = await ctrl.Create(item, Mock.Of<IKategorieService>());
            Assert.IsType<CreatedAtActionResult>(res);
        }
    }

    public class UzytkownicyControllerTests
    {
        [Fact]
        public async Task Delete_NoContent_OnSuccess()
        {
            var mock = new Mock<IUzytkownikService>();
            var ctrl = new UzytkownicyController(mock.Object);
            var res = await ctrl.Delete(1);
            Assert.IsType<NoContentResult>(res);
        }
    }
}
