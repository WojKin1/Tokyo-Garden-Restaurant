using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TokyoGarden.IDAL;
using TokyoGarden.Model;

namespace TokyoGarden.DAL
{
    public class AlergenyRepository : Repository<Alergeny>, IAlergenyRepository
    {
        public AlergenyRepository(DbTokyoGarden context) : base(context) { }

        public Task<Alergeny?> GetByNameAsync(string allergenName) =>
            _dbSet.FirstOrDefaultAsync(a => a.nazwa_alergenu == allergenName);
    }
}
