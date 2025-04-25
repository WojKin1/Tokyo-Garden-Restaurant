using IBL;
using IDAL;
using Model;
using System.Collections.Generic;

namespace BL
{
    public class ObslugaPozycjiZamowienia : IObslugaPozycjiZamowienia
    {
        private readonly IPozycjaZamowieniaRepository _repo;

        public ObslugaPozycjiZamowienia(IPozycjaZamowieniaRepository repo)
        {
            _repo = repo;
        }

        public IEnumerable<PozycjaZamowienia> PobierzPozycjeZamowienia()
        {
            return _repo.GetPozycjeZamowienia();
        }

        public int PoliczPozycjeZamowienia()
        {
            return _repo.GetPozycjeZamowienia().Count();
        }
    }
}
