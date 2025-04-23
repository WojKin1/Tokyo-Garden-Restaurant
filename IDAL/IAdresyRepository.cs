using Model;

namespace IDAL
{
        public interface IAdresyRepository : IDisposable
        {
            IEnumerable<Adresy> GetAdresy();
            Adresy GetAdresyByID(int adresId);
            void InsertAdres(Adresy adres);
            void DeleteAdres(int adresId);
            void UpdateAdres(Adresy adres);
            void Save();
        }
   
}
