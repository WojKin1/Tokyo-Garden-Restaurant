using Model;

namespace IDAL
{
    public interface IAlergenyRepository : IDisposable
    {
        IEnumerable<Alergeny> GetAlergeny();
        Alergeny GetAlergenyByID(int alergenId);
        void InsertAlergen(Alergeny alergen);
        void DeleteAlergen(int alergenId);
        void UpdateAlergen(Alergeny alergen);
        void Save();
    }
}

