using System.Collections.Generic;
using System.Threading.Tasks;
using TokyoGarden.Model;

namespace TokyoGarden.IBL
{
    public interface IPozycjeMenuService
    {
        Task<IEnumerable<Pozycje_Menu>> GetAllAsync();
        Task<Pozycje_Menu> GetByIdAsync(int id);
        Task AddAsync(Pozycje_Menu item);
        Task UpdateAsync(Pozycje_Menu item);
        Task DeleteAsync(int id);
        Task<IEnumerable<Pozycje_Menu>> GetByCategoryIdAsync(int categoryId);
        Task<IEnumerable<Pozycje_Menu>> SearchByNameAsync(string name);
        Task<IEnumerable<Pozycje_Menu>> GetByAllergenAsync(int allergenId);
        Task<IEnumerable<Pozycje_Menu>> GetAllWithDetailsAsync();
    }
}
