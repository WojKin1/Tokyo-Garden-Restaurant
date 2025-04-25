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
    }
}
