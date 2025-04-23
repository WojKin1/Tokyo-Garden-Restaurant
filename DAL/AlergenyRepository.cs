using IDAL;
using Model;

namespace DAL
{
    class AlergenyRepository : IAlergenyRepository
    {
        private DbTokyoGarden db;

        public AlergenyRepository(DbTokyoGarden db)
        {
            this.db = db;
        }
        public void DeleteAlergen(int alergenId)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Alergeny> GetAlergen()
        {
            return db.alergenies.ToList();
        }

        public Alergeny GetAlergenByID(int AlergenId)
        {
            return db.alergenies.Find(AlergenId);
        }

        public IEnumerable<Alergeny> GetAlergeny()
        {
            throw new NotImplementedException();
        }

        public Alergeny GetAlergenyByID(int alergenId)
        {
            throw new NotImplementedException();
        }

        public void InsertAlergen(Alergeny alergen)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public void UpdateAlergen(Alergeny alergen)
        {
            throw new NotImplementedException();
        }
    }
}