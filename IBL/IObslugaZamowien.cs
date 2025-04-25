using Model;
using System.Collections.Generic;

namespace IBL
{
    public interface IObslugaZamowien
    {
        IEnumerable<Zamowienie> PobierzZamowieniaPosortowane();
        int PoliczZamowienia();
    }
}
