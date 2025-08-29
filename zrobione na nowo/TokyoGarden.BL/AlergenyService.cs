using System.Collections.Generic;
using System.Threading.Tasks;
using TokyoGarden.IBL;
using TokyoGarden.IDAL;
using TokyoGarden.Model;

namespace TokyoGarden.BL
{
    public class AlergenyService : IAlergenyService
    {
        private readonly IAlergenyRepository _repo;
        public AlergenyService(IAlergenyRepository repo) { _repo = repo; }

        public Task<IEnumerable<Alergeny>> GetAllAsync() => _repo.GetAllAsync();
        public Task<Alergeny?> GetByIdAsync(int id) => _repo.GetByIdAsync(id);
        public Task AddAsync(Alergeny allergen) => _repo.AddAsync(allergen);
        public Task UpdateAsync(Alergeny allergen) => _repo.UpdateAsync(allergen);
        public Task DeleteAsync(int id) => _repo.DeleteAsync(id);
        public Task<Alergeny?> GetByNameAsync(string name) => _repo.GetByNameAsync(name);
    }
}
