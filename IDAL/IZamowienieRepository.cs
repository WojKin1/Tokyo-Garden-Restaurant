using Model;
using System.Collections.Generic;
using System;

namespace IDAL
{
    public interface IZamowienieRepository : IDisposable
    {
        IEnumerable<Zamowienie> GetZamowienia();
        Zamowienie GetZamowienieByID(int zamowienieId);
        void InsertZamowienie(Zamowienie zamowienie);
        void DeleteZamowienie(int zamowienieId);
        void UpdateZamowienie(Zamowienie zamowienie);
        void Save();
    }
}
