using System.Collections.Generic;
using System.Threading.Tasks;
using TokyoGarden.IBL;
using TokyoGarden.IDAL;
using TokyoGarden.Model;

namespace TokyoGarden.BL
{
    public class KategorieService : IKategorieService
    {
        private readonly IKategorieRepository _repo;
        public KategorieService(IKategorieRepository repo) { _repo = repo; }

        public Task<IEnumerable<Kategorie>> GetAllAsync() => _repo.GetAllAsync();
        public Task<Kategorie?> GetByIdAsync(int id) => _repo.GetByIdAsync(id);
        public Task AddAsync(Kategorie category) => _repo.AddAsync(category);
        public Task UpdateAsync(Kategorie category) => _repo.UpdateAsync(category);
        public Task DeleteAsync(int id) => _repo.DeleteAsync(id);
        public Task<Kategorie?> GetByNameAsync(string name) => _repo.GetByNameAsync(name);
    }
}
