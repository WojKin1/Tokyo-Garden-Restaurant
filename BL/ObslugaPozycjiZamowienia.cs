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

        public IEnumerable<PozycjaZamowienia> PobierzPozycjePoZamowieniu(int zamowienieId)
        {
            return _repo.GetPozycjeZamowienia().Where(p => p.ZamowienieId == zamowienieId);
        }

        public double ObliczWartoscZamowienia(int zamowienieId)
        {
            return _repo.GetPozycjeZamowienia().Where(p => p.ZamowienieId == zamowienieId).Sum(p => p.cena * p.ilosc);
        }

        public bool CzyPozycjaZamowieniaIstnieje(int id)
        {
            return _repo.GetPozycjeZamowienia().Any(p => p.id == id);
        }
    }
}
