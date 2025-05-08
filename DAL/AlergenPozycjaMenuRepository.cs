using IDAL;
using Model;
using System.Collections.Generic;
using System.Linq;

namespace DAL
{
    public class AlergenPozycjaMenuRepository : IAlergenPozycjaMenuRepository
    {
        private readonly DbTokyoGarden db;

        public AlergenPozycjaMenuRepository(DbTokyoGarden db)
        {
            this.db = db;
        }

        public IEnumerable<AlergenPozycjaMenu> GetAlergenPozycjaMenu()
        {
            return db.AlergenPozycjaMenu.ToList();
        }

        public AlergenPozycjaMenu GetByIDs(int idAlergen, int idPozycjaMenu)
        {
            return db.AlergenPozycjaMenu.FirstOrDefault(x => x.id_alergen == idAlergen && x.id_pozycja_menu == idPozycjaMenu);
        }

        public void InsertAlergenPozycjaMenu(AlergenPozycjaMenu apm)
        {
            db.AlergenPozycjaMenu.Add(apm);
        }

        public void DeleteAlergenPozycjaMenu(int idAlergen, int idPozycjaMenu)
        {
            var entity = GetByIDs(idAlergen, idPozycjaMenu);
            if (entity != null)
                db.AlergenPozycjaMenu.Remove(entity);
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
