using IDAL;
using Model;
using System.Collections.Generic;

namespace DAL
{
    public class PozycjaMenuRepository : IPozycjaMenuRepository
    {
        private readonly DbTokyoGarden db;

        public PozycjaMenuRepository(DbTokyoGarden db)
        {
            this.db = db;
        }

        public IEnumerable<PozycjaMenu> GetPozycje()
        {
            return db.PozycjeMenu.ToList();
        }

        public PozycjaMenu GetPozycjaByID(int id)
        {
            return db.PozycjeMenu.Find(id);
        }

        public void InsertPozycja(PozycjaMenu p)
        {
            db.PozycjeMenu.Add(p);
        }

        public void DeletePozycja(int id)
        {
            var p = db.PozycjeMenu.Find(id);
            if (p != null)
                db.PozycjeMenu.Remove(p);
        }

        public void UpdatePozycja(PozycjaMenu p)
        {
            db.PozycjeMenu.Update(p);
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
