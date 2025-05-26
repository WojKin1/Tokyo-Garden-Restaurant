using Model;

namespace IDAL
{
     public interface IUzytkownikRepository : IDisposable
    {
        IEnumerable<Uzytkownik> GetUzytkownik();
        Uzytkownik GetUzytkownikByID(int uzytkownikID);
        void InsertUzytkownik(Uzytkownik uzytkownik);
        void DeleteUzytkownik(int uzytkownikID);
        void UpdateUzytkownik(Uzytkownik uzytkownik);
        void Save();
        Uzytkownik GetUzytkownikByEmail(string email);
        bool UzytkownikExists(int uzytkownikID);
        IEnumerable<Uzytkownik> GetUzytkownikByRole(string rola);

    }
}

