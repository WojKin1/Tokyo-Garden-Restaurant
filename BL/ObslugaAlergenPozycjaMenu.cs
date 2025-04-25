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
    }
}
