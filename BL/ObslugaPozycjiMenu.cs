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
    }
}
