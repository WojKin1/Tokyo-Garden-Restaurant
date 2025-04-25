using IDAL;
using Model;

namespace DAL
{
    public class UzytkownicyRepository : IUzytkownikRepository

    {
        private readonly DbTokyoGarden db;

        public UzytkownicyRepository(DbTokyoGarden db)
        {
            this.db = db;
        }
        public void DeleteUzytkownik(int uzytkownikId)
        {
            var uzytkownik = db.uzytkownik.Find(uzytkownikId);
            if (uzytkownik != null)
                db.uzytkownik.Remove(uzytkownik);
        }

        public void Dispose()
        {
            db.Dispose();
        }

        public IEnumerable<Uzytkownik> GetUzytkownik()
        {
            return db.uzytkownik.ToList();
        }

        public Uzytkownik GetUzytkownikByID(int UzytkownikId)
        {
            return db.uzytkownik.Find(UzytkownikId);
        }

        public void InsertUzytkownik(Uzytkownik uzytkownik)
        {
            db.uzytkownik.Add(uzytkownik);
        }

        public void Save()
        {
            db.SaveChanges();
        }

        public void UpdateUzytkownik(Uzytkownik uzytkownik)
        {
            db.uzytkownik.Update(uzytkownik);
        }
    }
}