using Model;
using System.Collections.Generic;
using System;

namespace IDAL
{
    public interface IPozycjaZamowieniaRepository : IDisposable
    {
        IEnumerable<PozycjaZamowienia> GetPozycjeZamowienia();
        PozycjaZamowienia GetPozycjaZamowieniaByID(int id);
        void InsertPozycjaZamowienia(PozycjaZamowienia pozycja);
        void DeletePozycjaZamowienia(int id);
        void UpdatePozycjaZamowienia(PozycjaZamowienia pozycja);
        void Save();
    }
}
