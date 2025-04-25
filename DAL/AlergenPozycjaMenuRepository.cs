using IDAL;
using Model;
using System.Collections.Generic;

namespace DAL
{
    public class AlergenPozycjaMenuRepository : IAlergenPozycjaMenuRepository
    {
        private readonly DbTokyoGarden db;

        public AlergenPozycjaMenuRepository(DbTokyoGarden db)
        {
            this.db = db;
        }

        public IEnumerable<AlergenPozycjaMenu> GetAll()
        {
            return db.AlergenPozycjaMenu.ToList();
        }

        public AlergenPozycjaMenu GetById(int pozycjaId, int alergenId)
        {
            return db.AlergenPozycjaMenu.FirstOrDefault(x => x.id_pozycja_menu == pozycjaId && x.id_alergen == alergenId);
        }

        public void Insert(AlergenPozycjaMenu entity)
        {
            db.AlergenPozycjaMenu.Add(entity);
        }

        public void Delete(int pozycjaId, int alergenId)
        {
            var entity = GetById(pozycjaId, alergenId);
            if (entity != null)
                db.AlergenPozycjaMenu.Remove(entity);
        }

        public void Update(AlergenPozycjaMenu entity)
        {
            db.AlergenPozycjaMenu.Update(entity);
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
