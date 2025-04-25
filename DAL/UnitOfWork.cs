using IDAL;
using Model;
using System;

namespace DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbTokyoGarden db;

        public UnitOfWork(DbTokyoGarden context)
        {
            db = context;
            Adresy = new AdresyRepository(db);
            Alergeny = new AlergenyRepository(db);
            AlergenPozycjaMenu = new AlergenPozycjaMenuRepository(db);
            Kategorie = new KategoriaRepository(db);
            PozycjeMenu = new PozycjaMenuRepository(db);
            PozycjeZamowienia = new PozycjaZamowieniaRepository(db);
            Uzytkownicy = new UzytkownicyRepository(db);
            Zamowienia = new ZamowienieRepository(db);
        }

        public IAdresyRepository Adresy { get; private set; }
        public IAlergenyRepository Alergeny { get; private set; }
        public IAlergenPozycjaMenuRepository AlergenPozycjaMenu { get; private set; }
        public IKategoriaRepository Kategorie { get; private set; }
        public IPozycjaMenuRepository PozycjeMenu { get; private set; }
        public IPozycjaZamowieniaRepository PozycjeZamowienia { get; private set; }
        public IUzytkownikRepository Uzytkownicy { get; private set; }
        public IZamowienieRepository Zamowienia { get; private set; }

        public void Save()
        {
            db.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
                disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
