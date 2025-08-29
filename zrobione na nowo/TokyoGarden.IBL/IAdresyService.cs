using System.Collections.Generic;
using System.Threading.Tasks;
using TokyoGarden.Model;

namespace TokyoGarden.IBL
{
    public interface IAdresyService
    {
        Task<IEnumerable<Adresy>> GetAllAsync();
        Task<Adresy> GetByIdAsync(int id);
        Task AddAsync(Adresy address);
        Task UpdateAsync(Adresy address);
        Task DeleteAsync(int id);
        Task<IEnumerable<Adresy>> GetByCityAsync(string city);
    }
}
