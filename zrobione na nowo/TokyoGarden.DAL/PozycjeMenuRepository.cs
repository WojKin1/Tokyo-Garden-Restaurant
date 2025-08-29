using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TokyoGarden.IDAL;
using TokyoGarden.Model;

namespace TokyoGarden.DAL
{
    public class PozycjeMenuRepository : Repository<Pozycje_Menu>, IPozycjeMenuRepository
    {
        public PozycjeMenuRepository(DbTokyoGarden context) : base(context) { }

        public async Task<IEnumerable<Pozycje_Menu>> GetByCategoryIdAsync(int categoryId)
        {
            return await _dbSet
                .Include(pm => pm.kategoria_menu)
                .Where(pm => pm.kategoria_menu.id == categoryId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Pozycje_Menu>> SearchByNameAsync(string name)
        {
            return await _dbSet
                .Include(pm => pm.kategoria_menu)
                .Where(pm => pm.nazwa_pozycji.Contains(name))
                .ToListAsync();
        }

        public async Task<IEnumerable<Pozycje_Menu>> GetByAllergenAsync(int allergenId)
        {
            return await _dbSet
                .Include(pm => pm.kategoria_menu)
                .Include(pm => pm.alergeny)
                .Where(pm => pm.alergeny.Any(a => a.id == allergenId))
                .ToListAsync();
        }

        public async Task<IEnumerable<Pozycje_Menu>> GetAllWithDetailsAsync()
        {
            return await _dbSet
                .Include(pm => pm.kategoria_menu)
                .Include(pm => pm.alergeny)
                .ToListAsync();
        }
    }
}
