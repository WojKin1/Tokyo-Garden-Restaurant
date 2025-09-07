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
    public class ZamowieniaControllerTests
    {
        [Fact]
        public async Task GetById_Zwraca_Ok_Gdy_Znaleziono()
        {
            var svc = new Mock<IZamowieniaService>();
            svc.Setup(x => x.GetByIdAsync(1))
               .ReturnsAsync(new Zamowienia { id = 1 });

            var ctrl = new ZamowieniaController(svc.Object);
            var res = await ctrl.GetById(1);

            var ok = Assert.IsType<OkObjectResult>(res.Result);
            var dto = Assert.IsType<Zamowienia>(ok.Value);
            Assert.Equal(1, dto.id);
        }

        [Fact]
        public async Task GetById_Zwraca_NotFound_Gdy_Brak()
        {
            var svc = new Mock<IZamowieniaService>();
            svc.Setup(x => x.GetByIdAsync(999)).ReturnsAsync((Zamowienia?)null);

            var ctrl = new ZamowieniaController(svc.Object);
            var res = await ctrl.GetById(999);

            Assert.IsType<NotFoundResult>(res.Result);
        }

        [Fact]
        public async Task GetAll_Zwraca_Liste()
        {
            var svc = new Mock<IZamowieniaService>();
            svc.Setup(x => x.GetAllAsync())
               .ReturnsAsync(new List<Zamowienia> { new Zamowienia { id = 1 } });

            var ctrl = new ZamowieniaController(svc.Object);
            var res = await ctrl.GetAll();

            var ok = Assert.IsType<OkObjectResult>(res.Result);
            Assert.IsType<List<Zamowienia>>(ok.Value);
        }
    }
}
