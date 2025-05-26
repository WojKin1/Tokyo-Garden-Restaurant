using IDAL;
using Model;
using System.Collections.Generic;
using System.Linq;

namespace DAL
{
    public class UzytkownicyRepository : IUzytkownikRepository
    {
        private readonly DbTokyoGarden db;

        public Uzytkownik GetUzytkownikByEmail(string email)
        {
            return db.uzytkownik.FirstOrDefault(u => u.Email == email);
        }

        public bool UzytkownikExists(int uzytkownikID)
        {
            return db.uzytkownik.Any(u => u.ID == uzytkownikID);
        }

        public IEnumerable<Uzytkownik> GetUzytkownikByRole(string rola)
        {
            return db.uzytkownik.Where(u => u.Rola == rola).ToList();
        }
    }
}
