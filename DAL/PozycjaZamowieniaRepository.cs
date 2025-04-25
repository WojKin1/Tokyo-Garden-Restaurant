using IDAL;
using Model;
using System.Collections.Generic;

namespace DAL
{
    public class PozycjaZamowieniaRepository : IPozycjaZamowieniaRepository
    {
        private readonly DbTokyoGarden db;

        public PozycjaZamowieniaRepository(DbTokyoGarden db)
        {
            this.db = db;
        }

        public IEnumerable<PozycjaZamowienia> GetPozycje()
        {
            return db.PozycjeZamowienia.ToList();
        }

        public PozycjaZamowienia GetPozycjaByID(int id)
        {
            return db.PozycjeZamowienia.Find(id);
        }

        public void InsertPozycja(PozycjaZamowienia p)
        {
            db.PozycjeZamowienia.Add(p);
        }

        public void DeletePozycja(int id)
        {
            var p = db.PozycjeZamowienia.Find(id);
            if (p != null)
                db.PozycjeZamowienia.Remove(p);
        }

        public void UpdatePozycja(PozycjaZamowienia p)
        {
            db.PozycjeZamowienia.Update(p);
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
