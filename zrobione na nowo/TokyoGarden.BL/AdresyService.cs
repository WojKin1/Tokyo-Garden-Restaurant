using System.Collections.Generic;
using System.Threading.Tasks;
using TokyoGarden.IBL;
using TokyoGarden.IDAL;
using TokyoGarden.Model;

namespace TokyoGarden.BL
{
    public class AdresyService : IAdresyService
    {
        private readonly IAdresyRepository _repo;

        public AdresyService(IAdresyRepository repo)
        {
            _repo = repo;
        }

        public Task<IEnumerable<Adresy>> GetAllAsync() => _repo.GetAllAsync();
        public Task<Adresy> GetByIdAsync(int id) => _repo.GetByIdAsync(id);
        public Task AddAsync(Adresy address) => _repo.AddAsync(address);
        public Task UpdateAsync(Adresy address) => _repo.UpdateAsync(address);
        public Task DeleteAsync(int id) => _repo.DeleteAsync(id);
        public Task<IEnumerable<Adresy>> GetByCityAsync(string city) => _repo.GetByCityAsync(city);
    }
}
