using IBL;
using IDAL;
using Model;

namespace BL
{
    public class ObslugaUzytkownikow : IObslugaUzytkownik
    {
        private readonly IUzytkownikRepository UzytkownikRepo;

        public ObslugaUzytkownikow(IUzytkownikRepository repo)
        {
            UzytkownikRepo = repo;
        }

        public IEnumerable<Uzytkownik> PobierzPosortowaneUzytkownikow()
        {
            return UzytkownikRepo.GetUzytkownik().OrderBy(u => u.nazwisko);
        }

        public int PoliczUzytkownikow()
        {
            return UzytkownikRepo.GetUzytkownik().Count();
        }

        public bool CzyUzytkownikIstnieje(int uzytkownikID)
        {
            return UzytkownikRepo.UzytkownikExists(uzytkownikID);
        }

        public IEnumerable<Uzytkownik> PobierzUzytkownikowWedlugRoli(string rola)
        {
            return UzytkownikRepo.GetUzytkownikByRole(rola);
        }

    }
}
