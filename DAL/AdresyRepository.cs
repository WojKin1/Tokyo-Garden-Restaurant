using IDAL;
using Model;

namespace DAL
{
    public class AdresyRepository : IAdresyRepository

    {
        private readonly DbTokyoGarden db;

        public AdresyRepository(DbTokyoGarden db)
        {
            this.db = db;
        }

        public void InsertAdres(Adresy adres)
        {
            db.Adres.Add(adres);
        }

        public void UpdateAdres(Adresy adres)
        {
            db.Adres.Update(adres);
        }

        public void DeleteAdres(int adresId)
        {
            var adres = db.Adres.Find(adresId);
            if (adres != null)
                db.Adres.Remove(adres);
        }

        public void Save()
        {
            db.SaveChanges();
        }

        public void Dispose()
        {
            db.Dispose();
        }

        public IEnumerable<Adresy> GetAdresy()
        {
            return db.Adres.ToList();
        }

        public Adresy GetAdresyByID(int adresId)
        {
            return db.Adres.Find(adresId);
        }

    }
}
