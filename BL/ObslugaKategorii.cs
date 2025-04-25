using IBL;
using IDAL;
using Model;
using System.Collections.Generic;

namespace BL
{
    public class ObslugaKategorii : IObslugaKategorii
    {
        private readonly IKategoriaRepository _repo;

        public ObslugaKategorii(IKategoriaRepository repo)
        {
            _repo = repo;
        }

        public IEnumerable<Kategoria> PobierzKategorie()
        {
            return _repo.GetKategorie().OrderBy(k => k.nazwa_kategorii);
        }

        public int PoliczKategorie()
        {
            return _repo.GetKategorie().Count();
        }
    }
}
