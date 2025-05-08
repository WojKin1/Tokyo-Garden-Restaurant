using IDAL;
using Model;
using System.Collections.Generic;
using System.Linq;

namespace DAL
{
    public class PozycjaZamowieniaRepository : IPozycjaZamowieniaRepository
    {
        private readonly DbTokyoGarden db;

        public PozycjaZamowieniaRepository(DbTokyoGarden db)
        {
            this.db = db;
        }

        public IEnumerable<PozycjaZamowienia> GetPozycjeZamowienia()
        {
            return db.PozycjeZamowienia.ToList();
        }

        public PozycjaZamowienia GetPozycjaZamowieniaByID(int id)
        {
            return db.PozycjeZamowienia.Find(id);
        }

        public void InsertPozycjaZamowienia(PozycjaZamowienia pozycja)
        {
            db.PozycjeZamowienia.Add(pozycja);
        }

        public void DeletePozycjaZamowienia(int id)
        {
            var p = db.PozycjeZamowienia.Find(id);
            if (p != null)
                db.PozycjeZamowienia.Remove(p);
        }

        public void UpdatePozycjaZamowienia(PozycjaZamowienia pozycja)
        {
            db.PozycjeZamowienia.Update(pozycja);
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
