using System.Collections.Generic;
using System.Threading.Tasks;
using TokyoGarden.Model;

namespace TokyoGarden.IDAL
{
    public interface IPozycjeMenuRepository : IRepository<Pozycje_Menu>
    {
        Task<IEnumerable<Pozycje_Menu>> GetByCategoryIdAsync(int categoryId);
        Task<IEnumerable<Pozycje_Menu>> SearchByNameAsync(string name);
        Task<IEnumerable<Pozycje_Menu>> GetByAllergenAsync(int allergenId);
        Task<IEnumerable<Pozycje_Menu>> GetAllWithDetailsAsync();
    }
}
