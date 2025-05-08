using IDAL;
using Model;
using System.Collections.Generic;
using System.Linq;

namespace DAL
{
    public class PozycjaMenuRepository : IPozycjaMenuRepository
    {
        private readonly DbTokyoGarden db;

        public PozycjaMenuRepository(DbTokyoGarden db)
        {
            this.db = db;
        }

        public IEnumerable<PozycjaMenu> GetPozycjeMenu()
        {
            return db.PozycjeMenu.ToList();
        }

        public PozycjaMenu GetPozycjaMenuByID(int id)
        {
            return db.PozycjeMenu.Find(id);
        }

        public void InsertPozycjaMenu(PozycjaMenu pozycja)
        {
            db.PozycjeMenu.Add(pozycja);
        }

        public void DeletePozycjaMenu(int id)
        {
            var p = db.PozycjeMenu.Find(id);
            if (p != null)
                db.PozycjeMenu.Remove(p);
        }

        public void UpdatePozycjaMenu(PozycjaMenu pozycja)
        {
            db.PozycjeMenu.Update(pozycja);
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
