using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TokyoGarden.IDAL;
using TokyoGarden.Model;

namespace TokyoGarden.DAL
{
    public class KategorieRepository : Repository<Kategorie>, IKategorieRepository
    {
        public KategorieRepository(DbTokyoGarden context) : base(context) { }

        public Task<Kategorie?> GetByNameAsync(string categoryName) =>
            _dbSet.FirstOrDefaultAsync(k => k.nazwa_kategorii == categoryName);
    }
}
