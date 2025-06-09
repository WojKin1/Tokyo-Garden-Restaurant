using Model;

namespace IBL
{
    public interface IObslugaAdresow
    {
        IEnumerable<Adresy> PobierzPosortowaneAdresy();

        int PoliczAdresy();
        IEnumerable<Adresy> PobierzAdresyZWielkimiMiastami(int minimalnaDlugoscNazwyMiasta);  // 1
        IEnumerable<Adresy> PobierzAdresyBezNumeruMieszkania();                                // 2
        IEnumerable<Adresy> PobierzAdresyDlaUlicy(string ulica);                               // 3


    }
}
