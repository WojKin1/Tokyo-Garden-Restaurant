using IDAL;
using Model;
using System.Collections.Generic;
using System.Linq;

namespace DAL
{
    public class UzytkownicyRepository : IUzytkownikRepository
    {
        private readonly DbTokyoGarden db;

        public UzytkownicyRepository(DbTokyoGarden db)
        {
            this.db = db;
        }

        public IEnumerable<Uzytkownik> GetUzytkownik()
        {
            return db.uzytkownik.ToList();
        }

        public Uzytkownik GetUzytkownikByID(int uzytkownikID)
        {
            return db.uzytkownik.Find(uzytkownikID);
        }

        public void InsertUzytkownik(Uzytkownik uzytkownik)
        {
            db.uzytkownik.Add(uzytkownik);
        }

        public void DeleteUzytkownik(int uzytkownikID)
        {
            var uzytkownik = db.uzytkownik.Find(uzytkownikID);
            if (uzytkownik != null)
                db.uzytkownik.Remove(uzytkownik);
        }

        public void UpdateUzytkownik(Uzytkownik uzytkownik)
        {
            db.uzytkownik.Update(uzytkownik);
        }

        public void Save()
        {
            db.SaveChanges();
        }

        public void Dispose()
        {
            db.Dispose();
        }
    }
}
