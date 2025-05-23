﻿using IDAL;
using Model;
using System.Collections.Generic;
using System.Linq;

namespace DAL
{
    public class AlergenyRepository : IAlergenyRepository
    {
        private readonly DbTokyoGarden db;

        public AlergenyRepository(DbTokyoGarden db)
        {
            this.db = db;
        }

        public void DeleteAlergen(int alergenId)
        {
            var alergen = db.alergenies.Find(alergenId);
            if (alergen != null)
                db.alergenies.Remove(alergen);
        }

        public void Dispose()
        {
            db.Dispose();
        }

        public Alergeny GetAlergenyByID(int alergenId)
        {
            return db.alergenies.Find(alergenId);
        }

        public IEnumerable<Alergeny> GetAlergeny()
        {
            return db.alergenies.ToList();
        }

        public void InsertAlergen(Alergeny alergen)
        {
            db.alergenies.Add(alergen);
        }

        public void Save()
        {
            db.SaveChanges();
        }

        public void UpdateAlergen(Alergeny alergen)
        {
            db.alergenies.Update(alergen);
        }
    }
}
