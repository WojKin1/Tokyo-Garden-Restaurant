using Model;
using System.Collections.Generic;
using System;

namespace IDAL
{
    public interface IPozycjaMenuRepository : IDisposable
    {
        IEnumerable<PozycjaMenu> GetPozycjeMenu();
        PozycjaMenu GetPozycjaMenuByID(int id);
        void InsertPozycjaMenu(PozycjaMenu pozycja);
        void DeletePozycjaMenu(int id);
        void UpdatePozycjaMenu(PozycjaMenu pozycja);
        void Save();
    }
}
