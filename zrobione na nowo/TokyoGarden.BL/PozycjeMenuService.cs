using System.Collections.Generic;
using System.Threading.Tasks;
using TokyoGarden.IBL;
using TokyoGarden.IDAL;
using TokyoGarden.Model;

namespace TokyoGarden.BL
{
    public class PozycjeMenuService : IPozycjeMenuService
    {
        private readonly IPozycjeMenuRepository _repo;

        public PozycjeMenuService(IPozycjeMenuRepository repo)
        {
            _repo = repo;
        }

        public Task<IEnumerable<Pozycje_Menu>> GetAllAsync() => _repo.GetAllAsync();
        public Task<Pozycje_Menu> GetByIdAsync(int id) => _repo.GetByIdAsync(id);
        public Task AddAsync(Pozycje_Menu item) => _repo.AddAsync(item);
        public Task UpdateAsync(Pozycje_Menu item) => _repo.UpdateAsync(item);
        public Task DeleteAsync(int id) => _repo.DeleteAsync(id);
        public Task<IEnumerable<Pozycje_Menu>> GetByCategoryIdAsync(int categoryId) => _repo.GetByCategoryIdAsync(categoryId);
        public Task<IEnumerable<Pozycje_Menu>> SearchByNameAsync(string name) => _repo.SearchByNameAsync(name);
        public Task<IEnumerable<Pozycje_Menu>> GetByAllergenAsync(int allergenId) => _repo.GetByAllergenAsync(allergenId);
        public Task<IEnumerable<Pozycje_Menu>> GetAllWithDetailsAsync() => _repo.GetAllWithDetailsAsync();
    }
}
