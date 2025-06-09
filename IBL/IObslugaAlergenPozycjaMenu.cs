using Model;
using System.Collections.Generic;

namespace IBL
{
    public interface IObslugaAlergenPozycjaMenu
    {
        IEnumerable<AlergenPozycjaMenu> PobierzWszystkiePowiazania();
        int PoliczPowiazania();
        IEnumerable<AlergenPozycjaMenu> PobierzPowiazaniaDlaAlergenu(int idAlergen);
        IEnumerable<AlergenPozycjaMenu> PobierzPowiazaniaDlaPozycjiMenu(int idPozycjaMenu);
        bool CzyPozycjaZawieraAlergen(int idPozycjaMenu, int idAlergen);
    }
}
