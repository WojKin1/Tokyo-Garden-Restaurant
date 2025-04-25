using Model;
using System.Collections.Generic;

namespace IBL
{
    public interface IObslugaPozycjiMenu
    {
        IEnumerable<PozycjaMenu> PobierzPosortowanePozycje();
        int PoliczPozycje();
    }
}
