using IBL;
using IDAL;
using Model;

namespace BL
{
    public class ObslugaAlergenow : IObslugaAlergenow
    {
        private readonly IAlergenyRepository AlergenRepo;

        public ObslugaAlergenow(IAlergenyRepository repo)
        {
            AlergenRepo = repo;
        }

        public IEnumerable<Alergeny> PobierzPosortowaneAlergeny()
        {
            return AlergenRepo.GetAlergeny().OrderBy(a => a.nazwa);
        }

        public int PoliczAlergeny()
        {
            return AlergenRepo.GetAlergeny().Count();
        }
    }
}
