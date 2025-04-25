using Model;
using System.Collections.Generic;

namespace IBL
{
    public interface IObslugaKategorii
    {
        IEnumerable<Kategoria> PobierzKategorie();
        int PoliczKategorie();
    }
}
