using IDAL;
using Model;

namespace DAL
{
    public class UzytkownicyRepository : IUzytkownikRepository

    {
        DbTokyoGarden db;

        public UzytkownicyRepository(DbTokyoGarden db)
        {
            this.db = db;
        }
        public void DeleteUzytkownik(int uzytkownikId)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Uzytkownik> GetUzytkownik()
        {
            return db.uzytkownik.ToList();
        }

        public Uzytkownik GetUzytkownikByID(int UzytkownikId)
        {
            return db.uzytkownik.Find(UzytkownikId);
        }

        public void InsertAdres(Uzytkownik uzytkownik)
        {
            throw new NotImplementedException();
        }

        public void InsertUzytkownik(Uzytkownik uzytkownik)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public void UpdateUzytkownik(Uzytkownik uzytkownik)
        {
            throw new NotImplementedException();
        }
    }
}