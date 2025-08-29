using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TokyoGarden.IDAL;
using TokyoGarden.Model;

namespace TokyoGarden.DAL
{
    public class PozycjeZamowieniaRepository : Repository<Pozycje_Zamowienia>, IPozycjeZamowieniaRepository
    {
        public PozycjeZamowieniaRepository(DbTokyoGarden context) : base(context) { }

        public async Task<IEnumerable<Pozycje_Zamowienia>> GetByOrderIdAsync(int orderId)
        {
            return await _dbSet
                .Include(pz => pz.pozycja_menu)
                    .ThenInclude(pm => pm.kategoria_menu)
                .Include(pz => pz.zamowienie)
                .Where(pz => pz.zamowienie.id == orderId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Pozycje_Zamowienia>> GetAllWithDetailsAsync()
        {
            return await _dbSet
                .Include(pz => pz.pozycja_menu)
                    .ThenInclude(pm => pm.kategoria_menu)
                .Include(pz => pz.zamowienie)
                .ToListAsync();
        }
    }
}
