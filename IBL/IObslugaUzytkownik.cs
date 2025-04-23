using Model;

namespace IBL
{
    public interface IObslugaUzytkownik
    {
        IEnumerable<Uzytkownik> PobierzPosortowaneUzytkownikow();

        int PoliczUzytkownikow();

    }
}
