using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TokyoGarden.IBL;
using TokyoGarden.IDAL;
using TokyoGarden.Model;

namespace TokyoGarden.BL
{
    public class ZamowieniaService : IZamowieniaService
    {
        private readonly IZamowieniaRepository _repo;
        private readonly IPozycjeZamowieniaRepository _pozycjeRepo;

        public ZamowieniaService(IZamowieniaRepository repo, IPozycjeZamowieniaRepository pozycjeRepo)
        {
            _repo = repo;
            _pozycjeRepo = pozycjeRepo;
        }

        public Task<IEnumerable<Zamowienia>> GetAllAsync() => _repo.GetAllAsync();
        public Task<Zamowienia> GetByIdAsync(int id) => _repo.GetByIdAsync(id);
        public Task AddAsync(Zamowienia order) => _repo.AddAsync(order);
        public Task UpdateAsync(Zamowienia order) => _repo.UpdateAsync(order);
        public Task DeleteAsync(int id) => _repo.DeleteAsync(id);

        public Task<IEnumerable<Zamowienia>> GetAllWithDetailsAsync() => _repo.GetAllWithDetailsAsync();
        public Task<Zamowienia> GetByIdWithDetailsAsync(int id) => _repo.GetByIdWithDetailsAsync(id);
        public Task<IEnumerable<Zamowienia>> GetByUserIdAsync(int userId) => _repo.GetByUserIdAsync(userId);
        public Task<IEnumerable<Zamowienia>> GetByStatusAsync(string status) => _repo.GetByStatusAsync(status);

        public async Task<Zamowienia> RecalculateTotalAsync(int id)
        {
            var order = await _repo.GetByIdWithDetailsAsync(id);
            if (order == null) return null;

            order.laczna_cena = order.pozycje_zamowienia?.Sum(p => p.cena * p.ilosc) ?? 0m;
            await _repo.UpdateAsync(order);
            return order;
        }
    }
}
