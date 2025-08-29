using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TokyoGarden.IDAL;
using TokyoGarden.Model;

namespace TokyoGarden.DAL
{
    public class AdresyRepository : Repository<Adresy>, IAdresyRepository
    {
        public AdresyRepository(DbTokyoGarden context) : base(context) { }

        public async Task<IEnumerable<Adresy>> GetByCityAsync(string city)
        {
            return await _dbSet.Where(a => a.miasto.Contains(city)).ToListAsync();
        }
    }
}
