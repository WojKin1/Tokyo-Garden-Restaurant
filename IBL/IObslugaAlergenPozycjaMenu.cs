using Model;
using System.Collections.Generic;

namespace IBL
{
    public interface IObslugaAlergenPozycjaMenu
    {
        IEnumerable<AlergenPozycjaMenu> PobierzWszystkiePowiazania();
        int PoliczPowiazania();
    }
}
