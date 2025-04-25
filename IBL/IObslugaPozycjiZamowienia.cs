using Model;
using System.Collections.Generic;

namespace IBL
{
    public interface IObslugaPozycjiZamowienia
    {
        IEnumerable<PozycjaZamowienia> PobierzPozycjeZamowienia();
        int PoliczPozycjeZamowienia();
    }
}
