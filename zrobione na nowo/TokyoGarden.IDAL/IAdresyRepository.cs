using TokyoGarden.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TokyoGarden.IDAL
{
    public interface IAdresyRepository : IRepository<Adresy>
    {
        Task<IEnumerable<Adresy>> GetByCityAsync(string city);
    }
}
