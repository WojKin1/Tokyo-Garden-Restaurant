using Model;
using System.Collections.Generic;

namespace IBL
{
    public interface IObslugaPozycjiMenu
    {
        IEnumerable<PozycjaMenu> PobierzPosortowanePozycje();
        int PoliczPozycje();
        IEnumerable<PozycjaMenu> WyszukajPozycje(string fragmentNazwy);
        IEnumerable<PozycjaMenu> PobierzPozycjePoKategorii(int kategoriaId);
        bool CzyPozycjaIstnieje(string nazwa);
    }
}
