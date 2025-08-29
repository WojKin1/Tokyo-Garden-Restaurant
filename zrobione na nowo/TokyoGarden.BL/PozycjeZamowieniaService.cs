using System.Collections.Generic;
using System.Threading.Tasks;
using TokyoGarden.IBL;
using TokyoGarden.IDAL;
using TokyoGarden.Model;

namespace TokyoGarden.BL
{
    public class PozycjeZamowieniaService : IPozycjeZamowieniaService
    {
        private readonly IPozycjeZamowieniaRepository _repo;

        public PozycjeZamowieniaService(IPozycjeZamowieniaRepository repo)
        {
            _repo = repo;
        }

        public Task<IEnumerable<Pozycje_Zamowienia>> GetAllAsync() => _repo.GetAllAsync();
        public Task<Pozycje_Zamowienia> GetByIdAsync(int id) => _repo.GetByIdAsync(id);
        public Task AddAsync(Pozycje_Zamowienia item) => _repo.AddAsync(item);
        public Task UpdateAsync(Pozycje_Zamowienia item) => _repo.UpdateAsync(item);
        public Task DeleteAsync(int id) => _repo.DeleteAsync(id);
        public Task<IEnumerable<Pozycje_Zamowienia>> GetByOrderIdAsync(int orderId) => _repo.GetByOrderIdAsync(orderId);
        public Task<IEnumerable<Pozycje_Zamowienia>> GetAllWithDetailsAsync() => _repo.GetAllWithDetailsAsync();
    }
}
