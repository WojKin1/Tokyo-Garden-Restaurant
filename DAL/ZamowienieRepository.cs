using IDAL;
using Model;
using System.Collections.Generic;

namespace DAL
{
    public class ZamowienieRepository : IZamowienieRepository
    {
        private readonly DbTokyoGarden db;

        public ZamowienieRepository(DbTokyoGarden db)
        {
            this.db = db;
        }

        public IEnumerable<Zamowienie> GetZamowienia()
        {
            return db.Zamowienia.ToList();
        }

        public Zamowienie GetZamowienieByID(int id)
        {
            return db.Zamowienia.Find(id);
        }

        public void InsertZamowienie(Zamowienie z)
        {
            db.Zamowienia.Add(z);
        }

        public void DeleteZamowienie(int id)
        {
            var z = db.Zamowienia.Find(id);
            if (z != null)
                db.Zamowienia.Remove(z);
        }

        public void UpdateZamowienie(Zamowienie z)
        {
            db.Zamowienia.Update(z);
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
