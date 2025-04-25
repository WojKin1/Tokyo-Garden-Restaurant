using System;

namespace IDAL
{
    public interface IUnitOfWork : IDisposable
    {
        IAdresyRepository Adresy { get; }
        IAlergenyRepository Alergeny { get; }
        IAlergenPozycjaMenuRepository AlergenPozycjaMenu { get; }
        IKategoriaRepository Kategorie { get; }
        IPozycjaMenuRepository PozycjeMenu { get; }
        IPozycjaZamowieniaRepository PozycjeZamowienia { get; }
        IUzytkownikRepository Uzytkownicy { get; }
        IZamowienieRepository Zamowienia { get; }

        void Save();
    }
}
