using System.Collections.Generic;
using System.Threading.Tasks;
using TokyoGarden.Model;

namespace TokyoGarden.IBL
{
    public interface IZamowieniaService
    {
        Task<IEnumerable<Zamowienia>> GetAllAsync();
        Task<Zamowienia> GetByIdAsync(int id);
        Task AddAsync(Zamowienia order);
        Task UpdateAsync(Zamowienia order);
        Task DeleteAsync(int id);
        Task<IEnumerable<Zamowienia>> GetAllWithDetailsAsync();
        Task<Zamowienia> GetByIdWithDetailsAsync(int id);
        Task<IEnumerable<Zamowienia>> GetByUserIdAsync(int userId);
        Task<IEnumerable<Zamowienia>> GetByStatusAsync(string status);
        Task<Zamowienia> RecalculateTotalAsync(int id);
    }
}
