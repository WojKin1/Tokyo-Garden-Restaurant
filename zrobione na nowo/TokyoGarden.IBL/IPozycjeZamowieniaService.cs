using System.Collections.Generic;
using System.Threading.Tasks;
using TokyoGarden.Model;

namespace TokyoGarden.IBL
{
    public interface IPozycjeZamowieniaService
    {
        Task<IEnumerable<Pozycje_Zamowienia>> GetAllAsync();
        Task<Pozycje_Zamowienia> GetByIdAsync(int id);
        Task AddAsync(Pozycje_Zamowienia item);
        Task UpdateAsync(Pozycje_Zamowienia item);
        Task DeleteAsync(int id);
        Task<IEnumerable<Pozycje_Zamowienia>> GetByOrderIdAsync(int orderId);
        Task<IEnumerable<Pozycje_Zamowienia>> GetAllWithDetailsAsync();
    }
}
