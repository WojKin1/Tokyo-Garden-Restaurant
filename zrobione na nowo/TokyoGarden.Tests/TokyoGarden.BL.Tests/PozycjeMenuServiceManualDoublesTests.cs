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
using System.Threading;
using System.Threading.Tasks;
using TokyoGarden.IBL;
using TokyoGarden.IDAL;
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
    // Dummy object example
    public class DummyLogger { public void Log(string _msg) { /* no-op */ } }

    // Stub repository for menu positions
    public class PozycjeMenuRepoStub : IPozycjeMenuRepository
    {
        private readonly List<Pozycje_Menu> _data = new()
        {
            new Pozycje_Menu { id = 1, nazwa = "Ramen", cena = 39 },
            new Pozycje_Menu { id = 2, nazwa = "Sushi", cena = 49 }
        };

        public Task<Pozycje_Menu?> GetByIdAsync(int id) => Task.FromResult(_data.FirstOrDefault(x => x.id == id));
        public Task<IEnumerable<Pozycje_Menu>> GetAllAsync() => Task.FromResult(_data.AsEnumerable());
        public Task AddAsync(Pozycje_Menu entity) { entity.id = _data.Max(x=>x.id)+1; _data.Add(entity); return Task.CompletedTask; }
        public Task UpdateAsync(Pozycje_Menu entity) { var i=_data.FindIndex(x=>x.id==entity.id); if(i>=0)_data[i]=entity; return Task.CompletedTask; }
        public Task DeleteAsync(int id) { _data.RemoveAll(x=>x.id==id); return Task.CompletedTask; }
        public Task<IEnumerable<Pozycje_Menu>> GetByCategoryAsync(int kategoriaId) =>
            Task.FromResult(_data.Where(x => x.kategoria_id == kategoriaId).AsEnumerable());
    }

    // Fake UnitOfWork exposing stub
    public class UnitOfWorkFake : IUnitOfWork
    {
        public IPozycjeMenuRepository PozycjeMenu { get; }
        public UnitOfWorkFake(IPozycjeMenuRepository repo) { PozycjeMenu = repo; }

        public IUzytkownikRepository Uzytkownicy => throw new NotImplementedException();
        public IKategorieRepository Kategorie => throw new NotImplementedException();
        public IZamowieniaRepository Zamowienia => throw new NotImplementedException();
        public IPozycjeZamowieniaRepository PozycjeZamowienia => throw new NotImplementedException();
        public IAlergenyRepository Alergeny => throw new NotImplementedException();
        public IAdresyRepository Adresy => throw new NotImplementedException();

        // Spy: track save calls
        public int SaveCalls { get; private set; } = 0;
        public Task<int> SaveChangesAsync() { SaveCalls++; return Task.FromResult(1); }
        public ValueTask DisposeAsync() => ValueTask.CompletedTask;
    }

    public class KategorieServiceDummy : IKategorieService
    {
        public Task AddAsync(Kategorie k) => Task.CompletedTask;
        public Task DeleteAsync(int id) => Task.CompletedTask;
        public Task<IEnumerable<Kategorie>> GetAllAsync() => Task.FromResult(Enumerable.Empty<Kategorie>());
        public Task<Kategorie> GetByIdAsync(int id) => Task.FromResult(new Kategorie());
        public Task UpdateAsync(Kategorie k) => Task.CompletedTask;
    }

    public class PozycjeMenuServiceManualDoublesTests
    {
        [Fact]
        public async Task GetAll_Returns_From_Stub()
        {
            var stubRepo = new PozycjeMenuRepoStub();
            var uow = new UnitOfWorkFake(stubRepo);
            var svc = new PozycjeMenuService(uow); // expects IUnitOfWork in your BL

            var result = await svc.GetAllAsync();
            Assert.True(result.Any());
            Assert.Contains(result, x => x.nazwa == "Ramen");
        }

        [Fact]
        public async Task Add_Increments_Save_Spy_Called()
        {
            var stubRepo = new PozycjeMenuRepoStub();
            var uow = new UnitOfWorkFake(stubRepo);
            var svc = new PozycjeMenuService(uow);
            var before = uow.SaveCalls;

            await svc.AddAsync(new Pozycje_Menu { nazwa = "Udon", cena = 35 });
            var after = uow.SaveCalls;

            Assert.Equal(before + 1, after);
        }
    }
}
