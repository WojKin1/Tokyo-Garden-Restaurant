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
using Moq;
using TokyoGarden.IDAL;
using TokyoGarden.IBL;
using TokyoGarden.Model;
using TokyoGarden.BL;
using Xunit;

// -------------------------------------------------------------------------------------
//  Klasa testowa: opis celu i zakresu
//  - Weryfikuje kontrakty metod publicznych poprzez scenariusze o wysokiej wartości
//  - Minimalizuje powielanie setupu; w razie potrzeby używa helperów/fabryk
// -------------------------------------------------------------------------------------

namespace TokyoGarden.BL.Tests
{
    public class KategorieServiceMoqTests
    {
        [Fact]
        public async Task GetAll_Returns_List_From_Repo()
        {
            var uow = new Mock<IUnitOfWork>();
            var repo = new Mock<IKategorieRepository>();
            repo.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Kategorie>{ new Kategorie{ id = 1, nazwa = "Zupy"} });
            uow.SetupGet(x => x.Kategorie).Returns(repo.Object);

            var svc = new KategorieService(uow.Object);
            var res = await svc.GetAllAsync();

            Assert.Single(res);
        }
    }

    public class AlergenyServiceMoqTests
    {
        [Fact]
        public async Task Add_Calls_Repo_And_Save()
        {
            var uow = new Mock<IUnitOfWork>();
            var repo = new Mock<IAlergenyRepository>();
            uow.SetupGet(x => x.Alergeny).Returns(repo.Object);
            uow.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

            var svc = new AlergenyService(uow.Object);
            await svc.AddAsync(new Alergeny { nazwa = "Orzechy" });

            repo.Verify(r => r.AddAsync(It.IsAny<Alergeny>()), Times.Once);
            uow.Verify(u => u.SaveChangesAsync(), Times.Once);
        }
    }
}
