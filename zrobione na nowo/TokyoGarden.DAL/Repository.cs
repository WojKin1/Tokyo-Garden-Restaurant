using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using TokyoGarden.IDAL;

namespace TokyoGarden.DAL
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly DbTokyoGarden _context;
        protected readonly DbSet<T> _dbSet;

        public Repository(DbTokyoGarden context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync() => await _dbSet.ToListAsync();

        public async Task<T> GetByIdAsync(int id)
        {
            var found = await _dbSet.FindAsync(id);
            return found!; // świadomie: kontrakt IRepository<T> zwraca T (nie T?)
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
