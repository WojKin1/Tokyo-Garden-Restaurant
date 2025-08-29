using System.Collections.Generic;
using System.Threading.Tasks;
using TokyoGarden.Model;

namespace TokyoGarden.IBL
{
    public interface IKategorieService
    {
        Task<IEnumerable<Kategorie>> GetAllAsync();
        Task<Kategorie?> GetByIdAsync(int id);
        Task AddAsync(Kategorie category);
        Task UpdateAsync(Kategorie category);
        Task DeleteAsync(int id);
        Task<Kategorie?> GetByNameAsync(string name);
    }
}
