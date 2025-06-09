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

        public IEnumerable<Kategoria> WyszukajKategorie(string fragmentNazwy)
        {
            if (string.IsNullOrWhiteSpace(fragmentNazwy))
                return PobierzKategorie();

            return _repo.GetKategorie().Where(k => k.nazwa_kategorii.Contains(fragmentNazwy, StringComparison.OrdinalIgnoreCase)).OrderBy(k => k.nazwa_kategorii);
        }

        public bool CzyKategoriaIstnieje(string nazwa)
        {
            if (string.IsNullOrWhiteSpace(nazwa))
                return false;

            return _repo.GetKategorie().Any(k => k.nazwa_kategorii.Equals(nazwa, StringComparison.OrdinalIgnoreCase));
        }
    }
}
