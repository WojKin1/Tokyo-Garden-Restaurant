using Model;
using System.Collections.Generic;
using System;

namespace IDAL
{
    public interface IAlergenPozycjaMenuRepository : IDisposable
    {
        IEnumerable<AlergenPozycjaMenu> GetAlergenPozycjaMenu();
        AlergenPozycjaMenu GetByIDs(int idAlergen, int idPozycjaMenu);
        void InsertAlergenPozycjaMenu(AlergenPozycjaMenu apm);
        void DeleteAlergenPozycjaMenu(int idAlergen, int idPozycjaMenu);
        void Save();
    }
}
