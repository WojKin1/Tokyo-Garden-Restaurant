using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TokyoGarden.IDAL;
using TokyoGarden.Model;

namespace TokyoGarden.DAL
{
    public class UzytkownikRepository : Repository<Uzytkownicy>, IUzytkownikRepository
    {
        public UzytkownikRepository(DbTokyoGarden context) : base(context) { }

        public Task<Uzytkownicy?> GetByUsernameAsync(string username) =>
            _dbSet.FirstOrDefaultAsync(u => u.nazwa_uzytkownika == username);

        public Task<Uzytkownicy?> AuthenticateAsync(string username, string password) =>
            _dbSet.FirstOrDefaultAsync(u => u.nazwa_uzytkownika == username && u.haslo == password);

        public Task<bool> UsernameExistsAsync(string username) =>
            _dbSet.AnyAsync(u => u.nazwa_uzytkownika == username);
    }
}
