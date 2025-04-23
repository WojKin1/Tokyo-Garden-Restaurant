using IDAL;
using Model;

namespace DAL
{
    public class AdresyRepository : IAdresyRepository

    {
        DbTokyoGarden db;

        public AdresyRepository(DbTokyoGarden db)
        {
            this.db = db;
        }
        public void DeleteAdres(int adresId)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Adresy> GetAdresy()
        {
            return db.Adres.ToList();
        }

        public Adresy GetAdresyByID(int adresId)
        {
            return db.Adres.Find(adresId);
        }

        public void InsertAdres(Adresy adres)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public void UpdateAdres(Adresy adres)
        {
            throw new NotImplementedException();
        }
    }
}
