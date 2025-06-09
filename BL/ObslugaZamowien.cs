using IBL;
using IDAL;
using Model;
using System.Collections.Generic;

namespace BL
{
    public class ObslugaZamowien : IObslugaZamowien
    {
        private readonly IZamowienieRepository _repo;

        public ObslugaZamowien(IZamowienieRepository repo)
        {
            _repo = repo;
        }

        public IEnumerable<Zamowienie> PobierzZamowieniaPosortowane()
        {
            return _repo.GetZamowienia().OrderByDescending(z => z.data_zamowienia);
        }

        public int PoliczZamowienia()
        {
            return _repo.GetZamowienia().Count();
        }

        public IEnumerable<Zamowienie> PobierzZamowieniaUzytkownika(int uzytkownikId)
        {
            return _repo.GetZamowienia().Where(z => z.UzytkownikId == uzytkownikId).OrderByDescending(z => z.data_zamowienia);
        }

        public IEnumerable<Zamowienie> PobierzZamowieniaPoStatusie(string status)
        {
            return _repo.GetZamowienia().Where(z => z.status_zamowienia.Equals(status, StringComparison.OrdinalIgnoreCase)).OrderByDescending(z => z.data_zamowienia);
        }

        public double ObliczLacznaWartoscZamowienUzytkownika(int uzytkownikId)
        {
            return _repo.GetZamowienia().Where(z => z.UzytkownikId == uzytkownikId).Sum(z => z.laczna_cena);
        }

        public bool CzyZamowienieIstnieje(int zamowienieId)
        {
            return _repo.GetZamowienia().Any(z => z.id == zamowienieId);
        }
    }
}