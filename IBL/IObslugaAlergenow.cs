using Model;

namespace IBL
{
    public interface IObslugaAlergenow
    {
        IEnumerable<Alergeny> PobierzPosortowaneAlergeny();
        int PoliczAlergeny();
    }
}
