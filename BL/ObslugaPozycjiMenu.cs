using IBL;
using IDAL;
using Model;
using System.Collections.Generic;

namespace BL
{
    public class ObslugaPozycjiMenu : IObslugaPozycjiMenu
    {
        private readonly IPozycjaMenuRepository _repo;

        public ObslugaPozycjiMenu(IPozycjaMenuRepository repo)
        {
            _repo = repo;
        }

        public IEnumerable<PozycjaMenu> PobierzPosortowanePozycje()
        {
            return _repo.GetPozycje().OrderBy(p => p.nazwa_pozycji);
        }

        public int PoliczPozycje()
        {
            return _repo.GetPozycje().Count();
        }

        public IEnumerable<PozycjaMenu> WyszukajPozycje(string fragmentNazwy)
        {
            if (string.IsNullOrWhiteSpace(fragmentNazwy))
                return PobierzPosortowanePozycje();

            return _repo.GetPozycje().Where(p => p.nazwa_pozycji.Contains(fragmentNazwy, StringComparison.OrdinalIgnoreCase)).OrderBy(p => p.nazwa_pozycji);
        }

        public IEnumerable<PozycjaMenu> PobierzPozycjePoKategorii(int kategoriaId)
        {
            return _repo.GetPozycje().Where(p => p.KategoriaId == kategoriaId).OrderBy(p => p.nazwa_pozycji);
        }

        public bool CzyPozycjaIstnieje(string nazwa)
        {
            if (string.IsNullOrWhiteSpace(nazwa))
                return false;

            return _repo.GetPozycje().Any(p => p.nazwa_pozycji.Equals(nazwa, StringComparison.OrdinalIgnoreCase));
        }
    }
}
