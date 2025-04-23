using IBL;
using IDAL;
using Model;

namespace BL
{
    public class ObslugaAdresow : IObslugaAdresow
    {
        IAdresyRepository AdresyRepo;

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
    }
}
