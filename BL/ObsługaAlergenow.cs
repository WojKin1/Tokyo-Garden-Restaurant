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

        public Alergeny PobierzAlergenPoID(int alergenId)
        {
            return AlergenRepo.GetAlergenyByID(alergenId);
        }

        public void DodajAlergen(Alergeny alergen)
        {
            AlergenRepo.InsertAlergen(alergen);
            AlergenRepo.Save();
        }

        public void UsunAlergen(int alergenId)
        {
            AlergenRepo.DeleteAlergen(alergenId);
            AlergenRepo.Save();
        }

        public void AktualizujAlergen(Alergeny alergen)
        {
            AlergenRepo.UpdateAlergen(alergen);
            AlergenRepo.Save();
        }
    }
}