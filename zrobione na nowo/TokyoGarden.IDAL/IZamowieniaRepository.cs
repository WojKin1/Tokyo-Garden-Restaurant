using System.Collections.Generic;
using System.Threading.Tasks;
using TokyoGarden.Model;

namespace TokyoGarden.IDAL
{
    public interface IZamowieniaRepository : IRepository<Zamowienia>
    {
        Task<IEnumerable<Zamowienia>> GetAllWithDetailsAsync();
        Task<Zamowienia> GetByIdWithDetailsAsync(int id);
        Task<IEnumerable<Zamowienia>> GetByUserIdAsync(int userId);
        Task<IEnumerable<Zamowienia>> GetByStatusAsync(string status);
    }
}
