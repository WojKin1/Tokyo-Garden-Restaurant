using IBL;
using IDAL;
using Model;
using System.Collections.Generic;

namespace BL
{
    public class ObslugaAlergenPozycjaMenu : IObslugaAlergenPozycjaMenu
    {
        private readonly IAlergenPozycjaMenuRepository _repo;

        public ObslugaAlergenPozycjaMenu(IAlergenPozycjaMenuRepository repo)
        {
            _repo = repo;
        }

        public IEnumerable<AlergenPozycjaMenu> PobierzWszystkiePowiazania()
        {
            return _repo.GetAlergenPozycjaMenu();
        }

        public int PoliczPowiazania()
        {
            return _repo.GetAlergenPozycjaMenu().Count();
        }

        public IEnumerable<AlergenPozycjaMenu> PobierzPowiazaniaDlaAlergenu(int idAlergen)
        {
            return _repo.GetAlergenPozycjaMenu().Where(p => p.id_alergen == idAlergen);
        }

        public IEnumerable<AlergenPozycjaMenu> PobierzPowiazaniaDlaPozycjiMenu(int idPozycjaMenu)
        {
            return _repo.GetAlergenPozycjaMenu().Where(p => p.id_pozycja_menu == idPozycjaMenu);
        }

        public bool CzyPozycjaZawieraAlergen(int idPozycjaMenu, int idAlergen)
        {
            return _repo.GetAlergenPozycjaMenu().Any(p => p.id_pozycja_menu == idPozycjaMenu && p.id_alergen == idAlergen);
        }
    }
}
