using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TokyoGarden.BL;
using TokyoGarden.IDAL;
using TokyoGarden.Model;
using Xunit;

namespace TokyoGarden.BL.Tests
{
    // Prosty stub+spy repozytorium dla IPozycjeMenuRepository
    public class PozycjeMenuRepoStub : IPozycjeMenuRepository
    {
        private readonly List<Pozycje_Menu> _data = new()
        {
            new Pozycje_Menu { id = 1, nazwa_pozycji = "Ramen", cena = 39 },
            new Pozycje_Menu { id = 2, nazwa_pozycji = "Sushi", cena = 49 }
        };

        public int AddCalls { get; private set; }

        public Task<Pozycje_Menu?> GetByIdAsync(int id) => Task.FromResult(_data.FirstOrDefault(x => x.id == id));
        public Task<IEnumerable<Pozycje_Menu>> GetAllAsync() => Task.FromResult(_data.AsEnumerable());
        public Task AddAsync(Pozycje_Menu entity) { AddCalls++; entity.id = _data.Max(x=>x.id)+1; _data.Add(entity); return Task.CompletedTask; }
        public Task UpdateAsync(Pozycje_Menu entity) { var i=_data.FindIndex(x=>x.id==entity.id); if(i>=0)_data[i]=entity; return Task.CompletedTask; }
        public Task DeleteAsync(int id) { _data.RemoveAll(x=>x.id==id); return Task.CompletedTask; }
        public Task<IEnumerable<Pozycje_Menu>> GetByCategoryIdAsync(int categoryId) => Task.FromResult(Enumerable.Empty<Pozycje_Menu>());
        public Task<IEnumerable<Pozycje_Menu>> SearchByNameAsync(string name) => Task.FromResult(_data.Where(x=>x.nazwa_pozycji.Contains(name)).AsEnumerable());
        public Task<IEnumerable<Pozycje_Menu>> GetByAllergenAsync(int allergenId) => Task.FromResult(Enumerable.Empty<Pozycje_Menu>());
        public Task<IEnumerable<Pozycje_Menu>> GetAllWithDetailsAsync() => GetAllAsync();
    }

    public class PozycjeMenuServiceManualDoublesTests
    {
        [Fact]
        public async Task GetAll_Returns_From_Stub()
        {
            var stubRepo = new PozycjeMenuRepoStub();
            var svc = new PozycjeMenuService(stubRepo);

            var result = await svc.GetAllAsync();
            Assert.True(result.Any());
            Assert.Contains(result, x => x.nazwa_pozycji == "Ramen");
        }

        [Fact]
        public async Task Add_Increments_Spy_Called()
        {
            var stubRepo = new PozycjeMenuRepoStub();
            var svc = new PozycjeMenuService(stubRepo);
            var before = stubRepo.AddCalls;

            await svc.AddAsync(new Pozycje_Menu { nazwa_pozycji = "Udon", cena = 35 });
            var after = stubRepo.AddCalls;

            Assert.Equal(before + 1, after);
        }
    }
}
