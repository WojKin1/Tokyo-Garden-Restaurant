using Model;

namespace IBL
{
    public interface IObslugaAdresow
    {
        IEnumerable<Adresy> PobierzPosortowaneAdresy();

        int PoliczAdresy();

    }
}
