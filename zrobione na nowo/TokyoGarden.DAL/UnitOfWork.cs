using System.Threading.Tasks;
using TokyoGarden.IDAL;
using TokyoGarden.Model;

namespace TokyoGarden.DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbTokyoGarden _context;

        public UnitOfWork(DbTokyoGarden context)
        {
            _context = context;
        }

        public Task<int> SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }
    }
}
