using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TokyoGarden.IDAL;
using TokyoGarden.Model;

namespace TokyoGarden.DAL
{
    public class ZamowieniaRepository : Repository<Zamowienia>, IZamowieniaRepository
    {
        public ZamowieniaRepository(DbTokyoGarden context) : base(context) { }

        public async Task<IEnumerable<Zamowienia>> GetAllWithDetailsAsync()
        {
            return await _dbSet
                .Include(z => z.uzytkownik)
                .Include(z => z.pozycje_zamowienia)
                    .ThenInclude(pz => pz.pozycja_menu)
                        .ThenInclude(pm => pm.kategoria_menu)
                .ToListAsync();
        }

        public async Task<Zamowienia> GetByIdWithDetailsAsync(int id)
        {
            return await _dbSet
                .Include(z => z.uzytkownik)
                .Include(z => z.pozycje_zamowienia)
                    .ThenInclude(pz => pz.pozycja_menu)
                        .ThenInclude(pm => pm.kategoria_menu)
                .FirstOrDefaultAsync(z => z.id == id);
        }

        public async Task<IEnumerable<Zamowienia>> GetByUserIdAsync(int userId)
        {
            return await _dbSet
                .Include(z => z.uzytkownik)
                .Where(z => z.uzytkownik.id == userId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Zamowienia>> GetByStatusAsync(string status)
        {
            return await _dbSet
                .Where(z => z.status_zamowienia == status)
                .ToListAsync();
        }
    }
}
