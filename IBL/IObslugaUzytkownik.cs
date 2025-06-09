using Model;

namespace IBL
{
    public interface IObslugaUzytkownik
    {
        IEnumerable<Uzytkownik> PobierzPosortowaneUzytkownikow();

        int PoliczUzytkownikow();

        bool CzyUzytkownikIstnieje(int uzytkownikID);
        IEnumerable<Uzytkownik> PobierzUzytkownikowWedlugRoli(string rola);

    }
}
