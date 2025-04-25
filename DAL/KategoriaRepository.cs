using IDAL;
using Model;
using System.Collections.Generic;

namespace DAL
{
    public class KategoriaRepository : IKategoriaRepository
    {
        private readonly DbTokyoGarden db;

        public KategoriaRepository(DbTokyoGarden db)
        {
            this.db = db;
        }

        public IEnumerable<Kategoria> GetKategorie()
        {
            return db.Kategorie.ToList();
        }

        public Kategoria GetKategoriaByID(int id)
        {
            return db.Kategorie.Find(id);
        }

        public void InsertKategoria(Kategoria kategoria)
        {
            db.Kategorie.Add(kategoria);
        }

        public void DeleteKategoria(int id)
        {
            var kat = db.Kategorie.Find(id);
            if (kat != null)
                db.Kategorie.Remove(kat);
        }

        public void UpdateKategoria(Kategoria kategoria)
        {
            db.Kategorie.Update(kategoria);
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
