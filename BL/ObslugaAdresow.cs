using IBL;
using IDAL;
using Model;

namespace BL
{
    public class ObslugaAdresow : IObslugaAdresow
    {
        private readonly IAdresyRepository AdresyRepo;

        public ObslugaAdresow(IAdresyRepository AdresyRepo)
        {
            this.AdresyRepo = AdresyRepo;
        }

        public IEnumerable<Adresy> PobierzPosortowaneAdresy()
        {
            return AdresyRepo.GetAdresy().OrderBy(g => g.miasto);
        }

        public int PoliczAdresy()
        {
            return AdresyRepo.GetAdresy().Count();
        }

        public IEnumerable<Adresy> PobierzAdresyZWielkimiMiastami(int minimalnaDlugoscNazwyMiasta)
        {
            return AdresyRepo.GetAdresy().Where(a => a.miasto != null && a.miasto.Length >= minimalnaDlugoscNazwyMiasta);
        }

        public IEnumerable<Adresy> PobierzAdresyBezNumeruMieszkania()
        {
            return AdresyRepo.GetAdresy().Where(a => a.nr_mieszkania == 0);
        }

        public IEnumerable<Adresy> PobierzAdresyDlaUlicy(string ulica)
        {
            return AdresyRepo.GetAdresy().Where(a => a.ulica != null && a.ulica.Equals(ulica, StringComparison.OrdinalIgnoreCase));
        }
    }
}
