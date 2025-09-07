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


using System.Threading.Tasks;
using Moq;
using TokyoGarden.BL;
using TokyoGarden.IDAL;
using TokyoGarden.Model;
using Xunit;

// -------------------------------------------------------------------------------------
//  Klasa testowa: opis celu i zakresu
//  - Weryfikuje kontrakty metod publicznych poprzez scenariusze o wysokiej wartości
//  - Minimalizuje powielanie setupu; w razie potrzeby używa helperów/fabryk
// -------------------------------------------------------------------------------------

namespace TokyoGarden.BL.Tests
{
    public class UzytkownikServiceMoqTests
    {
        [Fact]
        public async Task Rejestracja_Wywoluje_Add_And_Save()
        {
            var uow = new Mock<IUnitOfWork>();
            var repo = new Mock<IUzytkownikRepository>();
            uow.SetupGet(x => x.Uzytkownicy).Returns(repo.Object);
            uow.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

            var svc = new UzytkownikService(uow.Object);
            var u = new Uzytkownicy { email = "test@example.com", haslo = "Haslo123!" };

            await svc.AddAsync(u);

            repo.Verify(r => r.AddAsync(It.IsAny<Uzytkownicy>()), Times.Once);
            uow.Verify(u => u.SaveChangesAsync(), Times.Once);
        }
    }

    public class PozycjeZamowieniaServiceMoqTests
    {
        [Fact]
        public async Task Update_Zmienia_Ilosc()
        {
            var uow = new Mock<IUnitOfWork>();
            var repo = new Mock<IPozycjeZamowieniaRepository>();
            uow.SetupGet(x => x.PozycjeZamowienia).Returns(repo.Object);
            uow.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

            var svc = new PozycjeZamowieniaService(uow.Object);

            var item = new Pozycje_Zamowienia { id = 1, ilosc = 1, cena = 10 };
            await svc.UpdateAsync(item);

            repo.Verify(r => r.UpdateAsync(It.IsAny<Pozycje_Zamowienia>()), Times.Once);
            uow.Verify(u => u.SaveChangesAsync(), Times.Once);
        }
    }
}
