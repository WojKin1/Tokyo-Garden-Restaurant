using System.Collections.Generic;
using System.Threading.Tasks;
using TokyoGarden.Model;

namespace TokyoGarden.IDAL
{
    public interface IPozycjeZamowieniaRepository : IRepository<Pozycje_Zamowienia>
    {
        Task<IEnumerable<Pozycje_Zamowienia>> GetByOrderIdAsync(int orderId);
        Task<IEnumerable<Pozycje_Zamowienia>> GetAllWithDetailsAsync();
    }
}
