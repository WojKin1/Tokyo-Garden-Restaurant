using Model;

namespace IBL
{
    public interface IObslugaAlergenow
    {
        IEnumerable<Alergeny> PobierzPosortowaneAlergeny();
        int PoliczAlergeny();
        Alergeny PobierzAlergenPoID(int alergenId);
        void DodajAlergen(Alergeny alergen);
        void UsunAlergen(int alergenId);
        void AktualizujAlergen(Alergeny alergen);
    }
}
}