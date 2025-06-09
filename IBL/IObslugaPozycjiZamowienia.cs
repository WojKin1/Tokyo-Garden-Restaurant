using Model;
using System.Collections.Generic;

namespace IBL
{
    public interface IObslugaPozycjiZamowienia
    {
        IEnumerable<PozycjaZamowienia> PobierzPozycjeZamowienia();
        int PoliczPozycjeZamowienia();
        IEnumerable<PozycjaZamowienia> PobierzPozycjePoZamowieniu(int zamowienieId);
        double ObliczWartoscZamowienia(int zamowienieId);
        bool CzyPozycjaZamowieniaIstnieje(int id);
    }
}
