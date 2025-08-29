using System.Collections.Generic;
using System.Threading.Tasks;
using TokyoGarden.Model;

namespace TokyoGarden.IBL
{
    public interface IAlergenyService
    {
        Task<IEnumerable<Alergeny>> GetAllAsync();
        Task<Alergeny?> GetByIdAsync(int id);
        Task AddAsync(Alergeny allergen);
        Task UpdateAsync(Alergeny allergen);
        Task DeleteAsync(int id);
        Task<Alergeny?> GetByNameAsync(string name);
    }
}
